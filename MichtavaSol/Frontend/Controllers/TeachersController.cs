using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Common;
using Entities.Models;
using FileHandler;
using Services.Interfaces;
using Frontend.Models;

namespace Frontend.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly ITextService _textService;

        private readonly IFileManager _fileManager = new FileManager();

        private Dictionary<string, string> _subjectsDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _textsDictionary = new Dictionary<string, string>();


        public TeachersController(ISubjectService subjectService, ITextService textService)
        {
            _subjectService = subjectService;
            _textService = textService;
        }

        // GET: Teachers
        public ActionResult Index()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("TeachersMenu");
        }

        public ActionResult NavigateToTextAdding()
        {
            ViewBag.Title = "בחר אפשרות";

            InitializeSubjects();

            return View("TextAdding");
        }

        public ActionResult NavigateToTextsView()
        {
            ViewBag.Title = "רשימת טקסטים";

            InitializeSubjects();

            return View("TextsView");
        }

        [HttpPost]
        public ActionResult ShowTexts(string CurrentSubject)
        {
            ViewBag.Title = "רשימת טקסטים";

            InitializeSubjects();

            //string subject = Request.Form["CurrentSubject"];
            if (!string.IsNullOrEmpty(CurrentSubject))
            {
                Session["CurrentSubject"] = CurrentSubject;
                InitializeTexts(CurrentSubject);
                TempData["msg"] = CurrentSubject;
            }

            return View("TextsView");
        }

        private void InitializeTexts(string subject)
        {
            foreach (var txt in _textService.All())
            {
                if (!string.IsNullOrEmpty(subject) && txt.Subject.Name.Equals(subject))
                {
                    _textsDictionary[txt.Id + "txt"] = txt.Name;
                    TempData[txt.Id + "txt"] = txt.Name;
                }
            }
        }

        private void InitializeSubjects()
        {
            foreach (var subject in _subjectService.All())
            {
                _subjectsDictionary["" + subject.Id] = subject.Name;
                TempData["" + subject.Id] = subject.Name;
            }
        }

        public ActionResult NavigateToPolicy()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("Policy");
        }

        public ActionResult SubmitPolicy(PolicyViewModel model)
        {
            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
            }

            return View("Policy");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitText(TextViewModel model)
        {
            InitializeSubjects();

            if (string.IsNullOrEmpty(model.Name))
            {
                TempData["msg"] = "<script>alert('אנא בחר/י טקסט');</script>";
                return View("TextAdding");
            }

            string txtSubjectID = _subjectsDictionary.FirstOrDefault(x => x.Value == model.SubjectID).Key;
            Subject subject = _subjectService.GetById(new Guid(txtSubjectID));//small change here (int-> Guid)

            Text txt = _fileManager.UploadText(Server.MapPath("~/uploads"), subject, model.Name,
                Request.Form["filecontents"]);                       

            _textService.Add(txt);

            TempData["msg"] = "<script>alert('הטקסט הועלה בהצלחה    : )');</script>";            

            return View("TextAdding");
        }

        [HttpPost]
        public ActionResult RemoveText(string CurrentText)
        {
            ViewBag.Title = "רשימת טקסטים";

            InitializeSubjects();
            InitializeTexts(Session["CurrentSubject"].ToString());

            var txtKey = _textsDictionary.FirstOrDefault(x => x.Value == CurrentText).Key;
            txtKey = txtKey.Substring(0, txtKey.Length - 3);

            _textService.Delete(_textService.GetById(new Guid(txtKey)));

            // TODO: handle situation where Texts have the same name

            TempData["msg"] = "<script>alert('הטקסט נמחק בהצלחה');</script>";

            return View("TextsView");
        }

        public ActionResult NavigateToAddSubject()
        {
            return View("AddSubject");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(AddSubjectViewModel model)
        {
            Subject subject = new Subject();
            subject.Name = model.SubjectName;

            if (_subjectService.GetByName(model.SubjectName)==null)
                _subjectService.Add(subject);

            //else display some nice error message
                

            return View();
        }

    }
}
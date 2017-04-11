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

            int i = 0;
            string key = "key" + i;
            foreach (var subject in _subjectService.All())
            {
                TempData[key] = subject.Name;
                i++;
                key = "key" + i;
            }

            return View("TextAdding");
        }

        public ActionResult NavigateToPolicy()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("Policy");
        }

        public ActionResult SubmitPolicy()
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
            if (string.IsNullOrEmpty(model.Name))
                return View("TextAdding");

            Text txt = new Text();
            txt.Name = model.Name;
            txt.UploadTime = DateTime.Now;
            //txt.FilePath = Path.Combine(Server.MapPath("~/uploads"), txt.Name);
            //txt.Subject = _subjectService.GetById(1);

            //TODO: get file path and subject

            //_textService.Add(txt);

            return View("TextAdding");
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

            if (!_subjectService.All().Contains(subject))
                _subjectService.Add(subject);

            return View();
        }
    }
}
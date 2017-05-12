using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
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
        private readonly ISchoolClassService _classService;
        private readonly IStudentService _studentService;
        private readonly IHomeworkService _homeworkService;

        private readonly IFileManager _fileManager = new FileManager();

        private Dictionary<string, string> _subjectsDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _textsDictionary = new Dictionary<string, string>();


        public TeachersController(ISubjectService subjectService, ITextService textService, ISchoolClassService classService, IStudentService studentService, IHomeworkService homeworkService)
        {
            _subjectService = subjectService;
            _textService = textService;
            _classService = classService;
            _studentService = studentService;
            _homeworkService = homeworkService;
        }

        // GET: Teachers
        public ActionResult Index()
        {
            InitializeClasses();

            ViewBag.Title = "בחר/י כיתה";

            return View("TeachersMenu");
        }

        public ActionResult NavigateToTextAdding(string CurrentSubject)
        {
            ViewBag.Title = CurrentSubject;

            InitializeSubjects();
            TempData["Subject"] = CurrentSubject;

            return View("TextAdding");
        }

        public ActionResult NavigateToTextsView(string subject)
        {
            ViewBag.Title = "רשימת טקסטים";

            InitializeSubjects();

            if (string.IsNullOrEmpty(subject))
            {
                subject = Session["CurrentSubject"].ToString();
            }

            Session["CurrentSubject"] = subject;
            InitializeTexts(subject);
            TempData["CurrentSubject"] = subject;

            return View("TextsView");
        }

        private void InitializeTexts(string subject)
        {
            _textsDictionary.Clear();
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
            _subjectsDictionary.Clear();
            foreach (var subject in _subjectService.All())
            {
                _subjectsDictionary["" + subject.Id] = subject.Name;
                TempData["" + subject.Id] = subject.Name;
            }
        }

        public ActionResult NavigateToPolicy(string text)
        {
            ViewBag.Title = "בחר אפשרות";

            if (string.IsNullOrEmpty(text))
            {
                InitializeTextsViewPage();
                TempData["msg"] = "<script>alert('בחר/י טקסט עליו תסתמך המטלה');</script>";
                return View("TextsView");
            }

            Text textForHomework = getText(text);
            TempData["TextForHomework"] = textForHomework.Content;

            return View("Policy");
        }

        private Text getText(string text)
        {
            foreach (var txt in _textService.All())
            {
                if (txt.Name.Equals(text))
                    return txt;
            }

            return null;
        }

        public ActionResult SubmitPolicy(PolicyViewModel model, string text)
        {
            TempData["TextForHomework"] = text;

            if (model == null || string.IsNullOrEmpty(model.Question))
            {
                TempData["msg"] = "<script>alert('לא הכנסת שאלה');</script>";
                return View("Policy");
            }

            Policy policy = new Policy()
            {
                MaxConnectors = model.MaxConnectors,
                MinConnectors = model.MinConnectors,
                MaxWords = model.MaxWords,
                MinWords = model.MinWords,
                //KeySentences = new HashSet<string>(model.KeySentences)
            };

            Question question = new Question()
            {
                Content = model.Question,
                Policy = policy,
                Date_Added = DateTime.Now,
                Suggested_Openings = SuggestedOpening.convert(model.KeySentences),
                Question_Number = 0 // TODO: add question number
            };

           /* var homework = new Homework()
            {
                Text = null,
                Text_Id = new Guid(),
                Questions = null,
                Deadline = DateTime.Now,
                Created_By = null,
                Teacher_Id = new Guid()    
            };*/

            //_homeworkService.Add(homework);
            // TODO: add the question and the policy to DB

            TempData["msg"] = "<script>alert('השאלה נוספה למערכת בהצלחה');</script>";

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

            var subjectName = string.IsNullOrEmpty(model.SubjectID) ? Session["CurrentSubject"] : model.SubjectID;
            Session["CurrentSubject"] = subjectName;

            string txtSubjectID = _subjectsDictionary.FirstOrDefault(x => x.Value.Equals(subjectName)).Key;
            var txtFinalSubjectID = new Guid(txtSubjectID);
            Subject subject = _subjectService.GetById(txtFinalSubjectID); //small change here (int-> Guid)

            Text txt = _fileManager.UploadText(Server.MapPath("~/uploads"), subject, model.Name,
                Request.Form["filecontents"]);
            txt.Subject_Id = txtFinalSubjectID;

            _textService.Add(txt);

            TempData["msg"] = "<script>alert('הטקסט הועלה בהצלחה    : )');</script>";


            ViewBag.Title = "רשימת טקסטים";

            InitializeSubjects();
            InitializeTexts(Session["CurrentSubject"].ToString());
            TempData["CurrentSubject"] = Session["CurrentSubject"];

            return View("TextsView");
        }

        private void InitializeTextsViewPage()
        {
            InitializeSubjects();
            InitializeTexts(Session["CurrentSubject"].ToString());
        }

        [HttpPost]
        public ActionResult RemoveText(string CurrentText)
        {
            ViewBag.Title = "רשימת טקסטים";

            InitializeTextsViewPage();

            if (string.IsNullOrEmpty(CurrentText))
            {                
                TempData["msg"] = "<script>alert('לא סומן טקסט למחיקה');</script>";
                return View("TextsView");
            }

            var txtKey = _textsDictionary.FirstOrDefault(x => x.Value == CurrentText).Key;
            txtKey = txtKey.Substring(0, txtKey.Length - 3);

            // TODO: handle situation where Texts have the same name
            _textService.Delete(_textService.GetById(new Guid(txtKey)));

            TempData.Clear();
            InitializeTextsViewPage();

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

        private void InitializeClasses()
        {
            foreach (var cls in _classService.All())
            {
                TempData[cls.Id + ""] = cls.ClassLetter + " " + cls.ClassNumber;
            }
        }

        private SchoolClass GetClass(string className)
        {
            foreach (var cls in _classService.All().Include(x => x.Students))
            {
                if (className.Equals(cls.ClassLetter + " " + cls.ClassNumber))
                    return cls;
            }

            return null;
        }

        private void InitializeClassView(SchoolClass cls)
        {
            foreach (var std in cls.Students)
            {
                TempData[std.Id + "_student"] = std.Name;
            }

            foreach (var sbj in cls.Subjects)
            {
                TempData[sbj.Id + "_subject"] = sbj.Name;
            }
        }

        public ActionResult NavigateToClassView(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                className = Session["CurrentClass"].ToString();
            }

            ViewBag.Title = className;
            Session["CurrentClass"] = className;

            var currentClass = GetClass(className);
            InitializeClassView(currentClass);

            return View("ClassView");
        }

        private Student GetStudent(string student)
        {
            foreach (var std in _studentService.All())
            {
                if (std.Name.Equals(student))
                    return std;
            }

            return null;
        }

        private void InitializeStudentView(Student std)
        {
            //TempData["Name"] = std.Name;
            TempData["userName"] = "שם משתמש: " + std.ApplicationUser.UserName;
            TempData["email"] = "אימייל: " + std.ApplicationUser.Email;
            TempData["phoneNumber"] = "טלפון: " + std.ApplicationUser.PhoneNumber;

            if (std.SchoolClass == null)
            {
                TempData["class"] = "כיתה: אין";
            }
            else
            {
                TempData["class"] = "כיתה: " + std.SchoolClass.ClassLetter + " " + std.SchoolClass.ClassNumber;
            }   

            foreach (var hw in std.Homeworks)
            {
                TempData[hw.Id + "_homework"] = "שיעורי בית: " + hw.Title + ", תאריך הגשה: " + hw.Deadline;
            }
        }

        public ActionResult NavigateToStudentView(string student)
        {
            ViewBag.Title = student;

            InitializeStudentView(GetStudent(student));

            return View("StudentView");
        }
    }
}
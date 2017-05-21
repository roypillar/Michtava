using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Entities.Models;
using FileHandler;
using Services.Interfaces;
using Frontend.Models;
using Microsoft.AspNet.Identity;

namespace Frontend.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ISubjectService _subjectService;
        private readonly ITextService _textService;
        private readonly ISchoolClassService _classService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IHomeworkService _homeworkService;
        private readonly IAnswerService _answerService;

        private readonly IFileManager _fileManager = new FileManager();

        private Dictionary<string, string> _subjectsDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _textsDictionary = new Dictionary<string, string>();


        public TeachersController(ISubjectService subjectService, ITextService textService,
            ISchoolClassService classService, IStudentService studentService, IHomeworkService homeworkService,
            ITeacherService teacherService, IAnswerService answerService)
        {
            _subjectService = subjectService;
            _textService = textService;
            _classService = classService;
            _studentService = studentService;
            _homeworkService = homeworkService;
            _teacherService = teacherService;
            _answerService = answerService;
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

            Teacher currentTeacher = GetCurrentUser();
            var currentTeacherId = currentTeacher.Id;

            Text textForHomework = getText(text);
            Guid currentTextId = textForHomework.Id;

            TempData["textName"] = textForHomework.Name;
            TempData["TextContent"] = textForHomework.Content;

            var currentHomework = _fileManager.GetCurrentHomework(Server.MapPath("~/TemporaryFiles/Homeworks"),
                currentTeacherId, currentTextId);
            if (currentHomework != null && currentHomework.Count > 0)
            {
                InitializeHomework(currentHomework);
            }

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

        public ActionResult SubmitPolicy(PolicyViewModel model, string textName, string Submit, DateTime? submissionDate)
        {
            Teacher currentTeacher = GetCurrentUser();
            var currentTeacherId = currentTeacher.Id;

            Text textForHomework = getText(textName);
            Guid currentTextId = textForHomework.Id;

            TempData["textName"] = textForHomework.Name;
            TempData["TextContent"] = textForHomework.Content;

            if (Submit.Equals("הוספת שיעורי הבית"))
            {
                if (submissionDate == null)
                {
                    TempData["msg"] = "<script>alert('לא הוכנס תאריך הגשה לשיעורי הבית');</script>";
                }
                else
                {
                    SubmitHomework(textForHomework, currentTeacher, currentTeacherId, currentTextId, (DateTime)submissionDate);
                }               

                return View("Policy");
            }

            Dictionary<int, string> currentHomework;
            if (string.IsNullOrEmpty(model?.Question))
            {
                currentHomework = _fileManager.GetCurrentHomework(Server.MapPath("~/TemporaryFiles/Homeworks"),
                    currentTeacherId, currentTextId);
                if (currentHomework != null && currentHomework.Count > 0)
                {
                    InitializeHomework(currentHomework);
                }

                TempData["msg"] = "<script>alert('לא הכנסת שאלה');</script>";
                return View("Policy");
            }

            Policy policy = new Policy()
            {
                MaxConnectors = model.MaxConnectors,
                MinConnectors = model.MinConnectors,
                MaxWords = model.MaxWords,
                MinWords = model.MinWords,
            };

            Question question = new Question()
            {
                Content = model.Question,
                Policy = policy,
                Date_Added = DateTime.Now,
                Suggested_Openings = SuggestedOpening.convert(model.KeySentences),
                Question_Number =
                    _fileManager.GetNextQuestionNumber(Server.MapPath("~/TemporaryFiles/Homeworks"), currentTeacherId,
                        currentTextId)
            };

            _fileManager.SaveQuestion(Server.MapPath("~/TemporaryFiles/Homeworks"), question, currentTeacherId,
                currentTextId);

            currentHomework = _fileManager.GetCurrentHomework(Server.MapPath("~/TemporaryFiles/Homeworks"),
                currentTeacherId, currentTextId);
            if (currentHomework != null && currentHomework.Count > 0)
            {
                InitializeHomework(currentHomework);
            }

            TempData["msg"] =
                "<script>alert('השאלה נוספה למערכת. רק כאשר תוסיף/י את שיעורי הבית, התלמידים יוכלו לראות את השאלה');</script>";

            return View("Policy");
        }

        private void SubmitHomework(Text textForHomework, Teacher currentTeacher, Guid currentTeacherId,
            Guid currentTextId, DateTime submissionDate)
        {
            var questions = _fileManager.ParseQuestions(Server.MapPath("~/TemporaryFiles/Homeworks"), currentTeacherId, currentTextId);

            if (questions == null || questions.Count == 0)
            {
                TempData["msg"] = "<script>alert('יש להוסיף שאלות לשיעורי הבית');</script>";
                return;
            }

            var homework = new Homework()
            {
                Text = textForHomework,
                Text_Id = currentTextId,
                Questions = questions,
                Deadline = submissionDate,
                Created_By = currentTeacher,
                Teacher_Id = currentTeacherId
            };

            // Add Homework to relevant tables in DB:
            var currentClass = GetClass(Session["CurrentClass"].ToString());
            var currentClassId = currentClass.Id;
            try
            {
                _classService.GetById(currentClassId).Homeworks.Add(homework);
                foreach (var student in _classService.GetById(currentClassId).Students)
                {
                    student.Homeworks.Add(homework);
                }
                
                _homeworkService.Add(homework);                
            }
            catch (Exception e)
            {
                TempData["msg"] = "<script>alert('אירעה תקלה בהוספת שיעורי הבית');</script>";
                return;
            }

            TempData["msg"] = "<script>alert('שיעורי הבית נוספו בהצלחה, כעת התלמידים יוכלו לראות אותם');</script>";
            _fileManager.ClearTemporaryQuestions(Server.MapPath("~/TemporaryFiles/Homeworks"), currentTeacherId, currentTextId);
        }

        private void InitializeHomework(Dictionary<int, string> currentHomework)
        {
            foreach (var question in currentHomework)
            {
                TempData["שאלה " + question.Key] = question.Value;
            }
        }

        private Teacher GetCurrentUser()
        {
            var userId = User.Identity.GetUserId();
            foreach (var teacher in _teacherService.All())
            {
                if (teacher.ApplicationUserId.Equals(userId))
                {
                    return teacher;
                }
            }

            return null;
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

        public ActionResult NavigateToAnswersView()
        {
            InitializeAnswers();

            return View("AnswersView");
        }

        private void InitializeAnswers()
        {
            foreach (var answer in _answerService.All())
            {
                if (answer.Answer_To != null && answer.Answer_To.Deadline <= DateTime.Now && string.IsNullOrEmpty(answer.TeacherFeedback))
                {
                    TempData[answer.Student_Id + "_hw"] = _studentService.GetById(answer.Student_Id) + answer.Answer_To.Title;
                }
            }
        }
    }
}
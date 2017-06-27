using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Common;
using Entities.Models;
using FileHandler;
using Services.Interfaces;
using Frontend.Models;
using Microsoft.Ajax.Utilities;
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
        private readonly IWordDefinitionService _wordDefinitionService;

        private readonly IFileManager _fileManager = new FileManager();

        private Dictionary<string, string> _subjectsDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _textsDictionary = new Dictionary<string, string>();


        public TeachersController(ISubjectService subjectService, ITextService textService,
            ISchoolClassService classService, IStudentService studentService, IHomeworkService homeworkService,
            ITeacherService teacherService, IAnswerService answerService, IWordDefinitionService wordDefinitionService)
        {
            _subjectService = subjectService;
            _textService = textService;
            _classService = classService;
            _studentService = studentService;
            _homeworkService = homeworkService;
            _teacherService = teacherService;
            _answerService = answerService;
            _wordDefinitionService = wordDefinitionService;
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

            LoadHomework(currentTeacherId, currentTextId);

            return View("Policy");
        }

        private void LoadHomework(Guid currentTeacherId, Guid currentTextId)
        {
            var currentHomework = _fileManager.GetCurrentHomework(Server.MapPath("~/TemporaryFiles/Homeworks"),
                currentTeacherId, currentTextId);
            if (currentHomework != null && currentHomework.Count > 0)
            {
                InitializeHomework(currentHomework);
            }
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

        public ActionResult SubmitPolicy(PolicyViewModel model, string textName, string Submit, DateTime? submissionDate,
            string homeworkTitle, string homeworkDescription, string word, string wordDefinition)
        {
            Teacher currentTeacher = GetCurrentUser();
            var currentTeacherId = currentTeacher.Id;

            Text textForHomework = getText(textName);
            Guid currentTextId = textForHomework.Id;

            TempData["textName"] = textForHomework.Name;
            TempData["TextContent"] = textForHomework.Content;

            Dictionary<int, string> currentHomework = null;
                currentHomework = _fileManager.GetCurrentHomework(Server.MapPath("~/TemporaryFiles/Homeworks"),
                    currentTeacherId, currentTextId);
            if (currentHomework != null && currentHomework.Count > 0)
            {
                InitializeHomework(currentHomework);
            }

            if (Submit.Equals("הוספת שיעורי הבית"))
            {
                SubmitHomework(textForHomework, currentTeacher, currentTeacherId, currentTextId, submissionDate,
                    homeworkTitle, homeworkDescription);

                return View("Policy");
            }
            if (Submit.Equals("הוספת פירוש"))
            {
                if (word.IsNullOrWhiteSpace() || wordDefinition.IsNullOrWhiteSpace())
                {
                    TempData["msg"] = "<script>alert('לא הוכנס ערך');</script>";
                }
                else
                {
                    AddWordDefinition(word, wordDefinition);
                }
                return View("Policy");
            }

            currentHomework?.Clear();
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

        private void AddWordDefinition(string word, string wordDefinition)
        {
            if (_wordDefinitionService.All().Count(x => x.Word.Equals(word)) > 0)
            {
                TempData["msg"] = "<script>alert('כבר קיים במערכת פירוש למילה המבוקשת');</script>";
                return;
            }
            WordDefinition wd = new WordDefinition()
            {
                Word = word,
                Definition = wordDefinition,

            };
            try
            {
                _wordDefinitionService.Add(wd);
                TempData["msg"] = "<script>alert('פירוש המילה נכנס למערכת בהצלחה');</script>";
            }
            catch (Exception)
            {

                TempData["msg"] = "<script>alert('אירעה תקלה בניסיון הוספת פירוש המילה');</script>";
            }
        }

        private void SubmitHomework(Text textForHomework, Teacher currentTeacher, Guid currentTeacherId,
            Guid currentTextId, DateTime? submissionDate, string homeworkTitle, string homeworkDescription)
        {
            var questions = _fileManager.ParseQuestions(Server.MapPath("~/TemporaryFiles/Homeworks"), currentTeacherId,
                currentTextId);

            if (questions == null || questions.Count == 0)
            {
                TempData["msg"] = "<script>alert('יש להוסיף שאלות לשיעורי הבית');</script>";
                return;
            }
            if (submissionDate == null)
            {
                TempData["msg"] = "<script>alert('לא הוכנס תאריך הגשה לשיעורי הבית');</script>";
                return;
            }

            var homework = new Homework()
            {
                Text = textForHomework,
                Text_Id = currentTextId,
                Questions = questions,
                Deadline = (DateTime)submissionDate,
                Created_By = currentTeacher,
                Teacher_Id = currentTeacherId,
                Title = homeworkTitle,
                Description = homeworkDescription
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

            _fileManager.ClearTemporaryQuestions(Server.MapPath("~/TemporaryFiles/Homeworks"), currentTeacherId,
                currentTextId);
            TempData.Clear();
            TempData["msg"] = "<script>alert('שיעורי הבית נוספו בהצלחה, כעת התלמידים יוכלו לראות אותם');</script>";
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
            ViewBag.Title = Session["CurrentClass"];

            return View("AddSubject");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(AddSubjectViewModel model)
        {
            Subject subject = new Subject();
            subject.Name = model.SubjectName;

            if (subject.Name.IsNullOrWhiteSpace())
            {
                TempData["msg"] = "<script>alert('לא הוכנס נושא');</script>";
                return View();
            }

            if (_subjectService.GetByName(model.SubjectName) == null)
            {
                try
                {
                    _subjectService.Add(subject);
                    foreach (var cls in _classService.All().Include(x => x.Subjects).ToList())
                    {
                        var className = cls.ClassLetter + " " + cls.ClassNumber;
                        if (className.Equals(Session["CurrentClass"].ToString()))
                        {
                            cls.Subjects.Add(subject);
                            _classService.Update(cls);
                        }
                    }
                }
                catch (Exception)
                {
                    TempData["msg"] = "<script>alert('אירעה תקלה בהוספת הנושא');</script>";
                    return View();
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('הנושא המבוקש כבר קיים');</script>";
                return View();
            }

            SchoolClass schoolClass = GetClass(Session["CurrentClass"].ToString());
            InitializeClassView(schoolClass);

            TempData["msg"] = "<script>alert('הנושא נוסף בהצלחה');</script>";
            return View("ClassView");
        }

        private void InitializeClasses()
        {
            foreach (var cls in _classService.All().Include(c => c.Homeworks).ToList())
            {
                if (ClassHasHomeworkToCheck(cls))
                {
                    TempData[cls.Id + "_notification"] = cls.ClassLetter + " " + cls.ClassNumber;
                }
                else
                {
                    TempData[cls.Id + "_class"] = cls.ClassLetter + " " + cls.ClassNumber;
                }               
            }
        }

        private bool ClassHasHomeworkToCheck(SchoolClass cls)
        {            
            foreach (var clsHomework in cls.Homeworks)
            {
                var allAnswersFeedbacked = true;
                foreach (var answer in _answerService.All().Where(a => a.Homework_Id.Equals(clsHomework.Id)).ToList())
                {
                    if (answer.TeacherFeedback.IsNullOrWhiteSpace())
                    {
                        allAnswersFeedbacked = false;
                        break;
                    }
                }

                if (clsHomework.Deadline < DateTime.Now && !allAnswersFeedbacked)
                {
                    return true;
                }
            }
            return false;
        }

        private SchoolClass GetClass(string className)
        {
            foreach (
                var cls in _classService.All().Include(x => x.Students).Include(x => x.Homeworks.Select(t => t.Text)).Include(x => x.Subjects))
            {
                if (className.Equals(cls.ClassLetter + " " + cls.ClassNumber))
                    return cls;
            }

            return null;
        }

        private void InitializeClassView(SchoolClass cls)
        {
            foreach (var std in cls.Students.ToList())
            {
                TempData[std.Id + "_student"] = std.Name;
            }

            foreach (var sbj in cls.Subjects.ToList())
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

            if (ClassHasHomeworkToCheck(currentClass))
            {
                TempData["HomeworkNotification"] = "true";
            }

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

        public ActionResult NavigateToHomeworkView()
        {
            InitializeHomeworks();

            ViewBag.Title = "שיעורי בית:";

            return View("HomeworkView");
        }

        private void InitializeHomeworks()
        {
            foreach (var classHW in GetClass(Session["CurrentClass"].ToString()).Homeworks.ToList())
            {
                if (classHW.Deadline < DateTime.Now)
                {
                    TempData[classHW.Id + "_hw"] = classHW.Text.Name;
                }
            }
        }

        public ActionResult NavigateToAnswersView(string homeworkText)
        {
            ViewBag.Title = "בחר/י תלמיד על מנת לצפות בתשובות";

            Session["HomeworkText"] = homeworkText;

            InitializeStudentAnswers();
            InitializQuestions();

            return View("AnswersView");
        }

        private void InitializeStudentAnswers()
        {
            foreach (var std in GetClass(Session["CurrentClass"].ToString()).Students.ToList())
            {
                foreach (var answer in _answerService.All().Include(a => a.Answer_To).ToList())
                {
                    if ((answer.Student_Id.Equals(std.Id) && answer.Answer_To.Text.Name.Equals(Session["HomeworkText"])))
                    {
                        if (answer.TeacherFeedback.IsNullOrWhiteSpace())
                        {
                            TempData[std.Id + "_student"] = std.Name;
                        }
                        else
                        {
                            TempData[std.Id + "_feedbacked"] = std.Name;
                        }
                    }                   
                }                
            }
        }

        public ActionResult ShowAnswer(string student)
        {
            ViewBag.Title = student;
            Session["CurrentStudentsAnswer"] = student;

            InitializeStudentAnswers();
            InitizlizeAnswer(student);
            InitializQuestions();

            return View("AnswersView");
        }

        private void InitializQuestions()
        {
            foreach (var hw in _homeworkService.All().Include(x => x.Questions))
            {
                if (hw.Text != null && hw.Text.Name.Equals(Session["HomeworkText"]))
                {
                    foreach (var hwQuestion in hw.Questions)
                    {
                        TempData[hwQuestion.Id + "_homework"] += "מספר שאלה: " + hwQuestion.Question_Number + "\n";
                        TempData[hwQuestion.Id + "_homework"] += "שאלה: \n" + hwQuestion.Content + "\n\n";
                    }
                }
            }
        }

        private void InitizlizeAnswer(string student)
        {
            foreach (var answer in _answerService.All().Include(x => x.questionAnswers.Select(y => y.Of_Question)).ToList())
            {
                if (answer.Answer_To != null && _studentService.GetById(answer.Student_Id).Name.Equals(student) &&
                    answer.Answer_To.Text.Name.Equals(Session["HomeworkText"]))
                {
                    foreach (var questionAnswer in answer.questionAnswers.ToList())
                    {
                        TempData[answer.Student_Id + "_answer"] += "מספר שאלה: " +
                                                                   questionAnswer.Of_Question.Question_Number + "\n";
                        TempData[answer.Student_Id + "_answer"] += "תשובה: \n" + questionAnswer.Content + "\n\n";
                    }
                }
            }
        }

        public ActionResult SubmitFeedback(int finalGrade, string feedback)
        {
            InitializeStudentAnswers();
            InitializQuestions();

            if (Session["CurrentStudentsAnswer"] == null || Session["CurrentStudentsAnswer"].ToString().IsNullOrWhiteSpace())
            {
                TempData["msg"] = "<script>alert('יש לבחור תלמיד');</script>";
                return View("AnswersView");
            }
            if (feedback.IsNullOrWhiteSpace())
            {
                TempData["msg"] = "<script>alert('יש למלא את שדה הבדיקה');</script>";
                return View("AnswersView");
            }

            try
            {
                AddFeedback(Session["CurrentStudentsAnswer"].ToString(), finalGrade, feedback);
            }
            catch (Exception e)
            {

                TempData["msg"] = "<script>alert('אירעה תקלה בניסיון שליחת הבדיקה');</script>";
                return View("AnswersView");
            }

            TempData["msg"] = "<script>alert('הבדיקה נשלחה לתלמיד/ה בהצלחה');</script>";

            NotifyStudentForFeedback();

            TempData.Clear();
            InitializeStudentAnswers();
            InitializQuestions();

            return View("AnswersView");
        }

        private void NotifyStudentForFeedback()
        {
            Student _student = null;
            foreach (var std in GetClass(Session["CurrentClass"].ToString()).Students.ToList())
            {
                if (std.Name.Equals(Session["CurrentStudentsAnswer"]))
                {
                    _student = std;
                    _student.notifyForNewGrade = true;
                }
            }
            if (_student != null)
            {
                _studentService.Update(_student);
            }
        }

        private void AddFeedback(string student, int finalGrade, string feedback)
        {
            Answer newAnswer = null;
            foreach (var answer in _answerService.All().Include(x => x.questionAnswers.Select(y => y.Of_Question)).ToList())
            {
                if (answer.Answer_To != null && _studentService.GetById(answer.Student_Id).Name.Equals(student) && answer.Answer_To.Text.Name.Equals(Session["HomeworkText"]))
                {
                    newAnswer = answer;
                    newAnswer.TeacherFeedback = feedback;
                    newAnswer.Grade = finalGrade;                   
                }
            }
            if (newAnswer != null)
            {
                _answerService.Update(newAnswer);
            }            
        }
    }
}
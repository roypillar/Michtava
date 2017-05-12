using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Entities.Models;
using FileHandler;
using Frontend.Areas.Administration.Models;
using SmartTextBox;
using Frontend.Models;
using Services.Interfaces;
using Frontend.App_Start.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;


namespace Frontend.Controllers
{
    public class StudentsController : Controller
    {
        private IFileManager _fileManager = new FileManager();
        public static ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy = new Policy();
        private Homework _homework = new Homework();
        private SmartTextViewModel smartView = new SmartTextViewModel();
        private Text text = new Text();
        private Teacher teacher = new Teacher();
        private Student student = new Student();
        private SchoolClass schoolClass = new SchoolClass();
        private Subject subj = new Subject();
        private Guid schoolClassGuid;
        private Guid textGuid;

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private readonly ISubjectService _subjectServiceService;
        private readonly IHomeworkService _homeworkService;
        private readonly IStudentService _studentService;
        private readonly IAnswerService _answerService;
        private readonly ISchoolClassService _schoolClassService;
        private readonly ITextService _textService;
      //  private readonly iquestionsservice

        public StudentsController(ISubjectService subjectServiceService, IHomeworkService homeworkService, IAnswerService answerService, IStudentService studentService, ISchoolClassService schoolClassService, ITextService textService)
        {
            _textService = textService;
            _schoolClassService = schoolClassService;
            _subjectServiceService = subjectServiceService;
            _homeworkService = homeworkService;
            _answerService = answerService;
            _studentService = studentService;
            
           

            dictionary.Add("סירותיהם", "הסירות שלהם, פירוש מעניין..");
            dictionary.Add("נמרצות", "מלא מרץ, מלא חיות, אנרגטי");
            dictionary.Add("עמך", "יחד, בצוותא, בשיתוף; אחד עם השני");

           

        }

      
        // GET: Students
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All())
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            Session["StudentName"] = student.Name;

            foreach (var sch_cls in _schoolClassService.All())
            {
                if (sch_cls.Id.Equals(student.SchoolClass.Id))
                {
                    schoolClass = sch_cls;
                    break;
                }
            }

            schoolClassGuid = schoolClass.Id;

            ViewBag.Title = "בחר נושא";
    
            List<Subject> subjects = new List<Subject>();


       
            Session["SchoolClassID"] = schoolClass.Id.ToString();



            subjects = schoolClass.Subjects.ToList();

            return View("Subjects",subjects);
        }


   /*     public ActionResult ChooseSubject()
        {
            //ViewBag.Title = "בחר תת-נושא";

            return RedirectToAction("ChooseSubSubject");
        }
        */

        public ActionResult ChooseSubSubject(string subjName)
        {
            ViewBag.Title = "בחר טקסט";
            List<Text> texts = new List<Text>();

            Subject tempSubj = _subjectServiceService.GetByName(subjName);

            texts = _textService.All().Where(x => x.Subject_Id == tempSubj.Id).ToList();

            return View("Texts",texts);
        }

        public ActionResult ChooseText(string textName)
        {
            ViewBag.Title = "בחר פעולה";

            Session["textName"] = textName;
            

            return View("TextMenu");
        }


        public ActionResult ChooseAction(string submit)
        {
            Session["title"] = submit;

           // switch (submit)
           // {
              //  case "לסיפור":
            Session["WithQuestion?"] = "Without";

            Session["TextContent"] = _fileManager.GetText(text.FilePath);
                    //init words definition
            Session["WordsDefs"] = getWordDefinitionsForText(_fileManager.GetText(text.FilePath));
            string tmpName = (string)Session["textName"];
            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            textGuid = _textService.All().Where(t => t.Name == tmpName).FirstOrDefault().Id;

          /// .All().Include(x => x.Questions.Select(q=>q.Policy)).Where(x => x.Id == hw.Id)
              

            foreach (var hw in student.Homeworks)
            {
                if (hw.Text_Id == textGuid)
                {
                    Session["NoHomeWork"] = "1";
                    return View("TextView");
                }
            }
            Session["NoHomeWork"] = "0";

            return View("TextView");

        
        }


        public ActionResult GotoSmartTextBox(string questionNumber)
        {
            int tmpQuestNumber = 1;


            InitializePolicy();
            Session["WithQuestion?"] = "With";
            if (questionNumber != null)
            {
                string[] tmpStringArray = questionNumber.Split(' ');

                if (tmpStringArray.Length == 2)
                {
                    tmpQuestNumber = Int32.Parse(tmpStringArray[1]);
                }
                else
                    tmpQuestNumber = Int32.Parse(tmpStringArray[2]);

            }


            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
              //  TempData["toManyWords"] = "";
            }

            //InitializeSmartView();
            smartView.QuestionNumber = tmpQuestNumber;

            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x=>x.Homeworks))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            string tmpName = (string)Session["textName"];
            textGuid = _textService.All().Where(t => t.Name == tmpName).FirstOrDefault().Id;


          /// .All().Include(x => x.Questions.Select(q=>q.Policy)).Where(x => x.Id == hw.Id)
              

            foreach (var hw in student.Homeworks)
            {
                if(hw.Text_Id == textGuid)
                {
                    List<Question> tmpQuestionsList = _homeworkService.All().Include(x => x.Questions.Select(q=>q.Policy)).Include(x=>x.Questions.Select(q=>q.Suggested_Openings)).Where(x => x.Id == hw.Id).FirstOrDefault().Questions.ToList();
                    smartView.question = tmpQuestionsList.Where(x=>x.Question_Number == tmpQuestNumber).FirstOrDefault();
                    smartView.Questions = tmpQuestionsList;

                    if (smartView.question.Suggested_Openings.Count == 0)
                    {
                        SuggestedOpening noSuggOpen = new SuggestedOpening("אין משפטי פתיחה לשאלה זו");
                        SuggestedOpening noSuggOpen2 = new SuggestedOpening("התשובה לשאלה נמצאת בגוף השאלה");
                        smartView.question.Suggested_Openings.Add(noSuggOpen);
                        smartView.question.Suggested_Openings.Add(noSuggOpen2);

                    }


                    List<int> SmartViewQuestionsNumbers = new List<int>();

                    List<Answer> QuestionsAlreadyAnswered = _answerService.All().Where(x => x.Homework_Id == hw.Id && x.Student_Id == student.Id).ToList();
                   if (QuestionsAlreadyAnswered == null)
                   {



                   }
                    else
                    {
                        smartView.CompleteQuestions = QuestionsAlreadyAnswered;
                        foreach (var Ans in QuestionsAlreadyAnswered)
                        {
                            SmartViewQuestionsNumbers.Add(Ans.QuestionNumber);
                        }
                        smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;
                    }

                }
            }

                return View("QuestionsView", smartView);
             
        }

       
  /*      public List<Guid> GetAnswersNumbers(Homework hw)
        {
            List<Guid> tmp = new List<Guid>();
            foreach(var question in hw.Questions)
            {
                tmp.Add(question.Id);
            }
            return tmp;
        }

    */
        //todo- fix analyze answer like gotosmartTextbox, check for already answered questions and mark them or load the answer or dont present them?



        public ActionResult AnalyzeAnswer(string questionNumber)
        {
            int tmpQuestNumber = 1;

            if (questionNumber != null)
            {
                string[] tmpStringArray = questionNumber.Split(' ');

                if (tmpStringArray.Length == 2)
                {
                    tmpQuestNumber = Int32.Parse(tmpStringArray[1]);
                }
                else
                    tmpQuestNumber = Int32.Parse(tmpStringArray[2]);

            }


            string input = Request.Form["TextBoxArea"];
            int numOfWords = _smartTextBox.GetNumberOfWords(input);
            int numOfConnectors = _smartTextBox.GetNumberOfConnectors(input);
            IDictionary<string,int> repeatedWords = _smartTextBox.GetRepeatedWords(input);

            string repeatedWordsString = "";
            foreach(var word in repeatedWords)
            {
                if (word.Value > 2 && _smartTextBox.IsConnector(word.Key))
                {
                    repeatedWordsString = repeatedWordsString + "השתמשת במילה " + word.Key + " " + word.Value + " פעמים, אולי תרצה להשתמש במילה אחרת כמו " + _smartTextBox.SuggestAlternativeWord(word.Key) + ". ";
                }
            }


            TempData["NumberOfWords"] = numOfWords;
            TempData["NumberOfConnectorWords"] = numOfConnectors;
            TempData["Answer"] = input;
            TempData["AlternativeWords"] = repeatedWordsString;

            
            if (numOfConnectors > _policy.MaxConnectors)
            {
                TempData["toManyConnectors"] = "הכנסת " + numOfConnectors + " מילות קישור, אבל מותר לכל היותר " + _policy.MaxConnectors + " מילות קישור.";
            }


            smartView.QuestionNumber = tmpQuestNumber;
            smartView.QuestionNumber = tmpQuestNumber;

            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            string tmpName = (string)Session["textName"];
            textGuid = _textService.All().Where(t => t.Name == tmpName).FirstOrDefault().Id;


            /// .All().Include(x => x.Questions.Select(q=>q.Policy)).Where(x => x.Id == hw.Id)


            foreach (var hw in student.Homeworks)
            {
                if (hw.Text_Id == textGuid)
                {
                    List<Question> tmpQuestionsList = _homeworkService.All().Include(x => x.Questions.Select(q => q.Policy)).Where(x => x.Id == hw.Id).FirstOrDefault().Questions.ToList();
                    smartView.question = tmpQuestionsList.Where(x => x.Question_Number == tmpQuestNumber).FirstOrDefault();
                    smartView.Questions = tmpQuestionsList;

                    if (smartView.question.Suggested_Openings.Count == 0)
                    {
                        SuggestedOpening noSuggOpen = new SuggestedOpening("אין משפטי פתיחה לשאלה זו");
                        SuggestedOpening noSuggOpen2 = new SuggestedOpening("התשובה לשאלה נמצאת בגוף השאלה");
                        smartView.question.Suggested_Openings.Add(noSuggOpen);
                        smartView.question.Suggested_Openings.Add(noSuggOpen2);

                    }


                }
            }


            return View("QuestionsView", smartView);

        }

       


        [System.Web.Services.WebMethod]
        public static int NumOfConnectors(string text)
        {
            return  _smartTextBox.GetNumberOfConnectors(text);
        }








        private void InitializePolicy()
        {
            _policy = new Policy();
           


            _policy = new Policy() {Id = _policy.Id , MinWords = 20, MaxWords = 30, MinConnectors = 3, MaxConnectors = 8};
        }

        private void InitializeSmartView()
        {


            smartView.question = getQuestionSample(3);
        

            // string text = _fileManager.GetText(@"C:\Users\mweiss\Desktop\Test.txt");
            // smartView.text = text;



            // IQueryable<SmartTextViewModel> home_works = _homeworkService.All().Project().To<SmartTextViewModel>();

            //smartView = home_works.GetEnumerator().First().To<SmartTextViewModel>();

        }

        public Question getQuestionSample(int i)
        {
            InitializePolicy();

            
            Question q = new Question(); //local question init
           // q.Id = i*10;
            q.Content = "שאלה לדוגמא שנשלוף מהבסיס נתונים, מהו מיהו וכד'.. עוד כמה דברים.. ענה בנימוק." + i + i + i + i;
            q.Policy = _policy;
            q.Question_Number = i;
            HashSet<string> _keySentencesList = new HashSet<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא " + i );
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא " + i*2);
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא " + i*3);
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא " + i*4);

            q.Suggested_Openings = SuggestedOpening.convert(_keySentencesList);
            return q;
        }


        private string getWordDefinitionsForText(string text)
        {
            Dictionary<string, string> wordDefinitionDictionary = new Dictionary<string, string>();

            string[] tokens = text.Split(' ', '.', ',', '-', '?', '!', '<', '>', '&', '[', ']', '(', ')');

            for (int i = 0; i < tokens.Length; i++)
            {
                
                try
                {
                    if (dictionary.TryGetValue(tokens[i], out string description))
                    {
                        wordDefinitionDictionary.Add(tokens[i], description);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            
            string wordsDefs = "";
            for (int i = 0; i < wordDefinitionDictionary.Count; i++)
            {
                wordsDefs = wordsDefs + "<strong>" + wordDefinitionDictionary.ElementAt(i).Key + " - " + "</strong>" + wordDefinitionDictionary.ElementAt(i).Value + " <br/> ";
            }
            return wordsDefs;
        }



        /// <summary>
        /// you don't need this anymore, all the data is seeded to the DB.
        /// </summary>
       
        
       
    }
}
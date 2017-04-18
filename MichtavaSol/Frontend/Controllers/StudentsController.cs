using System;
using System.Collections.Generic;
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

namespace Frontend.Controllers
{
    public class StudentsController : Controller
    {
        private IFileManager _fileManager = new FileManager();
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy = new Policy();
        private Homework _homework = new Homework();
        private SmartTextViewModel smartView = new SmartTextViewModel();
        private Text text = new Text();
        private Teacher teacher = new Teacher();
        private Student student = new Student();
        private SchoolClass schoolClass = new SchoolClass();
        private Subject subj = new Subject();

        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        private readonly ISubjectService _subjectServiceService;
        private readonly IHomeworkService _homeworkService;
        private readonly IStudentService _studentService;
        private readonly IAnswerService _answerService;

        public StudentsController(ISubjectService subjectServiceService, IHomeworkService homeworkService, IAnswerService answerService)
        {
            _subjectServiceService = subjectServiceService;
            _homeworkService = homeworkService;
            _answerService = answerService;
            initStudent();

            dictionary.Add("סירותיהם", "הסירות שלהם, פירוש מעניין..");
            dictionary.Add("נמרצות", "מלא מרץ, מלא חיות, אנרגטי");
            dictionary.Add("עמך", "יחד, בצוותא, בשיתוף; אחד עם השני");

        }

        // GET: Students
        public ActionResult Subjects()
        {
            ViewBag.Title = "בחר נושא";

            IQueryable<Subject> Allsubjects =
                _subjectServiceService.All();                   //this uses a mapping for AutoMapper
            List<Subject> subjects = new List<Subject>();
            
            foreach(Subject subj in Allsubjects)
            {
                try
                {
                    //***************************//
                    //still doing problems.. ask roi..
                    //***************************//
                    if (student.SchoolClass.Subjects.Contains(_subjectServiceService.GetByName("היסטוריה")))
                    {
                        subjects.Add(subj);
                    }

                }
                catch(Exception e)
                {
                    subjects.Add(subj);
                }


            }

            return View(subjects);
        }

        public ActionResult ChooseSubject()
        {
            ViewBag.Title = "בחר תת-נושא";

            return View("SubSubjects");
        }

        public ActionResult ChooseSubSubject()
        {
            ViewBag.Title = "בחר טקסט";

            return View("Texts");
        }

        public ActionResult ChooseText()
        {
            ViewBag.Title = "בחר פעולה";

            
            return View("TextMenu");
        }



        //להמשיך לטעון דינאמית מסטודנט את מה שצריך
        //להעביר כל מה שצריך מגו טו סמארט טקסט בוקס אל הטקטס ויו, ומאנאלייז.
        //לנסות לעבוד עם session
        //נשאר קבוע עד שישנו כמו למשל טקסט של הסיפור..
        //לקבל לפעולות כמו פה מידע ולעבוד איתו
        //להפוך את סמארט טקסט ויו מודל להחזיק שיעורי בית ומספר של שאלה וככה להציג אותה בתיבה החכמה..
        public ActionResult ChooseAction(string submit)
        {
            Session["title"] = submit;
            
            switch (submit)
            {
                case "לסיפור":
                    Session["WithQuestion?"] = "Without";

                    Session["TextContent"] = _fileManager.GetText(text.FilePath);
                    //init words definition
                    Session["WordsDefs"] = getWordDefinitionsForText(_fileManager.GetText(text.FilePath));
                    return View("TextView");

                case "משחקי אוצר מילים":
                    Session["WithQuestion?"] = "Without";

                    Session["TextContent"] = _fileManager.GetText(text.FilePath);
                    //init words definition
                    Session["WordsDefs"] = getWordDefinitionsForText(_fileManager.GetText(text.FilePath));
                    return View("TextView");
                    //for games an stuff..

            }

            return View("TextView");
        
        }


        public ActionResult GotoSmartTextBox(string questionNumber)
        {
            int tmpQuestNumber = 1;



            //************need to complete*************
            //init by specific policy... get the question policy from the real question..
            //************need to complete*************

            InitializePolicy();
            Session["WithQuestion?"] = "With";
            if (questionNumber != null)
            {
                tmpQuestNumber = Int32.Parse(questionNumber);
            }
            

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
                TempData["toManyWords"] = "";
            }

            //InitializeSmartView();
            smartView.QuestionNumber = tmpQuestNumber;
            smartView.question = student.Homeworks.First(x => x.Text == text).Questions.First(m => m.Question_Number == tmpQuestNumber);
            smartView.Questions = student.Homeworks.First(x => x.Text == text).Questions.Cast<Question>().ToList();

            //List<int> answersNumber = GetAnswersNumbers(student.Homeworks.First(x => x.Text == text));

            //smartView.CompleteQuestions = _answerService.All().Select(x => answersNumber.Contains(x.question.Id)).Cast<Answer>().ToList();


            return View("QuestionsView", smartView);
        }

       
        public List<int> GetAnswersNumbers(Homework hw)
        {
            List<int> tmp = new List<int>();
            foreach(var question in hw.Questions)
            {
                tmp.Add(question.Id);
            }
            return tmp;
        }




        public ActionResult AnalyzeAnswer(string questionNumber)
        {

            int tmpQuestNumber = Int32.Parse(questionNumber);


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

            if (numOfWords > _policy.MaxWords)
            {
                TempData["toManyWords"] = "הכנסת " + numOfWords + " מילים, אבל מותר לכל היותר " + _policy.MaxWords + " מילים.";
            }
            if (numOfConnectors > _policy.MaxConnectors)
            {
                TempData["toManyConnectors"] = "הכנסת " + numOfConnectors + " מילות קישור, אבל מותר לכל היותר " + _policy.MaxConnectors + " מילות קישור.";
            }


            smartView.QuestionNumber = tmpQuestNumber;
            smartView.question = student.Homeworks.First(x => x.Text == text).Questions.First(m => m.Question_Number == tmpQuestNumber);
            smartView.Questions = student.Homeworks.First(x => x.Text == text).Questions.Cast<Question>().ToList();


            return View("QuestionsView", smartView);

        }













        private void InitializePolicy()
        {
            _policy = new Policy();
            HashSet<string> _keySentencesList = new HashSet<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 1");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 2");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 3");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 4");


            _policy = new Policy() { Id = 1, MinWords = 20, MaxWords = 30, MinConnectors = 3, MaxConnectors = 8, KeySentences = _keySentencesList };
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
            q.Id = i*10;
            q.Content = "שאלה לדוגמא שנשלוף מהבסיס נתונים, מהו מיהו וכד'.. עוד כמה דברים.. ענה בנימוק." + i + i + i + i;
            q.Policy = _policy;
            q.Question_Number = i;
            HashSet<string> _keySentencesList = new HashSet<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 1");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 2");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 3");
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא 4");

            q.Suggested_Openings = _keySentencesList;
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



        public void initStudent()
        {
            student.Name = "student";
            teacher.Name = "teacher";
            
            subj.Name = "היסטוריה";
            text.Name = "הסיפור הישן על הספינה";
            text.FilePath = @"C:\Users\mweiss\Desktop\Test.txt";


            List<Subject> tmpSubList = new List<Subject>();
            tmpSubList.Add(subj);
                
            List<Student> tmpStudentList = new List<Student>();
            tmpStudentList.Add(student);

            schoolClass.Students = tmpStudentList;
            schoolClass.Subjects = tmpSubList;

            List<SchoolClass> tmpSchoolClassList = new List<SchoolClass>();
            tmpSchoolClassList.Add(schoolClass);


            teacher.SchoolClasses = tmpSchoolClassList;

            List<Question> qlist = new List<Question>();
            qlist.Add(getQuestionSample(1));
            qlist.Add(getQuestionSample(2));
            qlist.Add(getQuestionSample(3));
            qlist.Add(getQuestionSample(4));


            _homework.Text = text;
            _homework.SchoolClasses = tmpSchoolClassList;
            _homework.Created_By = teacher;
            _homework.Questions = qlist;

            List<Homework> tmpHwList = new List<Homework>();
            tmpHwList.Add(_homework);
            student.Homeworks = tmpHwList;
            student.SchoolClass = schoolClass;


        }
    }
}
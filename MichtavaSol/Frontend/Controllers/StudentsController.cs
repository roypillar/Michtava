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
            dictionary.Add("שטה", "מפליגה בים");
           // dictionary.Add("יפהפייה", "מהממת ביופיה");

           

        }

      
        // GET: Students
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks.Select(t=>t.Text)))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }
            

            Session["StudentName"] = student.Name;

      /*      if(student.notifyForNewHomework == true)
            {
                student.notifyForNewHomework = false;
                _studentService.Update(student);
            }

            if (student.notifyForNewGrade == true)
            {

            }
            */


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

            List<Guid> subjectsIDList = new List<Guid>();
            List<Tuple<string,string, Text>> tmpTexts = new List<Tuple<string,string, Text>>();
            subjects = schoolClass.Subjects.ToList();

            foreach(var hw in student.Homeworks)
            {
                if (subjects.Contains(hw.Text.Subject))
                {
                    subjectsIDList.Add(hw.Text.Subject_Id);
                    Tuple<string,string, Text> t = new Tuple<string,string,Text>(hw.Created_By.Name,hw.Deadline.ToString(),hw.Text);
                    tmpTexts.Add(t);
                }
            }

            SubjectsNotificationsViewModel model = new SubjectsNotificationsViewModel();
            model.Subjects = subjects;
            model.subjectsIDList = subjectsIDList;
            model.tmpTexts = tmpTexts;
            return View("Subjects",model);
        }


   /*     public ActionResult ChooseSubject()
        {
            //ViewBag.Title = "בחר תת-נושא";

            return RedirectToAction("ChooseSubSubject");
        }
        */

        public ActionResult ChooseSubSubject(string subjName)
        {
            ViewBag.Title = subjName;
            List<Text> texts = new List<Text>();
            List<Tuple<string, string, Text>> textTuple = new List<Tuple<string, string, Text>>();

            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks.Select(t => t.Text)))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            Guid tmpSubjectGuid = _subjectServiceService.All().Where(x => x.Name == subjName).FirstOrDefault().Id;

            Subject tempSubj = _subjectServiceService.GetByName(subjName);
            List<Guid> textsIDList = new List<Guid>();
            texts = _textService.All().Where(x => x.Subject_Id == tempSubj.Id).ToList();

            if (student.Homeworks.Where(x=> x.Text.Subject_Id == tmpSubjectGuid).Count() != 0)
            {
                TempData["HomeWorks"] = "true";

                foreach (var hw in student.Homeworks)
                {
                    if (texts.Contains(hw.Text))
                    {
                        textsIDList.Add(hw.Text.Id);
                        Tuple<string, string, Text> t = new Tuple<string, string, Text>(hw.Created_By.Name, hw.Deadline.ToString(), hw.Text);
                        textTuple.Add(t);
                    }
                }

            }
            else
            {

                TempData["HomeWorks"] = "false";

                foreach (var hw in student.Homeworks)
                {
                    Tuple<string, string, Text> t = new Tuple<string, string, Text>(hw.Created_By.Name, hw.Deadline.ToString(), hw.Text);
                    textTuple.Add(t);

                }

            }


            TextsNotificationsViewModel model = new TextsNotificationsViewModel();
            model.Texts = texts;
            model.TextsIDList = textsIDList;
            model.TextsTuple = textTuple;

            return View("Texts",model);
        }

        public ActionResult ChooseText(string textName)
        {
            ViewBag.Title = "בחר פעולה";

            Session["textName"] = textName;

           

            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks.Select(t => t.Text)))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }

            Text text = _textService.All().Where(x => x.Name == textName).FirstOrDefault();
            foreach (var hw in student.Homeworks)
            {
                if (hw.Text_Id == text.Id)
                {
                    Session["notificationFlag"] = "true";
                }
            }

          

            return View("TextMenu");
        }


        public ActionResult ChooseAction(string textName)
        {
            Session["textName"] = textName;
            string[] tmpStringArray = textName.Split(' ');
            if (tmpStringArray[0].Equals("לסיפור"))
            {
                textName = "";
                int i;
                for (i = 1; i < tmpStringArray.Count(); i++)
                {
                    if (i == tmpStringArray.Count() - 1)
                    {
                        textName = textName + tmpStringArray[i];
                    }
                    else
                    {
                        textName = textName + tmpStringArray[i] + " ";
                    }
                }
            }

            Session["title"] = textName;

           // switch (submit)
           // {
              //  case "לסיפור":
            Session["WithQuestion?"] = "Without";

            Session["TextContent"] = _fileManager.GetText(text.FilePath);
                    //init words definition
            Session["WordsDefs"] = getWordDefinitionsForText(_fileManager.GetText(text.FilePath));
            string tmpName = (string)Session["title"];
            string userid = User.Identity.GetUserId();

            foreach (var std in _studentService.All().Include(x => x.Homeworks))
            {
                if (std.ApplicationUserId.Equals(userid))
                {
                    student = std;
                    break;
                }
            }
            //todo- check if first == "לסיפור"
           
            textGuid = _textService.All().Where(t => t.Name == tmpName).FirstOrDefault().Id;

            /// .All().Include(x => x.Questions.Select(q=>q.Policy)).Where(x => x.Id == hw.Id)
            List<Homework> AllStudentHW = _homeworkService.All().Where(x => x.Text.Id == textGuid).ToList();
            /// .All().Include(x => x.Questions.Select(q=>q.Policy)).Where(x => x.Id == hw.Id)



            Homework hw = AllStudentHW.First();


            if (hw.Text_Id == textGuid)
            {
                Session["NoHomeWork"] = "1";
                return View("TextView");
            }
            
            Session["NoHomeWork"] = "0";

            return View("TextView");

        
        }


        public ActionResult GotoSmartTextBox(string questionNumber)
        {

            int tmpQuestNumber = 1;

            if(questionNumber == "בחזרה לסיפור")
            {
                return View("TextView");
            }

            Session["WithQuestion?"] = "With";
            if (questionNumber != null)
            {
                string[] tmpStringArray = questionNumber.Split(' ');
                tmpQuestNumber = Int32.Parse(tmpStringArray.Last());
        
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

            List<Homework> AllStudentHW = _homeworkService.All().Where(x=>x.Text.Id == textGuid).ToList();

            Homework hw = AllStudentHW.First();
            List<Question> tmpQuestionsList = _homeworkService.All().Include(x => x.Questions.Select(q => q.Policy)).Include(x => x.Questions.Select(q => q.Suggested_Openings)).Where(x => x.Id == hw.Id).FirstOrDefault().Questions.ToList();
            smartView.question = tmpQuestionsList.Where(x => x.Question_Number == tmpQuestNumber).FirstOrDefault();
            smartView.Questions = tmpQuestionsList;

            if (smartView.question.Suggested_Openings.Count == 0)
            {
                SuggestedOpening noSuggOpen = new SuggestedOpening("אין משפטי פתיחה לשאלה זו");
                SuggestedOpening noSuggOpen2 = new SuggestedOpening("התשובה לשאלה נמצאת בגוף השאלה");
                smartView.question.Suggested_Openings.Add(noSuggOpen);
                smartView.question.Suggested_Openings.Add(noSuggOpen2);
                
            }


            List<int> SmartViewQuestionsNumbers = new List<int>();

            List<QuestionAnswer> QuestionsAnswered = _answerService.All().Where(x => x.Homework_Id == hw.Id && x.Student_Id == student.Id).SelectMany(x => x.questionAnswers).ToList();
            if (QuestionsAnswered == null || QuestionsAnswered.Count == 0)
            {
                ///*************************************************////
                ///*************************************************////
                //here cant be empty.. take care for analyze and gotosmart text box..
                smartView.CompleteQuestions = new List<QuestionAnswer>();

                SmartViewQuestionsNumbers.Add(-1);

                smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;

                Session["percentage"] = 0;

            }
            else
            {
                smartView.CompleteQuestions = QuestionsAnswered;
                double k = 0;
                foreach (var Ans in smartView.CompleteQuestions)
                {
                    SmartViewQuestionsNumbers.Add(Ans.Of_Question.Question_Number); //----------------------COMMENT 1/5
                    k++;
                }
                smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;

                Session["percentage"] = (int)(k / smartView.Questions.Count() * 100);

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
                
                    tmpQuestNumber = Int32.Parse(tmpStringArray.Last());

                if (tmpStringArray.Length > 4)
                {
                    return RedirectToAction("FinalAnswerSubmit", new { questionNumber = tmpQuestNumber, textContent = Request.Form["TextBoxArea"]
                });
                }

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


            List<Homework> AllStudentHW = _homeworkService.All().Where(x => x.Text.Id == textGuid).ToList();

            Homework hw = AllStudentHW.First();

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

            List<int> SmartViewQuestionsNumbers = new List<int>();

            List<QuestionAnswer> QuestionsAnswered = _answerService.All().Where(x => x.Homework_Id == hw.Id && x.Student_Id == student.Id).SelectMany(x => x.questionAnswers).ToList();


            smartView.CompleteQuestions = QuestionsAnswered;

            if (QuestionsAnswered.Count() == 0)
            {
                ///*************************************************////
                ///*************************************************////
                //here cant be empty.. take care for analyze and gotosmart text box..
                smartView.CompleteQuestions = new List<QuestionAnswer>();

                SmartViewQuestionsNumbers.Add(-1);

                smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;

                Session["percentage"] = 0;


            }
            else {
                int k = 0;

                foreach (var QuestAns in QuestionsAnswered)
                {
                    SmartViewQuestionsNumbers.Add(QuestAns.Of_Question.Question_Number);
                    k++;
                }
                smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;
                Session["percentage"] = (k / smartView.Questions.Count * 100);

            }


            return View("QuestionsView", smartView);

        }

       
        public ActionResult FinalAnswerSubmit(int questionNumber, string textContent)
        {
            string input = textContent;



            int numOfWords = _smartTextBox.GetNumberOfWords(input);
            int numOfConnectors = _smartTextBox.GetNumberOfConnectors(input);
            IDictionary<string, int> repeatedWords = _smartTextBox.GetRepeatedWords(input);

         

            TempData["NumberOfWords"] = numOfWords;
            TempData["NumberOfConnectorWords"] = numOfConnectors;
            TempData["Answer"] = input;

           
            smartView.QuestionNumber = questionNumber;


            Answer ans = new Answer();

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


            List<Homework> AllStudentHW = _homeworkService.All().Where(x => x.Text.Id == textGuid).ToList();

            Homework hw = AllStudentHW.First();

            ICollection<Question> tmpQuestionsList = _homeworkService.All().Include(x => x.Questions.Select(p=>p.Policy)).Where(x => x.Id == hw.Id).FirstOrDefault().Questions;
            smartView.question = tmpQuestionsList.Where(x => x.Question_Number == questionNumber).FirstOrDefault();//include policy..
            smartView.Questions = tmpQuestionsList.ToList();

            if (_answerService.All().Where(x => x.Student_Id == student.Id && x.Homework_Id == hw.Id).FirstOrDefault() == null) {

                ans.Answer_To = _homeworkService.All().Where(x => x.Id == hw.Id).FirstOrDefault();
                ans.Date_Submitted = DateTime.Now;
                ans.Homework_Id = hw.Id;
             //   ans.Id = Guid.NewGuid();
                ans.IsDeleted = false;
                ans.TestID = 0;
                ans.Grade = 0;
                QuestionAnswer tmpQuestionAnswer = new QuestionAnswer();
                tmpQuestionAnswer.In_Answer = ans;
                tmpQuestionAnswer.Answer_Id = ans.Id;
                tmpQuestionAnswer.Of_Question = smartView.question;
                tmpQuestionAnswer.Question_Id = smartView.question.Id;
                tmpQuestionAnswer.Content = input;
                tmpQuestionAnswer.Date_Submitted = ans.Date_Submitted;
                ans.questionAnswers.Add(tmpQuestionAnswer);
                ans.Student_Id = student.Id;
                ans.Submitted_By = student;

                _answerService.Add(ans);
            }

            else
            {
                ICollection<QuestionAnswer> QuestionsAnsweredOnHomeWork = _answerService.All().Where(x => x.Homework_Id == hw.Id && x.Student_Id == student.Id).SelectMany(x => x.questionAnswers).ToList();
                ans = _answerService.All().Where(x => x.Student_Id == student.Id && x.Homework_Id == hw.Id).FirstOrDefault();

                if (QuestionsAnsweredOnHomeWork.Where(x => x.Of_Question.Question_Number == questionNumber).Count() == 0)

                {
                    QuestionAnswer tmpQuestionAnswer = new QuestionAnswer();
                 //   tmpQuestionAnswer.Id = Guid.NewGuid();
                    tmpQuestionAnswer.Answer_Id = ans.Id;
                    tmpQuestionAnswer.Content = input;
                    tmpQuestionAnswer.Date_Submitted = DateTime.Now;
                    tmpQuestionAnswer.In_Answer = ans;
                    tmpQuestionAnswer.Of_Question = smartView.question;
                    tmpQuestionAnswer.Question_Id = smartView.question.Id;
                    QuestionsAnsweredOnHomeWork.Add(tmpQuestionAnswer);
                    ans.questionAnswers = QuestionsAnsweredOnHomeWork;
                    _answerService.Update(ans);

                }
                else
                {
                    QuestionAnswer tmpQuestionAnswer = QuestionsAnsweredOnHomeWork.Where(x => x.Of_Question.Question_Number == questionNumber).FirstOrDefault();
                    ans.questionAnswers.Remove(tmpQuestionAnswer);
                    tmpQuestionAnswer.Content = input;
                    tmpQuestionAnswer.Date_Submitted = DateTime.Now;
                    ans.questionAnswers.Add(tmpQuestionAnswer);
                    _answerService.Update(ans);
                }
            }


            if (smartView.question.Suggested_Openings.Count == 0)
            {
                SuggestedOpening noSuggOpen = new SuggestedOpening("אין משפטי פתיחה לשאלה זו");
                SuggestedOpening noSuggOpen2 = new SuggestedOpening("התשובה לשאלה נמצאת בגוף השאלה");
                smartView.question.Suggested_Openings.Add(noSuggOpen);
                smartView.question.Suggested_Openings.Add(noSuggOpen2);

            }

            List<int> SmartViewQuestionsNumbers = new List<int>();

            Answer QuestionsAnswered = _answerService.All().Where(x => x.Homework_Id == hw.Id && x.Student_Id == student.Id).FirstOrDefault();
          
            
            smartView.CompleteQuestions = QuestionsAnswered.questionAnswers.ToList();
            int k = 0;

            foreach (var QuestAns in smartView.CompleteQuestions)
            {
                SmartViewQuestionsNumbers.Add(QuestAns.Of_Question.Question_Number);
                k++;
            }
                smartView.CompleteQuestionsNumbers = SmartViewQuestionsNumbers;

            if ((int)(k / smartView.Questions.Count() * 100) > 99)
            {
                student.Homeworks.Remove(hw);
                _studentService.Update(student);
            }
            

                    

            TempData["toManyConnectors"] = "התשובה הוגשה בהצלחה למערכת. תוכל לשנות אותה עד לתאריך ההגשה האחרון";

            string tmpstring = "שאלה מספר " + questionNumber;
            return RedirectToAction("GotoSmartTextBox", new { questionNumber = tmpstring });
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
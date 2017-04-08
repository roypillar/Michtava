using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;
using FileHandler;
using SmartTextBox;
using Frontend.Models;

namespace Frontend.Controllers
{
    public class StudentsController : Controller
    {
        private IFileManager _fileManager = new FileManager();
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy = new Policy();
        private SmartTextViewModel smartView = new SmartTextViewModel();
        private Text text = new Text();

        // GET: Students
        public ActionResult Index()
        {
            ViewBag.Title = "בחר נושא";

            return View("Subjects");
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

        public ActionResult ChooseAction()
        {
            ViewBag.Title = "טקסט";

            //we want to pass the real text model, not only the string..
            //we get all the real data when start get it from the dal..
        //    text.Id = 10;
          //  text.Name = "המסמר";
            
/*
        public DateTime UploadTime { get; set; }

        public FileFormats Format { get; set; }

        public string FilePath { get; set; }
            */
            //should get the path from the text..
            TempData["TextContent"] = _fileManager.GetText(@"C:\Users\mweiss\Desktop\Test.txt");

            return View("TextView");
        }

        public ActionResult GotoSmartTextBox()
        {
            

            ViewBag.Title = "שאלות לתיבת טקסט חכמה";
            TempData["TextContent"] = _fileManager.GetText(@"C:\Users\mweiss\Desktop\Test.txt");

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
                TempData["toManyWords"] = "";
            }

            InitializeSmartView();

            
            
            return View("QuestionsView", smartView);
        }

       

        


        public ActionResult AnalyzeAnswer()
        {
            ViewBag.Title = "שאלות לתיבת טקסט חכמה";
            TempData["TextContent"] = _fileManager.GetText(@"C:\Users\mweiss\Desktop\Test.txt");

            // here we have to call the SmartTextBox in server side

            string input = Request.Form["TextBoxArea"];
            int numOfWords = _smartTextBox.GetNumberOfWords(input);
            int numOfConnectors = _smartTextBox.GetNumberOfConnectors(input);
            TempData["NumberOfWords"] = numOfWords;
            TempData["NumberOfConnectorWords"] = numOfConnectors;
            TempData["Answer"] = input;

            //כשנוסיף את הפוליסי שתרוץ לא תהיה כנראה את הבעיה.. בינתיים
            InitializePolicy();

            if (numOfWords > _policy.MaxWords)
            {
                TempData["toManyWords"] = "הכנסת " + numOfWords + " מילים, אבל מותר לכל היותר " + _policy.MaxWords + " מילים.";
            }
            if (numOfConnectors > _policy.MaxConnectors)
            {
                TempData["toManyConnectors"] = "הכנסת " + numOfConnectors + " מילות קישור, אבל מותר לכל היותר " + _policy.MaxConnectors + " מילות קישור.";
            }
            InitializeSmartView();
            return View("QuestionsView", smartView);
        }

        private void InitializePolicy()
        {
            _policy = new Policy();
            HashSet<string> _keySentencesList = new HashSet<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא");
            _policy = new Policy() { Id = "1", MinWords = 20, MaxWords = 30, MinConnectors = 3, MaxConnectors = 8, KeySentences = _keySentencesList };
        }

        private void InitializeSmartView()
        {
            InitializePolicy();
            smartView.policy = _policy;
            Question q = new Question();
            q.Id = 11;
            q.Content = "שאלה לדוגמא שנשלוף מהבסיס נתונים";
            q.Policy = _policy;
            List<Question> qList = new List<Question>();
            qList.Add(q);
            smartView.questions = qList;
            
        }
    }
}
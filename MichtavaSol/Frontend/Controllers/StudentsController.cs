using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;
using FileHandler;
using SmartTextBox;

namespace Frontend.Controllers
{
    public class StudentsController : Controller
    {
        private IFileManager _fileManager = new FileManager();
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy = new Policy();

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

            TempData["TextContent"] = _fileManager.GetText(@"C:\Users\mweiss\Desktop\Test.txt");

            return View("TextView");
        }

        public ActionResult GotoSmartTextBox()
        {
            ViewBag.Title = "תיבת טקסט חכמה";

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
                TempData["toManyWords"] = "";
            }

            InitializePolicy();

            return View("SmartTextBox", _policy);
        }

        public ActionResult AnalyzeAnswer()
        {

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

            return View("SmartTextBox", _policy);
        }

        private void InitializePolicy()
        {
            _policy = new Policy();
            HashSet<string> _keySentencesList = new HashSet<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא");
            _policy = new Policy() { Id = "1", MinWords = 20, MaxWords = 30, MinConnectors = 3, MaxConnectors = 8, KeySentences = _keySentencesList };
        }
    }
}
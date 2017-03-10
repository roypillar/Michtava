using SmartTextBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;
using Frontend.Models;

namespace Frontend.Controllers
{
    public class SmartTextBoxController : Controller
    {
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy;

        public ActionResult SmartTextBox()
        {
            ViewBag.Title = "תיבת טקסט חכמה";

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
                TempData["toManyWords"] = "";
            }


            // hard coded policy - need to be removed ------------------

            Policy _policy = new Policy();
            List<string> _keySentencesList = new List<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא");
            _policy = new Policy() { _id = "1", _minWords = 20, _maxWords = 30, _minConnectors = 3, _maxConnectors = 8, _keySentences = _keySentencesList };

            //----------------------------------------------------------

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
            Policy _policy = new Policy();
            List<string> _keySentencesList = new List<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא");
            _policy = new Policy() { _id = "1", _minWords = 20, _maxWords = 30, _minConnectors = 3, _maxConnectors = 8, _keySentences = _keySentencesList };

            if(numOfWords>_policy._maxWords)
            {
                TempData["toManyWords"] = "הכנסת " + numOfWords + " מילים, אבל מותר לכל היותר " + _policy._maxWords + " מילים.";
            }
            if (numOfConnectors > _policy._maxConnectors)
            {
                TempData["toManyConnectors"] = "הכנסת " + numOfConnectors + " מילות קישור, אבל מותר לכל היותר " + _policy._maxConnectors + " מילות קישור.";
            }
            
            
            return RedirectToAction("SmartTextBox", _policy);
        }
    }
}

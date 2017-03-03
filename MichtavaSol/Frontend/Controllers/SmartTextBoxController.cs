using SmartTextBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;

namespace Frontend.Controllers
{
    public class SmartTextBoxController : Controller
    {
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();

        public ActionResult SmartTextBox()
        {
            ViewBag.Title = "תיבת טקסט חכמה";

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
            }

            Policy _policy = new Policy();
            List<string> _keySentencesList = new List<string>();
            _keySentencesList.Add("התשובה לשאלה שנשאלה היא");
            _policy = new Policy() { _id = "1", _minWords = 20, _maxWords = 30, _minConnectors = 3, _maxConnectors = 8, _keySentences = _keySentencesList };

            return View("SmartTextBox", _policy);
        }

        public ActionResult AnalyzeAnswer()
        {

            // here we have to call the SmartTextBox in server side
           

            string input = Request.Form["TextBoxArea"];

            TempData["NumberOfWords"] = _smartTextBox.GetNumberOfWords(input);
            TempData["NumberOfConnectorWords"] = _smartTextBox.GetNumberOfConnectors(input);
            TempData["Answer"] = input;   
            
            return RedirectToAction("SmartTextBox");
        }
    }
}

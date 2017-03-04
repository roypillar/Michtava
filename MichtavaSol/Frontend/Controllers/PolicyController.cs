using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal;
using Entities.Models;
using SmartTextBox;

namespace Frontend.Controllers
{
    public class PolicyController : Controller
    {
        private ISmartTextBox _smartTextBox = new SmartTextBoxImpl();
        private Policy _policy;

        public ActionResult Policy()
        {
            ViewBag.Title = "הוספת שאלה";

            return View("Policy");
        }

        public ActionResult SubmitPolicy()
        {
            InitializePolicy();

            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
            }
 
            return View("../SmartTextBox/SmartTextBox",  _policy);
        }

        private void InitializePolicy()
        {
            _policy = new Policy();

            _policy._minWords = int.Parse(Request.Form["_minWords"]);
            _policy._maxWords = int.Parse(Request.Form["_maxWords"]);
            _policy._minConnectors = int.Parse(Request.Form["_minConnectors"]);
            _policy._maxConnectors = int.Parse(Request.Form["_maxConnectors"]);
        }
    }
}

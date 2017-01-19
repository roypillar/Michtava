using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class SmartTextBoxController : Controller
    {
        public ActionResult SmartTextBox()
        {
            ViewBag.Title = "תיבת טקסט חכמה";

            return View();
        }

        public ActionResult AnalyzeAnswer()
        {
            // here we have to call the SmartTextBox in server side

            TempData["Answer"] = Request.Form["myTextBox2"];

            return RedirectToAction("SmartTextBox");
        }
    }
}

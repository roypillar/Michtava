using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Models;

namespace Frontend.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Teachers
        public ActionResult Index()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("TeachersMenu");
        }

        public ActionResult NavigateToTextAdding()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("TextAdding");
        }

        public ActionResult NavigateToPolicy()
        {
            ViewBag.Title = "בחר אפשרות";

            return View("Policy");
        }

        public ActionResult SubmitPolicy()
        {
            if (TempData["NumberOfWords"] == null && TempData["NumberOfConnectorWords"] == null)
            {
                TempData["NumberOfWords"] = "0";
                TempData["NumberOfConnectorWords"] = "0";
            }

            return View("Policy");
        }
    }
}
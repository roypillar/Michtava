using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "מכתבה - בית";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "מכתבה - אודות";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "מכתבה - צור קשר";

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
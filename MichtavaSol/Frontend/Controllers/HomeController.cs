using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileHandler;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private IFileManager _fileManager = new FileManager();

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

        public ActionResult TextAdding()
        {
            ViewBag.Title = "בחר טקסט";

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
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

            TempData["TextContent"] = _fileManager.GetText(@"C:\Users\mweiss\Desktop\NewTextDocument.txt");

            return View("TextView");
        }
    }
}
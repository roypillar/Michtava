using System;
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

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            ViewBag.Title = "בחר נושא";

            if (userName != null && userName.Equals("Student", StringComparison.InvariantCultureIgnoreCase))
                return RedirectToAction("Subjects", "Students");

            else if (userName != null && userName.Equals("Teacher", StringComparison.InvariantCultureIgnoreCase))
                return RedirectToAction("Index", "Teachers");

            return View("Index");
        }
    }
}
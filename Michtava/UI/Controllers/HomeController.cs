using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {

            TempData["currentTime"] = DateTime.Now.ToString();
           
                Session["currentTime2"] = DateTime.Now.ToString();
            return RedirectToAction("showHomePage", "Home"); 
            //return View();
        }

        public ActionResult showHomePage()
        {
           // ViewData["currentTime"] = DateTime.Now.ToString();
            //ViewBag.currentTime1 = DateTime.Now.ToString();

            return View("MyHome");
        }
	}
}
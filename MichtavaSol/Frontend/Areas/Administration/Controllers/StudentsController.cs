using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Areas.Administration.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Administration/Students
        public ActionResult Index()
        {
            return View();
        }
    }
}
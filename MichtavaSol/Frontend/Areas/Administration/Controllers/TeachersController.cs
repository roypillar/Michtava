using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Areas.Administration.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Administration/Teachers
        public ActionResult Index()
        {
            return View();
        }
    }
}
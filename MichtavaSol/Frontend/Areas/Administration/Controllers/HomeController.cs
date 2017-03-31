namespace Frontend.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Common;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class HomeController : Controller
    {
        // GET: Administration/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
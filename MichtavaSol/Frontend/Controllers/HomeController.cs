using Common;
using Frontend.App_Start.Identity;
using Frontend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggingUser = await UserManager.FindByNameAsync(model.UserName);


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            /*---------------------------------------------------------------------------------
             * -
             * ---------------------------------------------------------------------------------
             * 
             * --------------------------------------------------------------------------------

            /*  מגלוס נראה לי שבגלל שלמורים ולתלמידים ולאדמינים יש מסכים ולוגיקה שונים לגמרי לדעתי כדאי להפריד ל
            areas
             כמו שעשיתי באדמיניסטרציה כי מקבלים המון דברים חינם אם בוחרים לחלק ככה אבל אם לא אז אין לי בעיה שתעשה בדרך שהתחלת
             

            _---------------------------------------------------------------------------------------
            -

            -------------------------------------------------------------------------------------

            --------------------------------------------------------------------------------------
             */


            switch (result)
            {
                case SignInStatus.Success:
                    if (string.IsNullOrEmpty(returnUrl))//there is nowhere to return to, just go to the home of the role.
                    {
                        if (this.UserManager.IsInRole(loggingUser.Id, GlobalConstants.AdministratorRoleName))//if the user is an admin
                        {
                            return RedirectToAction("Index", "Home", new { area = "Administration" });
                        }
                        else if (this.UserManager.IsInRole(loggingUser.Id, GlobalConstants.TeacherRoleName))//if the user is a teacher
                        {
                            return RedirectToAction("Index", "Teachers");
                        }
                        else if (this.UserManager.IsInRole(loggingUser.Id, GlobalConstants.StudentRoleName))//if the user is a student
                        {
                            return RedirectToAction("Index", "Students");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { area = string.Empty });//otherwise, homepage
                        }
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }





                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
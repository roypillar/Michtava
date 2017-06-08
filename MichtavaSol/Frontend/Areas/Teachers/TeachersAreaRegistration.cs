using System.Web.Mvc;

namespace Frontend.Areas.Teachers
{
    public class TeachersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Teachers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Teachers_default",
                "Teachers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            //context.MapRoute(
            //  name: "ghdfzshdrfashd",
            //  url: "Teachers/Account/Register",
            //  defaults: new { controller = "Account", action = "Register" },
            //  namespaces: new string[] { "Frontend.Areas.Teachers.Controllers" });

        }
    }
}
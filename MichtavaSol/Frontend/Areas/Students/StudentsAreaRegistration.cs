using System.Web.Mvc;

namespace Frontend.Areas.Students
{
    public class StudentsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Students";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Students_default",
            //    "Students/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            context.MapRoute(
          name: "Students_default",
          url: "Students/{controller}/{action}/{id}",
          defaults: new { action = "Index", id = UrlParameter.Optional },
          namespaces: new string[] { "Frontend.Areas.Students.Controllers" });
        }
    }
}
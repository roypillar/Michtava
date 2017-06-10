using System.Web.Mvc;

namespace Frontend.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {


            context.MapRoute(
              name: "Administration_administrator_details",
              url: "Administration/Administrators/Details/{username}",
              defaults: new { controller = "Administrators", action = "Details" },
              namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
              name: "Administration_administrator_edit",
              url: "Administration/Administrators/Edit/{username}",
              defaults: new { controller = "Administrators", action = "Edit" },
              namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
              name: "Administration_administrator_delete",
              url: "Administration/Administrators/Delete/{username}",
              defaults: new { controller = "Administrators", action = "Delete" },
              namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
               name: "Administration_default",
               url: "Administration/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
               name: "Administration_teacher_create",
               url: "Administration/Teachers/Create",
               defaults: new { controller = "Account", action = "Register" },
               namespaces: new string[] { "Frontend.Areas.Teachers.Controllers" });

            context.MapRoute(
               name: "Administration_student_create",
               url: "Administration/Students/Create",
               defaults: new { controller = "Account", action = "Register" },
               namespaces: new string[] { "Frontend.Areas.Students.Controllers" });

     
        }
    }
}
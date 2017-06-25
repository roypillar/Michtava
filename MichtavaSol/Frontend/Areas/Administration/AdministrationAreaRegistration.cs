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
               name: "Administration_teacher_create",
               url: "Administration/Teachers/Create",
               defaults: new { controller = "Account", action = "Register" },
               namespaces: new string[] { "Frontend.Areas.Teachers.Controllers" });

            context.MapRoute(
               name: "Administration_student_create",
               url: "Administration/Students/Create",
               defaults: new { controller = "Account", action = "Register" },
               namespaces: new string[] { "Frontend.Areas.Students.Controllers" });







            


            context.MapRoute(
            name: "Administration_classes_students",
            url: "Administration/SchoolClasses/{id}/Students",
            defaults: new { controller = "SchoolClasses", action = "Students" },
            namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
           name: "Administration_remove_student_from_class",
           url: "Administration/SchoolClasses/{id}/Students/Remove/{username}",
           defaults: new { controller = "SchoolClasses", action = "Students" },
           namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
           name: "Administration_add_student_to_class",
           url: "Administration/SchoolClasses/{id}/Students/Add/{username}",
           defaults: new { controller = "SchoolClasses", action = "Students" },
           namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });




            context.MapRoute(
           name: "Administration_classes_teachers",
           url: "Administration/SchoolClasses/{id}/Teachers",
           defaults: new { controller = "SchoolClasses", action = "Teachers" },
           namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
           name: "Administration_remove_teacher_from_class",
           url: "Administration/SchoolClasses/{id}/Teachers/Remove/{username}",
           defaults: new { controller = "SchoolClasses", action = "Teachers" },
           namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            context.MapRoute(
           name: "Administration_add_teacher_to_class",
           url: "Administration/SchoolClasses/{id}/Teachers/Add/{username}",
           defaults: new { controller = "SchoolClasses", action = "Teachers" },
           namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });

            //context.MapRoute(
            //name: "Administration_add_student_to_class",
            //url: "Administration/SchoolClasses/{id}/Students/Add/{username}",
            //defaults: new { controller = "SchoolClasses", action = "AddStudentToClass" },
            //namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });


            context.MapRoute(
              name: "Administration_default",
              url: "Administration/{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });
        }
    }
}
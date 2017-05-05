using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Frontend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Frontend.Controllers" });


            // ----------------------------------------------- Students -----------------------------------------------

            routes.MapRoute(
                name: "Students",
                url: "Students/{action}/{username}",
                defaults: new { controller = "Students", action = "Subjects", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "SubSubjects",
                url: "ChooseSubject/{action}/{username}",
                defaults: new { controller = "Students", action = "ChooseSubject", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "Texts",
                url: "ChooseSubSubject/{action}/{username}",
                defaults: new { controller = "Students", action = "ChooseSubSubject", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "TextMenu",
                url: "ChooseText/{action}/{username}",
                defaults: new { controller = "Students", action = "ChooseText", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "TextView",
                url: "ChooseAction/{action}/{username}",
                defaults: new { controller = "Students", action = "ChooseAction", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "SmartTextBox",
                url: "GotoSmartTextBox/{action}/{username}",
                defaults: new { controller = "Students", action = "GotoSmartTextBox", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "AnalyzeAnswer",
                url: "AnalyzeAnswer/{action}/{username}",
                defaults: new { controller = "Students", action = "AnalyzeAnswer", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "AnalyzeAnswer2",
                url: "Students/GotoSmartTextBox/{username}",
                defaults: new { controller = "Students", action = "GotoSmartTextBox", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            // --------------------------------------------------------------------------------------------------------------



            // ------------------------------------------------- Teachers -------------------------------------------------

            routes.MapRoute(
                name: "TextsViewAfterAddingText",
                url: "NavigateToClassView/NavigateToTextsView/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToTextsView", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "ClassView",
                url: "NavigateToClassView/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToClassView", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "TeacherHome",
                url: "Home/{action}/{username}",
                defaults: new { controller = "Teachers", action = "Index", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "AddSubject",
                url: "NavigateToAddSubject/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToAddSubject", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "TextsView",
                url: "NavigateToTextsView/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToTextsView", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "TextAdding",
                url: "NavigateToTextAdding/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToTextAdding", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "Policy",
                url: "NavigateToPolicy/{action}/{username}",
                defaults: new { controller = "Teachers", action = "NavigateToPolicy", username = string.Empty },
                namespaces: new string[] { "Frontend.Controllers" });

            // --------------------------------------------------------------------------------------------------------------



            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "Frontend.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Frontend.Controllers" });
        }
    }
}

﻿using System.Web.Mvc;

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
               name: "Administration_default",
               url: "Administration/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "Frontend.Areas.Administration.Controllers" });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Models;

namespace UI.ModelBinders
{
    public class UserBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string uFirstName = objContext.Request.Form["user.FirstName"];
            string uLastName = objContext.Request.Form["user.LastName"];
            string uNumber = objContext.Request.Form["user.UserNumber"];

            User obj = new User 
                                        {FirstName = uFirstName,
                                         LastName = uLastName,
                                         UserNumber = uNumber
                                        };

            return obj;
        }
    }
}
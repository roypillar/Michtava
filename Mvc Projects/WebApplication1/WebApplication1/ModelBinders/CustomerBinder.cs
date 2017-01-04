using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ModelBinders
{
    public class CustomerBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string customerFirstName = objContext.Request.Form["customer.FirstName"];
            string customerLastName = objContext.Request.Form["customer.LastName"];
            string customerCustomerNumber = objContext.Request.Form["customer.CustomerNumber"];

            Customer obj = new Customer 
                                        {FirstName = customerFirstName,
                                         LastName = customerLastName,
                                         CustomerNumber = customerCustomerNumber
                                        };

            return obj;
        }
    }
}
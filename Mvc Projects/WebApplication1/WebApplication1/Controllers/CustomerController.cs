using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ModelBinders;
using WebApplication1.Dal;
using WebApplication1.ModelView;
using System.Threading;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        public ActionResult Load()
        {
            // Customer myCustomer = new Customer("first","last","1");  //with constructor
            Customer myCustomer2 = new Customer { FirstName = "Yarden", LastName = "Menashe", CustomerNumber = "123f" }; //without constructor

            return View("Customer", myCustomer2);
        }



        public ActionResult ShowSearch()
        {
            CustomerViewModel cvm = new CustomerViewModel();
            cvm.customers = new List<Customer>();
            return View("SearchCustomers", cvm);
        }

        public ActionResult SearchCustomers()
        {
            CustomerDal dal = new CustomerDal();
            string searchValue = Request.Form["txtFirstName"].ToString();
            List<Customer> customersList =
                (
                   from x in dal.Customers
                   where x.FirstName.Contains(searchValue)
                   select x
                ).ToList<Customer>();
            CustomerViewModel cvm = new CustomerViewModel();
            cvm.customers = customersList;
            return View("SearchCustomers", cvm);
        }

        public ActionResult GetCustomersByJson()
        {
            CustomerDal dal = new CustomerDal();
          //  Thread.Sleep(4000);
            List<Customer> customersList = dal.Customers.ToList<Customer>();
            return Json(customersList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Enter()
        {
            CustomerDal dal = new CustomerDal();
            List<Customer> customersList = dal.Customers.ToList<Customer>();        //gets the customers list from the dal

            CustomerViewModel cvm = new CustomerViewModel();
            cvm.customer = new Customer();
            cvm.customers = customersList;
            return View(cvm);        //we pass customer view model object to submit because submit need some obj other wise he cant get cust..
        }                               //here we didnt pass the view name because we have a match for Enter procedure and view. they bond!


        //this is the direct approch, allowed because the id names in the enter view matches the fields name of customer model!
        /* // other option is to create model binder to the data like so
         * //        public ActionResult Submit([ModelBinder(typeof(CustomerBinder))] Customer cust)
         * //           we can use it if one works on the views and name the id fields in one name and other on the models with different names..
         * //           but for some reason modelstate.isvalid doesnt work when model binder used.. check on it if its becomes a problem..

         * //here we get to each field by his name like this.. 
        Customer cust = new Customer();
        cust.FirstName = Request.Form["FirstName"];
        cust.LastName = Request.Form["LastName"];
        cust.CustomerNumber = Request.Form["CustomerNumber"];
         * */

        //any way--> this is server side validation.. if we want in client side its possible but more complicated--> using jquery

        public ActionResult Submit()
        {
            Customer objCust = new Customer();
            CustomerDal dal = new CustomerDal();
            objCust.FirstName = Request.Form["customer.FirstName"].ToString();
            objCust.LastName = Request.Form["customer.LastName"].ToString();
            objCust.CustomerNumber = Request.Form["customer.CustomerNumber"].ToString();


            if (ModelState.IsValid)
            {
                //here we adding to the dal layer new customer
                dal.Customers.Add(objCust);                //by adding to the customerDal new customer
                dal.SaveChanges();                      //and save to the db ..x    

            }
           // Thread.Sleep(4000);
            List<Customer> customersList = dal.Customers.ToList<Customer>();
            return Json(customersList, JsonRequestBehavior.AllowGet);



        }

    }
}
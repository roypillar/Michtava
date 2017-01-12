using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Models;
using UI.ModelBinders;
using DAL;
using Domain.ModelView;
using System.Threading;

namespace UI.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Load()
        {
            // User myUser = new User("first","last","1");  //with constructor
            User myUser2 = new User { FirstName = "Yarden", LastName = "Menashe", UserNumber = "123f" }; //without constructor

            return View("User", myUser2);
        }



        public ActionResult ShowSearch()
        {
            UserViewModel uvm = new UserViewModel();
            uvm.users = new List<User>();
            return View("SearchUsers", uvm);
        }

        public ActionResult SearchUsers()
        {
            UserDal dal = new UserDal();
            string searchValue = Request.Form["txtFirstName"].ToString();
            List<User> usersList = dal.doSomething(searchValue);
                
            UserViewModel uvm = new UserViewModel();
            uvm.users = usersList;
            return View("SearchUsers", uvm);
        }

        public ActionResult GetUsersByJson()
        {
            UserDal dal = new UserDal();
           // Thread.Sleep(4000);
            List<User> usersList = dal.Users.ToList<User>();
            return Json(usersList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Enter()
        {
            UserDal dal = new UserDal();
            List<User> usersList = dal.Users.ToList<User>();        //gets the users list from the dal

            UserViewModel uvm = new UserViewModel();
            uvm.user = new User();
            uvm.users = usersList;
            return View(uvm);        //we pass user view model object to submit because submit need some obj other wise he cant get user..
        }                               //here we didnt pass the view name because we have a match for Enter procedure and view. they bond!


        //this is the direct approch, allowed because the id names in the enter view matches the fields name of user model!
        /* // other option is to create model binder to the data like so
         * //        public ActionResult Submit([ModelBinder(typeof(UserBinder))] User user)
         * //           we can use it if one works on the views and name the id fields in one name and other on the models with different names..
         * //           but for some reason modelstate.isvalid doesnt work when model binder used.. check on it if its becomes a problem..

         * //here we get to each field by his name like this.. 
        User user = new User();
        user.FirstName = Request.Form["FirstName"];
        user.LastName = Request.Form["LastName"];
        user.UserNumber = Request.Form["UserNumber"];
         * */

        //any way--> this is server side validation.. if we want in client side its possible but more complicated--> using jquery

        public ActionResult Submit()
        {
            User user = new User();
            UserDal dal = new UserDal();
            user.FirstName = Request.Form["user.FirstName"].ToString();
            user.LastName = Request.Form["user.LastName"].ToString();
            user.UserNumber = Request.Form["user.UserNumber"].ToString();


            if (ModelState.IsValid)
            {
                //here we adding to the dal layer new user
                dal.Users.Add(user);                //by adding to the userDal new user
                dal.SaveChanges();                      //and save to the db ..x    

            }
           // Thread.Sleep(4000);
            List<User> usersList = dal.Users.ToList<User>();
            UserViewModel uvm = new UserViewModel();
            uvm.user = new User();
            uvm.users = usersList;

            return View("Enter",uvm);
         //   return Json(usersList, JsonRequestBehavior.AllowGet);



        }

    }
}
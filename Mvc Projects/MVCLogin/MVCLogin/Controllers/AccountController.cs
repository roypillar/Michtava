﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLogin.Models;

namespace MVCLogin.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                return View(db.userAccount.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount usr)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    db.userAccount.Add(usr);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Messege = usr.FirstName + " " + usr.LastName + " successfully registared.";
            }
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using(OurDBContext db = new OurDBContext())
            {
                var usr = db.userAccount.Single(u => u.FirstName == user.FirstName && u.Password == user.Password);
                if (usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserID"] !=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
           
	}
}
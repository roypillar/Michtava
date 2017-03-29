﻿using System;
using System.IO;
using Frontend.Areas.Students.Models.AccountViewModels;

namespace Frontend.Areas.Students.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Common;
    using Entities.Models;
    using Services.Interfaces;
    using Frontend.App_Start.Identity;
    using Frontend.Areas.Students.Models.AccountViewModels;



   // [Authorize(Roles = GlobalConstants.StudentRoleName)]//only students allowed here
    public class AccountController : Controller
    {
        private readonly IStudentService studentService;//the service, incl. create/read/update/delete functions

        private ApplicationSignInManager signInManager;//this and the userManager only exist in account controllers, everywhere else has only services.

        private ApplicationUserManager userManager;

        public AccountController(IStudentService studentService)//bind with Ninject (look at Frontend/Infra/NinjectDependencyResolver.cs)
        {
            this.studentService = studentService;//checked, ninject binding works!!
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //if (!User.IsInRole(GlobalConstants.AdministratorRoleName))
            //{
            //    return RedirectToAction("Index", "Home", new { area = string.Empty });
            //}


            RegisterViewModel model = new RegisterViewModel();


            return View(model);
        }


        // POST: /Account/Register
        [HttpPost]
      //  [OverrideAuthorization]
       // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
       

            if (ModelState.IsValid)
            {
           
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                };

                IdentityResult result = await this.UserManager.CreateAsync(user, model.Password);//create the applicationUser through userManager

                if (result.Succeeded)
                {
                    this.UserManager.AddToRole(user.Id, GlobalConstants.StudentRoleName);//assign role of student to newly registered user

                    Student student = new Student();
                    student.ApplicationUserId = user.Id;


                    Mapper.Map<RegisterViewModel, Student>(model, student);//dump the model into the student (in this case, all it does is student.Name = model.Name but in other cases it will apare us a lot of lines of code)
                    //the map is created in frontend/automapperconfig/organizationprofile.cs, here it is just applied.

                    this.studentService.Add(student);//add student to DB through service



                    //return RedirectToAction("Index", "Students", new { area = "Administration" });/TODO make this come back

                    return RedirectToAction("Index", "Home", new { area = string.Empty });//do we need any routing values for this?
                }
                else
                {
                    this.AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userManager != null)
                {
                    this.userManager.Dispose();
                    this.userManager = null;
                }

                if (this.signInManager != null)
                {
                    this.signInManager.Dispose();
                    this.signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        // Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}
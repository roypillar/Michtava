using System;
using System.IO;
using Frontend.Areas.Teachers.Models.AccountViewModels;

namespace Frontend.Areas.Teachers.Controllers
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
    using Frontend.Areas.Teachers.Models.AccountViewModels;



    public class AccountController : Controller
    {
        private readonly ITeacherService teacherService;//the service, incl. create/read/update/delete functions

        private ApplicationSignInManager signInManager;//this and the userManager only exist in account controllers, everywhere else has only services.

        private ApplicationUserManager userManager;

        public AccountController(ITeacherService teacherService)//bind with Ninject (look at Frontend/Infra/NinjectDependencyResolver.cs)
        {
            this.teacherService = teacherService;//checked, ninject binding works!!
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]//only admins can add users
        public ActionResult Register()
        {
            if (!User.IsInRole(GlobalConstants.AdministratorRoleName))//only admins can see the registration screen
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }


            TeacherRegisterViewModel model = new TeacherRegisterViewModel();


            return View(model);
        }


        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]//only admins can add users
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(TeacherRegisterViewModel model)
        {


            if (ModelState.IsValid)
            {

                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,

                    PhoneNumber = model.PhoneNumber,
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);//create the applicationUser through userManager

                if (result.Succeeded)
                {
                    this.UserManager.AddToRole(user.Id, GlobalConstants.TeacherRoleName);//assign role of Teacher to newly registered user

                    Teacher Teacher = new Teacher();
                    Teacher.ApplicationUserId = user.Id;
                    Teacher.Name = model.Name;
                    //Teacher.ApplicationUser = user;

                    //Mapper.Map<RegisterViewModel, Teacher>(model, Teacher);//dump the model into the Teacher (in this case, all it does is Teacher.Name = model.Name but in other cases it will apare us a lot of lines of code)
                    //the map is created in frontend/automapperconfig/organizationprofile.cs, here it is just applied.

                    MichtavaResult res = this.teacherService.Add(Teacher);//add Teacher to DB through service


                    if (res is MichtavaSuccess)
                    {
                        return RedirectToAction("Index", "Teachers", new { area = "Administration" });//TODO make this work
                    }
                    // return RedirectToAction("Index", "Home", new { area = string.Empty });//getting 404 here
                    else
                        ModelState.AddModelError(string.Empty, res.Message);
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
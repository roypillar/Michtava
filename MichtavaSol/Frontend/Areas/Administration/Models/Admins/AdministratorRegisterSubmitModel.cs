namespace Frontend.Areas.Administration.Models.Admins
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Frontend.Areas.Administration.Models.Account;

    public class AdministratorRegisterSubmitModel
    {
        public Guid Id { get; set; }

        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; }
    }
}
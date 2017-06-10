namespace Frontend.Areas.Administration.Models.Admins
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Frontend.Areas.Administration.Models.Account;

    public class AdministratorRegisterSubmitModel
    {
        public Guid Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; }
    }
}
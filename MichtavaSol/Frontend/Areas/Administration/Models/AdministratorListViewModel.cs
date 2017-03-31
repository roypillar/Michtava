﻿namespace Frontend.Areas.Administration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AdministratorListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "שם מתשמש")]
        public string UserName { get; set; }

        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם משפחה")]
        public string LastName { get; set; }

        [Display(Name = "אימייל")]
        public string Email { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
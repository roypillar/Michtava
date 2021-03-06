﻿namespace Frontend.Areas.Administration.Models.Students
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Web;
    using Frontend.Areas.Administration.Models.Account;
        
    public class StudentDetailsEditModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name must not be more than 20 characters.")]
        public string Name { get; set; }

        [Display(Name = "Class")]
        public string SchoolClass { get; set; }


        public AccountDetailsEditModel AccountDetailsEditModel { get; set; }

      
    }
}
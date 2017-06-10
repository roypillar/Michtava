namespace Frontend.Areas.Administration.Models.Students
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        
        public string Name { get; set; }

        [Display(Name = "Class")]
        public string SchoolClass { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string Phone { get; set; }
    }
}
namespace Frontend.Areas.Administration.Models.Teachers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeacherListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Name { get; set; }

        public int ClassesNumber { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
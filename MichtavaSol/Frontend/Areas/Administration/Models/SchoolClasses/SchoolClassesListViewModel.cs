namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SchoolClassesListViewModel
    {
        public Guid Id { get; set; }

        public string ClassLetter { get; set; }

        public int ClassNumber { get; set; }

        [Display(Name = "Students")]
        public int StudentsNumber { get; set; }

        [Display(Name = "Teachers")]
        public int TeachersNumber { get; set; }

    }
}
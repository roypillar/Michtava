using Frontend.Areas.Administration.Models.Students;
using Frontend.Areas.Administration.Models.Teachers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    public class SchoolClassDetailsEditModel
    {
        private List<StudentListViewModel> students;
        private List<TeacherListViewModel> teachers;

        public SchoolClassDetailsEditModel()
        {
            this.students = new List<StudentListViewModel>();
            this.teachers = new List<TeacherListViewModel>();

        }

        public Guid Id { get; set; }

        public string ClassLetter { get; set; }

        public int ClassNumber { get; set; }

        [Display(Name = "Students number")]
        public int StudentsNumber { get; set; }

        [Display(Name = "Teachers number")]
        public int TeachersNumber { get; set; }

        public List<StudentListViewModel> Students
        {
            get { return this.students; }
            set { this.students = value; }
        }

        public List<TeacherListViewModel> Teachers
        {
            get { return this.teachers; }
            set { this.teachers = value; }
        }
    }
}
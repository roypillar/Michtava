namespace Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SchoolClass : DeletableEntity
    {

        private ICollection<Student> students;
        private ICollection<Subject> subjects;
        private ICollection<Homework> activeHomeworks;

        public SchoolClass()
        {
            students = new HashSet<Student>();
            subjects = new HashSet<Subject>();
            activeHomeworks = new HashSet<Homework>();

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ClassLetter { get; set; }

        public int GradeYear { get; set; }

        public virtual ICollection<Student> Students
        {
            get { return this.students; }
            set { this.students = value; }
        }

        public virtual ICollection<Subject> Subjects
        {
            get { return this.subjects; }
            set { this.subjects = value; }
        }

        public virtual ICollection<Homework> ActiveHomeworks
        {
            get { return this.activeHomeworks; }
            set { this.activeHomeworks = value; }
        }

    }
}

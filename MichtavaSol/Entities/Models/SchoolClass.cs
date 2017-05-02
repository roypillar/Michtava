namespace Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SchoolClass : DeletableEntity
    {

        private ICollection<Student> students;
        private ICollection<Teacher> teachers;
        private ICollection<Subject> subjects;
        private ICollection<Homework> homeworks;

        public SchoolClass()
        {
            students = new HashSet<Student>();
            teachers = new HashSet<Teacher>();
            subjects = new HashSet<Subject>();
            homeworks = new HashSet<Homework>();

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ClassLetter { get; set; }

        public int ClassNumber { get; set; }

        public ICollection<Student> Students
        {
            get { return this.students; }
            set { this.students = value; }

        }

        public  ICollection<Teacher> Teachers
        {
            get { return this.teachers; }
            set { this.teachers = value; }
        }

        public virtual ICollection<Subject> Subjects
        {
            get { return this.subjects; }
            set { this.subjects = value; }
        }

        public virtual ICollection<Homework> Homeworks
        {
            get { return this.homeworks; }
            set { this.homeworks = value; }
        }
        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

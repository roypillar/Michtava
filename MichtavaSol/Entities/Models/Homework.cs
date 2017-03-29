using System;
using System.Collections.Generic;


namespace Entities.Models
{


    public class Homework : DeletableEntity
    {

        private ICollection<Question> questions;
        private ICollection<SchoolClass> schoolClasses;

        public Homework()
        {

            this.schoolClasses = new HashSet<SchoolClass>();
            this.questions = new HashSet<Question>();

        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public virtual Teacher Created_By { get; set; }

        public Text Text { get; set; }

        public ICollection<Question> Questions
        {
            get
            {
                return this.questions;
            }

            set
            {
                this.questions = value;
            }
        }

        public ICollection<SchoolClass> SchoolClasses
        {
            get
            {
                return this.schoolClasses;
            }

            set
            {
                this.schoolClasses = value;
            }
        }


    }
}

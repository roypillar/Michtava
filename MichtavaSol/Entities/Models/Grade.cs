using System;
using System.Collections.Generic;

namespace Entities.Models
{
 

    public class Grade : DeletableEntity
    {

        private IList<Subject> subjects;//optional for now

        public Grade()
        {
            this.subjects = new List<Subject>();
        }

        public string Id { get; set; }


        public virtual IList<Subject> Subjects
        {
            get { return this.subjects; }
            set { this.subjects = value; }
        }

        public int GradeYear { get; set; }
    }
}

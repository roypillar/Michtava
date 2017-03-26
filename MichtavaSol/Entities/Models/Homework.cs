using System;
using System.Collections.Generic;


namespace Entities.Models
{


    public class Homework : DeletableEntity
    {

        private ICollection<Question> questions { get; set; }
        private ICollection<SchoolClass> schoolClasses;

        public Homework()
        {
        
            this.schoolClasses = new HashSet<SchoolClass>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public virtual Teacher Created_By { get; set; }

        public Text Text { get; set; }

        public IList<Question> Questions { get; set; }


    }
}

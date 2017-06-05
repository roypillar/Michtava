using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    public class Teacher : DeletableEntity, HasId
    {
        public Teacher()
        {
            schoolClasses = new HashSet<SchoolClass>();
        }

        public Teacher(ApplicationUser user,string name)
        {
            schoolClasses = new HashSet<SchoolClass>();
            Name = name;
            ApplicationUser = user;
            if (user != null)

                ApplicationUserId = user.Id;
        }

        private ICollection<SchoolClass> schoolClasses;

        public virtual ICollection<SchoolClass> SchoolClasses
        {
            get { return this.schoolClasses; }
            set { this.schoolClasses = value;  }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

      
        [Required]
        public string Name { get; set; }


        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

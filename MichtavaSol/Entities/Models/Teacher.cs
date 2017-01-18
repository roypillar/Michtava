using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    public class Teacher : DeletableEntity
    {


        public virtual ICollection<SchoolClass> SchoolClasses
        {
            get { return this.SchoolClasses; }
            set { this.SchoolClasses = value; }
        }

        //[Key]??TODO
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [MaxLength(12, ErrorMessage = "שם המשתמש ארוך מדי")]
        [Required]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "אימייל ארוך מדי")]
        public string Email { get; set; }

        [MaxLength(255, ErrorMessage = "טלפון ארוך מדי")]
        public string PhoneNumber { get; set; }

        public Teacher()
            : base()
        {
           // this.classes = new HashSet<SchoolClass>();//TODO
        }
    }
}

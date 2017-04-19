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
        public Teacher()
        {
            schoolClasses = new HashSet<SchoolClass>();
        }

        private ICollection<SchoolClass> schoolClasses;

        public virtual ICollection<SchoolClass> SchoolClasses
        {
            get { return this.schoolClasses; }
            set { this.schoolClasses = value; }
        }

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

        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

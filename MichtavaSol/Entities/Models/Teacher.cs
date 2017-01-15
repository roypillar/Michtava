using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    public class Teacher : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }

        public string UserId { get; set; }


        [MaxLength(12, ErrorMessage = "שם המשתמש ארוך מדי")]
        [Required]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "אימייל ארוך מדי")]
        public string Email { get; set; }

        [MaxLength(255, ErrorMessage = "טלפון ארוך מדי")]
        public string PhoneNumber { get; set; }

        public Teacher()
            : base()
        { }
    }
}
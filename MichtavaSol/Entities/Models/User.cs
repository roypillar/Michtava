using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Pupil : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PupilId { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
        public bool IsCurrentDisplayed { get; set; }

        public Pupil() : base() { }

        //[ForeignKey("UserId")]
        //public virtual ApplicationUser User { get; set; }
    }
}
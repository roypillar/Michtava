using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class AddSubjectViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} חייב להיות קצר מ{1} תווים.")]
        [Display(Name = "נושא")]
        public string SubjectName { get; set; }
    }
}
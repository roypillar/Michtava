using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class SubjectsListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "נושא")]
        public string SubjectsName { get; set; }

    }
}
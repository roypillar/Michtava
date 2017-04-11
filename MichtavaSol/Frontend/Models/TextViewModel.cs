using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class TextViewModel
    {
        [Display(Name = "טקסט")]
        public string Name { get; set; }

        public DateTime UploadTime { get; set; }

        public string FilePath { get; set; }
    }
}
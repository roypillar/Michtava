using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Policy
    {
        [Required]
        public string _id { get; set; }

        [Required]
        public int _minWords { get; set; }

        [Required]
        public int _maxWords { get; set; }

        [Required]
        public int _minConnectors { get; set; }

        [Required]
        public int _maxConnectors { get; set; }

        [Required]
        public List<string> _keySentences { get; set; }
    }
}
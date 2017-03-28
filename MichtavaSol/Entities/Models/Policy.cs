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
        public string Id { get; set; }

        [Required]
        public int MinWords { get; set; }

        [Required]
        public int MaxWords { get; set; }

        [Required]
        public int MinConnectors { get; set; }

        [Required]
        public int MaxConnectors { get; set; }

        [Required]
        public List<string> KeySentences { get; set; }
    }
}
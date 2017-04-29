using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Policy
    {

        public Policy()
        {
            MinWords = 10;
            MaxWords = 100;
            MinConnectors = 2;
            MaxConnectors = 10;

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int MinWords { get; set; }

        [Required]
        public int MaxWords { get; set; }

        [Required]
        public int MinConnectors { get; set; }

        [Required]
        public int MaxConnectors { get; set; }
    }
}
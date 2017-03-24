using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
  
    public class Student : DeletableEntity
    {



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public SchoolClass SchoolClass { get; set; }
        

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<Homework> Homeworks
        {
            get { return this.Homeworks; }
            set { this.Homeworks = value; }
        }
    }
}
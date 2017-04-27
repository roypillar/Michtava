using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
  
    public class Student : DeletableEntity
    {

        public Student()
        {
            Homeworks = new HashSet<Homework>();

        }

        public ICollection<Homework> Homeworks
        {
            get; set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool isNotified { get; set; }

        public SchoolClass SchoolClass { get; set; }
        

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }



        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
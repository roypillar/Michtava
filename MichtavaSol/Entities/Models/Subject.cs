using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{

    public class Subject : DeletableEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalHours { get; set; }

        public ICollection<SchoolClass> schoolClasses { get; set; }//DO NOT USE THIS FIELD


        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

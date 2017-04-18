using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Answer : DeletableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime Date_Submitted { get; set; }

        public Homework Answer_To { get; set; }

        public Question question { get; set; }

        public Student Submitted_By { get; set; }

    }
}
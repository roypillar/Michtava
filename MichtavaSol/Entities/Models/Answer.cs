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

        public Answer()
        {
            Date_Submitted = DateTime.Now;
            answers = new Dictionary<Question, string>();
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime Date_Submitted { get; set; }

        public Homework Answer_To { get; set; }

        [ForeignKey("Answer_To")]
        public Guid Homework_Id { get; set; }


        public Dictionary<Question, string> answers { get; set; }

        public Student Submitted_By { get; set; }

        [ForeignKey("Submitted_By")]
        public Guid Student_Id { get; set; }

        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
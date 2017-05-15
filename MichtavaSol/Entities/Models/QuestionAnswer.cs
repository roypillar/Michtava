using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class QuestionAnswer : DeletableEntity
    {
        private string _content;
        public QuestionAnswer()
        {
        }
        public QuestionAnswer(Question quest, string content)
        {
            Date_Submitted = DateTime.Now;
            this.Of_Question = quest;
            this.Content = content;

        }



        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime Date_Submitted { get; set; }

        public Answer In_Answer { get; set; }

        [ForeignKey("In_Answer")]
        public Guid Answer_Id { get; set; }

        public string Content {
            set
            {
                Date_Submitted = DateTime.Now;
                _content = value;
            }
            get
            {
                return _content;
            }
           
        }

        public Question Of_Question { get; set;}

        [ForeignKey("Of_Question")]
        public Guid Question_Id { get; set; }

        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
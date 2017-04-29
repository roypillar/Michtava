using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{


    public class Homework : DeletableEntity
    {

        private ICollection<Question> questions;

        public Homework()
        {

            this.questions = new HashSet<Question>();

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public virtual Teacher Created_By { get; set; }

        [ForeignKey("Created_By")]
        public Guid Teacher_Id { get; set; }

        public Text Text { get; set; }

        [ForeignKey("Text")]
        public Guid Text_Id { get; set; }


        public ICollection<Question> Questions
        {
            get
            {
                return this.questions;
            }

            set
            {
                this.questions = value;
            }
        }

       


        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

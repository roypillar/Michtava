using Common.Exceptions;
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

        public Homework(string title, string desc, DateTime dl, Teacher created_by,
            Text t) : base()
        {
            if (created_by.Id.Equals(Guid.Empty))
                throw new MichtavaNoIdException("Cannot pass a teacher with no Id as an argument");

            if(t.Id.Equals(Guid.Empty))
                throw new MichtavaNoIdException("Cannot pass a text with no Id as an argument");

            Title = title;
            Description = desc;
            Deadline = dl;
            Created_By = created_by;
            Teacher_Id = created_by.Id;

            Text = t;
            Text_Id = t.Id;
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

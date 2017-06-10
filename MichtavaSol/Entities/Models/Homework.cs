using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{


    public class Homework : DeletableEntity, HasId
    {

        private ICollection<Question> questions;
        private Text _text;
        private Teacher _teacher;

        public Homework()
        {
            Deadline = DateTime.Now;
            this.questions = new HashSet<Question>();
        }

        public Homework(string title, string desc, DateTime dl, Teacher created_by,
            Text t) : base()
        {
            if(t==null || created_by == null)
                throw new ArgumentNullException("Please do not send null values to this constructor");

            if (created_by!=null && created_by.Id.Equals(Guid.Empty))
                throw new MichtavaNoIdException("Cannot pass a teacher with no Id as an argument");

            if(t!=null && t.Id.Equals(Guid.Empty))
                throw new MichtavaNoIdException("Cannot pass a text with no Id as an argument");

            Title = title;
            Description = desc;
            Deadline = dl;
            Created_By = created_by;
            Teacher_Id = created_by.Id;
            this.questions = new HashSet<Question>();

            Text = t;
            Text_Id = t.Id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public virtual Teacher Created_By { get
            {
                return _teacher;
            }

            set
            {
                _teacher = value;
                if (value != null)
                    Teacher_Id = value.Id;
            }

        }

        [ForeignKey("Created_By")]
        public Guid Teacher_Id { get; set; }

        public Text Text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (value != null)
                    Text_Id = value.Id;
            }
        }

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

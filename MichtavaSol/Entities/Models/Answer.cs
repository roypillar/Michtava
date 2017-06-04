using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Answer : DeletableEntity, HasId
    {
        private Homework _homework;
        private Student _student;
        private string _feedback;

        public Answer()
        {
            Date_Submitted = DateTime.Now;
            questionAnswers = new List<QuestionAnswer>();
        }

        public Answer(Homework hw, Student s)
        {
            Date_Submitted = DateTime.Now;
            questionAnswers = new List<QuestionAnswer>();
            Answer_To = hw;
            Submitted_By = s;
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime Date_Submitted { get; set; }

        public Homework Answer_To
        {
            get
            {
                return _homework;
            }

            set
            {
                _homework = value;
                if (value != null)
                    Homework_Id = value.Id;
            }
        }

        [ForeignKey("Answer_To")]
        public Guid Homework_Id { get; set; }

        //public int QuestionNumber { get; set; }

        public ICollection<QuestionAnswer> questionAnswers { get; set; }


        //public string QuestionAnswer { get; set; } 

        public Student Submitted_By
        {
            get
            {
                return _student;
            }

            set
            {
                _student = value;
                if (value != null)
                    Student_Id = value.Id;
            }
        }

        [ForeignKey("Submitted_By")]
        public Guid Student_Id { get; set; }

        [Range(0, 100)]
        public int? Grade { get; set; }


        public bool notifyStudentOfNewFeedback {get; set;}

        public string TeacherFeedback { get
            {
                return _feedback;
            }

            set {
                _feedback = value;
                notifyStudentOfNewFeedback = true;//this is where the flag is raised, it should be lowered manually by shay.
            }
        }

        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
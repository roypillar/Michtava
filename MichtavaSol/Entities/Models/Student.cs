using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
  
    public class Student : DeletableEntity, HasId
    {


        private ICollection<Homework> homeworks;
        public Student(ApplicationUser user,string name)
        {
            homeworks = new List<Homework>();
            Name = name;
            ApplicationUser = user;
            if(user!=null)
                ApplicationUserId = user.Id;

        }


        public Student()
        {
            homeworks = new List<Homework>();

        }

        public ICollection<Homework> Homeworks
        {
            get { return this.homeworks; }
            set { this.homeworks = value; }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool notifyForNewHomework { get; set; }

        public bool notifyForNewGrade { get; set; }


        public SchoolClass SchoolClass { get; set; }
        

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }



        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
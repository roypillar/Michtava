using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Text : DeletableEntity, HasId
    {
        private Subject _subject;

        public Text()
        {
            UploadTime = DateTime.Now;
        }

        public Text(string name,Subject s)
        {
            Name = name;
            Subject = s;
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime UploadTime { get; set; }

        public string FilePath { get; set; }

        public Subject Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
                if (value != null)
                    Subject_Id = value.Id;
            }
        }

        

        [ForeignKey("Subject")]
        public Guid Subject_Id { get; set; }
        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}
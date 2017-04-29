using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Question
    {

        public Question()
        {
            Date_Added = DateTime.Now;
        }

        public Question(string content,HashSet<string> so)
        {
            Date_Added = DateTime.Now;
            this.Content = content;
            this.Suggested_Openings = so;
            this.Policy = new Policy();
     
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Question_Number { get; set; }

        public string Content { get; set; }

        public DateTime Date_Added { get; set; }

        public HashSet<string> Suggested_Openings { get; set; }

        public Policy Policy { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Question : HasId
    {

        public Question()
        {
            Date_Added = DateTime.Now;
            Suggested_Openings = new List<SuggestedOpening>();
        }

        public Question(string content, ICollection<SuggestedOpening> so)
        {
            Date_Added = DateTime.Now;
            this.Content = content;
            this.Suggested_Openings = so;
            this.Policy = new Policy();
     
        }
        public Question(string content)
        {
            Date_Added = DateTime.Now;
            this.Content = content;
            Suggested_Openings = new List<SuggestedOpening>();
            this.Policy = new Policy();

        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Question_Number { get; set; }

        public string Content { get; set; }

        public DateTime Date_Added { get; set; }

        public ICollection<SuggestedOpening> Suggested_Openings { get; set; }

        public Policy Policy { get; set; }

    }
}
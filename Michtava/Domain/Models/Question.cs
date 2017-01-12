using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class Question
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime Date_Added { get; set; }

        public List<string> Suggested_Openings { get; set; }

        public Policy Policy { get; set; }



    }
}
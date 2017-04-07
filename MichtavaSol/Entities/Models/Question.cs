using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date_Added { get; set; }

        public HashSet<string> Suggested_Openings { get; set; }

        public Policy Policy { get; set; }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class Answer
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime Date_Submitted { get; set; }

    }
}
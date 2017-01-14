using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Task
    {
        public string Id { get; set; }

        public Text Text { get; set; }

        public List<Question> Questions { get; set; }
    }
}
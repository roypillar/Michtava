using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class StudentTextViewModel
    {
        public Text text { get; set; }

        public List<Tuple<string, string>> TextsTuple { get; set; }
    }
}
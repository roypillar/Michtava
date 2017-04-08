using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Models;



namespace Frontend.Models
{
    public class SmartTextViewModel
    {
        public Text text { get; set; }

        public List<Question> questions { get; set; }

        public Policy policy { get; set; }
    }
}

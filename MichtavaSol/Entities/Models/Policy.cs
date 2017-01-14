using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Policy
    {
        public string Id { get; set; }

        public int Min_Words { get; set; }

        public int Max_Words { get; set; }

        public int Min_Connectors { get; set; }

        public int Max_Connectors { get; set; }




    }
}
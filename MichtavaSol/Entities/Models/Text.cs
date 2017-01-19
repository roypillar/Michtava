using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class Text
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public DateTime Uploaded { get; set; }

        public string Content { get; set; }


    }
}
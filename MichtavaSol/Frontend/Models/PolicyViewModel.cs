using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class PolicyViewModel
    {
        // -------------------------------- Policy fields ---------------------------------

        public int Id { get; set; }

        public int MinWords { get; set; }

        public int MaxWords { get; set; }

        public int MinConnectors { get; set; }

        public int MaxConnectors { get; set; }

        public List<string> KeySentences { get; set; }

        public List<string> WordDefinitions { get; set; }

        // --------------------------------------------------------------------------------

        public string Question { get; set; }
    }
}
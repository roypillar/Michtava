using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class SuggestedOpening
    {
        public SuggestedOpening(string Content)
        {
            this.Content = Content;
        }

        public SuggestedOpening()
        {

        }

        public SuggestedOpening(SuggestedOpening so)
        {
            this.Content = so.Content;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public string Content { get; set; }

        public static List<SuggestedOpening> convert(ICollection<string> col)
        {
            List<SuggestedOpening> res = new List<SuggestedOpening>();
            foreach(string s in col)
            {
                res.Add(new SuggestedOpening(s));
            }
            return res;
        }
    }
}

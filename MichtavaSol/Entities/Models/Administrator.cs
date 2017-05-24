using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Administrator : DeletableEntity, HasId
    {
        public Administrator()
        {

        }

        public Administrator(string fn, string ln)
        {
            FirstName = fn;
            LastName = ln;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override void setId(Guid id)
        {
            Id = id;
        }

    }
}

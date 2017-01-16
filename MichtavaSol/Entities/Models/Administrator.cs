using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Administrator : DeletableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

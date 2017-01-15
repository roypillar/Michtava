using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public abstract class EntityBase
    {
        [Index(IsUnique = true)]
        [Required]
        public Guid Identifier { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public EntityBase()
        {
            this.Identifier = Guid.Empty;
            this.CreatedDate = DateTime.UtcNow;
            this.ModifiedDate = DateTime.UtcNow;
            this.IsActive = true;
            this.IsDeleted = false;
        }
    }
}

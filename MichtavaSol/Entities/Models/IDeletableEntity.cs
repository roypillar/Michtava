using System;


namespace Entities.Models
{

    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }

        string DeletedBy { get; set; }

         int TestID { get; set; }
    }
}

using System.Collections.Generic;

namespace Entities.Models
{

    public class Subject : DeletableEntity
    {


        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalHours { get; set; }

    }
}

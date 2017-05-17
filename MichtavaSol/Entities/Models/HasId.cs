using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    interface HasId
    {
         Guid Id { get; set; }
    }
}

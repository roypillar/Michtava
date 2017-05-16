using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class MichtavaNoIdException : Exception
    {
        public MichtavaNoIdException(string message) : base(message)
        {
        }
    }
}

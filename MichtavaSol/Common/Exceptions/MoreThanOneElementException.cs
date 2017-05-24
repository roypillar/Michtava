using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class MoreThanOneElementException : Exception
    {
        public MoreThanOneElementException(string message) : base(message)
        {
        }
    }
}

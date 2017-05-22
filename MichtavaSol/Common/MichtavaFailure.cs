using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MichtavaFailure : MichtavaResult
    {
        public MichtavaFailure(string message) : base(message)
        {
        }

        public MichtavaFailure() : base("בעיה לא צפויה קרתה, נסה שוב")
        {

        }


    }
}

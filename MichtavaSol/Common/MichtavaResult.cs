using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class MichtavaResult
    {
    public MichtavaResult(string message)
    {
            Message = message;
    }
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.ExceptionFiltering
{
    public class UnrecognisedTypeException : Exception
    {
        public UnrecognisedTypeException(string message) : base(message)
        {
        }
    }
}

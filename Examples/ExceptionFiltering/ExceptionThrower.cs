using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.ExceptionFiltering
{
    public class ExceptionThrower
    {
        private readonly ExceptionFilter _filter;

        public ExceptionThrower(ExceptionFilter filter)
        {
            _filter = filter;
        }

        public void ThrowExceptionForFilter(string exceptionType, string message)
        {
            try
            {

            }
            catch (Exception e)
            {
                _filter.RespondToThrownException(e);
            }
        }
    }
}

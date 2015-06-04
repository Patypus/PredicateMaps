using Examples.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.ExceptionFiltering
{
    public class ExceptionThrower
    {
        private readonly IDictionary<string, Type> _supportedExceptionTypes;

        public ExceptionThrower()
        {
            _supportedExceptionTypes = CreateDictionaryOfSupportedTypes();
        }

        public void ThrowExceptionForFilter(string exceptionType, string message)
        {
            if (!_supportedExceptionTypes.ContainsKey(exceptionType))
            {
                var exceptionMessage = string.Format(Strings.UnrecognisedExceptionType, exceptionType);
                throw new UnrecognisedTypeException(exceptionMessage);
            }
            ThrowCorrectException(exceptionType, message);            
        }

        private void ThrowCorrectException(string exceptionType, string message)
        {
            var type = _supportedExceptionTypes[exceptionType];
            var instance = Activator.CreateInstance(type, new[] { message }) as Exception;
            throw instance;
        }

        private IDictionary<string, Type> CreateDictionaryOfSupportedTypes()
        {
            return new Dictionary<string, Type>()
            {
                { typeof(ArgumentException).Name, typeof(ArgumentException) },
                { typeof(NotImplementedException).Name, typeof(NotImplementedException) },
                { typeof(NullReferenceException).Name, typeof(NullReferenceException) }
            }; 
        }
    }
}
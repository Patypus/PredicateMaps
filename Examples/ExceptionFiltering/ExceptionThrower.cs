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

        //This method throws the exception that the user has requested, provided that the type can be interpreded.
        //The unrecognised type exception is thrown if the type cannot be recognised and is also handled by the filter.
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

        //This dictionary maps the names of exception types to the types that the Activator can use.
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
using Examples.Resources;
using PredicateMaps.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.ExceptionFiltering
{
    public class ExceptionFilter
    {
        private readonly IPredicateMap<Exception, string> _filter;

        public ExceptionFilter()
        {
            _filter = CreateFilterMap();
        }

        public string RespondToThrownException(Exception thrown)
        {
            return _filter.GetFirstMatch(thrown);
        }

        public static IEnumerable<string> GetHandledTypesDescriptions()
        {
            return new List<string>()
            {
                "These types are have special handling in the ExceptionFilter:",
                "ArgumentExceptions",
                "NotImplementedExceptions without the word 'yet' in the message",
                "NotImplementedExceptions with the word 'yet' in the message",
                "NullReferenceException",
                "Enter the type name of the exception to throw for the filter to catch. If the name is not recognised an UnrecognisedTypeException will be thrown."
            };
        }

        //This predicate map is more complex as it uses methods defined below as the predicates for the map.
        //This setup allows for more complex conditions to be used as the map's matching predicates.
        private IPredicateMap<Exception, string> CreateFilterMap()
        {
            var exceptionFilters = new Dictionary<Predicate<Exception>, string>()
            {
                { IsArgumentException, Strings.ArgumentExceptionMessage },
                { IsNotImplementedException, Strings.NotImplementedExceptionMessage },
                { IsNotImplementedExceptionTemporarily, Strings.NotCurrentlyImplementedExceptionMessage },
                { IsNullReferenceException, Strings.NullReferenceExceptionMessage },
                { IsUnrecognisedTypeException, Strings.IsUnrecognisedTypeException }
            };
            var defaultMessage = Strings.UnableToHandlePassedException;
            return new PredicateMap<Exception, string>(exceptionFilters, defaultMessage);
        }
        
        private bool IsArgumentException(Exception exception)
        {
            return exception is ArgumentException;
        }

        private bool IsNotImplementedException(Exception exception)
        {
            return exception is NotImplementedException && !exception.Message.Contains("yet");
        }
        
        private bool IsNotImplementedExceptionTemporarily(Exception exception)
        {
            return exception is NotImplementedException && exception.Message.Contains("yet");
        }

        private bool IsNullReferenceException(Exception exception)
        {
            return exception is NullReferenceException;
        }

        private bool IsUnrecognisedTypeException(Exception exception)
        {
            return exception is UnrecognisedTypeException;
        }
    }
}
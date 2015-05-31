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
            return null;
        }

        private IPredicateMap<Exception, string> CreateFilterMap()
        {
            var exceptionFilters = new Dictionary<Predicate<Exception>, string>()
            {
                { IsArgumentException, Strings.ArgumentExceptionMessage },
                { IsNotImplementedException, Strings.NotImplementedExceptionMessage },
                { IsNotImplementedExceptionTemporarily, Strings.},
                { IsNullReferenceException, Strings.NullReferenceExceptionMessage}
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
    }
}
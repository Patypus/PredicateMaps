using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Exceptions
{
    public class InconsitentIndexException : Exception
    {
        public InconsitentIndexException(int keyCollectionSize, int valueCollectionSize) 
            : base(string.Format(strings.InconsistentCollectionSizeError, keyCollectionSize, valueCollectionSize))
        {
        }
    }
}

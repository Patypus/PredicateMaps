using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Exceptions
{
    public class InconsistentIndexException : Exception
    {
        public InconsistentIndexException(int keyCollectionSize, int valueCollectionSize)
            : base(string.Format(StringResources.InconsistentCollectionSizeError, keyCollectionSize, valueCollectionSize))
        {
        }
    }
}

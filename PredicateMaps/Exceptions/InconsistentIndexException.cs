using PredicateMaps.Resources;
using System;

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

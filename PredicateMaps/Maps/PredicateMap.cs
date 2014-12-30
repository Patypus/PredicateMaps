using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    public class PredicateMap<K, V>
    {
        private readonly List<Predicate<K>> keyPredicateList;
        private readonly List<V> valueList;

        public PredicateMap()
        {
            keyPredicateList = new List<Predicate<K>>();
            valueList = new List<V>();
        }



    }
}

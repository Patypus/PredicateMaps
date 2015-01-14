using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    public class PredicateMap<K, V>
    {
        public List<Predicate<K>> keyPredicateList { get; private set; }
        public List<V> valueList { get; private set; }

        public PredicateMap()
        {
            keyPredicateList = new List<Predicate<K>>();
            valueList = new List<V>();
        }

        public PredicateMap( List<Predicate<K>> keyList, List<V> dataList )
        {
            if(keyList == null || dataList == null) {
                var message = keyList == null ? Strings.InvalidKeyCollectionParameter : Strings.InvalidDataCollectionParameter;
                throw new ArgumentException(message);
            }
            if (keyList.Count != dataList.Count) {
                throw new InconsistentIndexException(keyList.Count, dataList.Count);
            }
            keyPredicateList = keyList;
            valueList = dataList;
        }

        public int GetCount()
        {
            return keyPredicateList.Count;
        }

        public void Add(Predicate<K> key, V value)
        {
            keyPredicateList.Add(key);
            valueList.Add(value);
        }
    }
}

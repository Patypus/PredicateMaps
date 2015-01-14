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

        /// <summary>
        /// Creates a PredicateMap which is has the contents of the keyList and dataList params populated in it.
        /// The collections for keys and data must be of the same size.
        /// </summary>
        /// <param name="keyList">Collection of typed Predicates to use as the keys collection in the new map.</param>
        /// <param name="dataList">Typed collection of data to include in the map.</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and dataList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public PredicateMap(List<Predicate<K>> keyList, List<V> dataList)
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

        /// <summary>
        /// Returns the number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
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

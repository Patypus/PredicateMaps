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

        /// <summary>
        /// Creates a new PredicateMap with empty key and value lists
        /// </summary>
        public PredicateMap()
        {
            keyPredicateList = new List<Predicate<K>>();
            valueList = new List<V>();
        }

        /// <summary>
        /// Creates a PredicateMap which is has the contents of the keyList and valuesList params populated in it.
        /// The collections for keys and values must be of the same size.
        /// </summary>
        /// <param name="keyList">Collection of typed Predicates to use as the keys collection in the new map.</param>
        /// <param name="valuesList">Typed collection of values to include in the map.</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and dataList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public PredicateMap(List<Predicate<K>> keyList, List<V> valuesList)
        {
            if(keyList == null || valuesList == null) {
                var message = keyList == null ? StringResources.InvalidKeyCollectionParameter : 
                                                StringResources.InvalidDataCollectionParameter;
                throw new ArgumentException(message);
            }
            if (keyList.Count != valuesList.Count) {
                throw new InconsistentIndexException(keyList.Count, valuesList.Count);
            }
            keyPredicateList = keyList;
            valueList = valuesList;
        }

        /// <summary>
        /// Returns the number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        public int GetCount()
        {
            return keyPredicateList.Count;
        }

        /// <summary>
        /// Add a key value pair to the map. The value can be retrieved later with values supplied to the Get methods in this class
        /// which return true for the key Predicate supplied.
        /// </summary>
        /// <param name="key">Predicate which can be used to return the value when it evaluates to true.</param>
        /// <param name="value">Value item to be stored.</param>
        public void Add(Predicate<K> key, V value)
        {
            if (key == null || value == null) 
            {
                var message = key == null ? StringResources.InvalidKeyParameter
                                          : StringResources.InvalidDataParameter;
                throw new ArgumentException(message);
            }
            keyPredicateList.Add(key);
            valueList.Add(value);
        }
    }
}

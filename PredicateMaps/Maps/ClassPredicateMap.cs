using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    public class ClassPredicateMap<K, V> : IPredicateMap<K, V> where V : class
    {
        private readonly int NO_VALUE_FOUND = -1;
        public List<Predicate<K>> keyPredicateList { get; private set; }
        public List<V> valueList { get; private set; }

        /// <summary>
        /// Creates a new PredicateMap with empty key and value lists
        /// </summary>
        public ClassPredicateMap()
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
        public ClassPredicateMap(List<Predicate<K>> keyList, List<V> valuesList)
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

        /// <summary>
        /// Method to find the value which is associated with the first predicate to resolve to true for the valueToTest parameter.
        /// 
        /// This method only finds the value for the first predicate which resolves to true with the value supplied through
        /// the valueToTest parameter. To find values associated with all predicates which resolve to true for the supplied
        /// parameter use GetAllMatches
        /// </summary>
        /// <param name="valueToTest">Value to test in key predicates</param>
        /// <returns>Value associated with first predicate found that is true for the valueToTest parameter</returns>
        public V GetFirstMatch(K valueToTest)
        {
            var indexForValue = GetIndexOfFirstMatch(valueToTest);
            return indexForValue != NO_VALUE_FOUND ? valueList[indexForValue] : null;
        }

        private int GetIndexOfFirstMatch(K valueToTest)
        {
            foreach (var keyPred in keyPredicateList) {
                var valueMatchesPredicate = keyPred.Invoke(valueToTest);
                if (valueMatchesPredicate)
                {
                    //Quick return for performence over completeness.
                    return keyPredicateList.IndexOf(keyPred);
                }
            }
            //Return nothing found if no predicates evaluate to true for the given value
            return NO_VALUE_FOUND;
        }

        /// <summary>
        /// Adds all elements in key and value lists to the map. The values from both lists are matched by index and must be provided in
        /// the correct order. If the size of the keyList and valueList do not match this method will throw an 
        /// InconsistentIndexException.
        /// 
        /// Null is not a valid value for either parameter
        /// </summary>
        /// <param name="keyList">Collection of predicates for use as keys</param>
        /// <param name="valuesList">Collection of values for retreival when the key predicate evaluates to true.</param>
        /// /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public void AddAll(List<Predicate<K>> keyList, List<V> valueList)
        {
            throw new NotImplementedException();
        }
    }
}
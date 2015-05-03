using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Implementation of the IPredicateMap interface for which wraps a dictionary of type
    /// predicate of K to value of type V (which extend class). The wrapper simplifies access 
    /// to the dictionary for the user. The mapping is add only. Once a value is in it cannot 
    /// be removed or updated to have a 
    /// new value (currently - see open project issues). 
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates</typeparam>
    /// <typeparam name="V">Class type of values</typeparam>
    public class PredicateMap<K, V> : IPredicateMap<K, V> where V : class
    {
        private const int NO_VALUE_FOUND = -1;

        private readonly IDictionary<Predicate<K>, V> _storageMap;

        /// <summary>
        /// Creates a new PredicateMap with empty key and value lists
        /// </summary>
        public PredicateMap()
        {
            _storageMap = new Dictionary<Predicate<K>, V>();
        }

        /// <summary>
        /// Creates a new PredicateMap populated with the values from the supplied dictionary.
        /// </summary>
        /// <param name="mappingToWrap">Dictionary containing all the keys and values to include in the predicate map.</param>
        /// <exception cref="ArgumentException">Thrown if mapToWrap parameter value is null.</exception>
        public PredicateMap(IDictionary<Predicate<K>, V> mappingToWrap)
        {
            if (mappingToWrap == null)
            {
                throw new ArgumentException(StringResources.InvalidDictionaryConstructorParameter);
            }
            _storageMap = mappingToWrap;
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
            CheckValidityOfMultipleAddParameters(keyList, valuesList);

            _storageMap = new Dictionary<Predicate<K>, V>();
            AddAll(keyList, valuesList);
        }

        private void CheckValidityOfMultipleAddParameters(List<Predicate<K>> keyList, List<V> valueList)
        {
            if (keyList == null || valueList == null)
            {
                var message = keyList == null ? StringResources.InvalidKeyCollectionParameter :
                                                StringResources.InvalidDataCollectionParameter;
                throw new ArgumentException(message);
            }
            if (keyList.Count != valueList.Count)
            {
                throw new InconsistentIndexException(keyList.Count, valueList.Count);
            }
        }

        public List<Predicate<K>> KeyPredicateList()
        {
            return _storageMap.Keys.ToList();
        }

        public List<V> ValueItemList()
        {
            return _storageMap.Values.ToList();
        }

        /// <summary>
        /// Returns the number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        public int GetCount()
        {
            return _storageMap.Count;
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
            _storageMap.Add(key, value);
        }

        /// <summary>
        /// Method to find the value which is associated with the first predicate to resolve to true for the valueToTest parameter.
        /// 
        /// This method only finds the value for the first predicate which resolves to true with the value supplied through
        /// the valueToTest parameter. To find values associated with all predicates which resolve to true for the supplied
        /// parameter use GetAllMatches.
        /// </summary>
        /// <param name="valueToTest">Value to test in key predicates</param>
        /// <returns>Value associated with first predicate found that is true for the valueToTest parameter</returns>
        public V GetFirstMatch(K valueToTest)
        {
            var matches = _storageMap.Where(pair => pair.Key.Invoke(valueToTest));
            return matches.Any() ? matches.First().Value : null;
        }

        /// <summary>
        /// Adds all elements in key and value lists to the map. The values from both lists are matched by index and must be provided in
        /// the correct order. If the size of the keyList and valueList do not match this method will throw an 
        /// InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates for use as keys</param>
        /// <param name="valueList">Collection of values for retreival when the key predicate evaluates to true.</param>
        /// /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public void AddAll(List<Predicate<K>> keyList, List<V> valueList)
        {
            CheckValidityOfMultipleAddParameters(keyList, valueList);

            for (var index = 0; index < keyList.Count; index++)
            {
                _storageMap.Add(keyList[index], valueList[index]);
            }
        }

        /// <summary>
        /// Finds all values where valueToTest evaluates the prdicate related to the value to true.
        /// 
        /// This method favours completeness over speed and will evaluate all predicates in the collection even
        /// if only one near the start evaluates to true. If you know that the Map contains only a single matching
        /// predicate for valueToTest use GetFirstMatch for performance. 
        /// </summary>
        /// <param name="valueToTest">Value to test against the predicate keys</param>
        /// <returns>A list of all matches</returns>
        public List<V> GetAllMatches(K valueToTest)
        {
            return _storageMap.Where(pair => pair.Key.Invoke(valueToTest)).Select(match => match.Value).ToList();
        }

        /// <summary>
        /// Counts the number of predicates in the map which evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to evaluate predicates with.</param>
        /// <returns>The number of predicates which are true for valueToTest</returns>
        public int CountMatches(K valueToTest)
        {
            return _storageMap.AsParallel().Count(pair => pair.Key.Invoke(valueToTest));
        }

        /// <summary>
        /// Method to find if the map contains any matches for the value of valueToTest.
        /// </summary>
        /// <param name="valueToTest">Value to run in predicate keys</param>
        /// <returns>true when a predicate returns true for valueToTest, false otherwise.</returns>
        public bool AnyMatches(K valueToTest)
        {
            return _storageMap.Keys.Any((predicate) => predicate.Invoke(valueToTest));
        }
    }
}

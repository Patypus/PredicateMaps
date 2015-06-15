using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Implementation of the IPredicateMap interface for which wraps a dictionary of Predicates
    /// of type K to value of type V. The wrapper simplifies access to the dictionary for the user. 
    /// The mapping is add only. Once a value is in it cannot be removed or updated to have a 
    /// new value (currently - see open project issues). 
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates</typeparam>
    /// <typeparam name="V">Type of values to map to predicates</typeparam>
    public class PredicateMap<K, V> : IPredicateMap<K, V>
    {
        private const int NO_VALUE_FOUND = -1;

        private readonly IDictionary<Predicate<K>, V> _storageMap;

        private V _defaultValue;

        /// <summary>
        /// Creates an new, empty PredicateMap containing no values to look up. The provided default value is used as a 
        /// return value when the map is asked for matches to a value of type K when no matches exist.
        /// <param name="defaultValue">Value to return when no matches are found. Null is a valid value for this parameter.</param>
        /// </summary>
        public PredicateMap(V defaultValue)
        {
            _storageMap = new Dictionary<Predicate<K>, V>();
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Creates a new PredicateMap populated with the values from the supplied dictionary. The provided default value is used as a 
        /// return value when the map is asked for matches to a value of type K when no matches exist.
        /// </summary>
        /// <param name="mappingToWrap">Dictionary containing all the keys and values to include in the predicate map.
        /// Null is not a valid value for this parameter.</param>
        /// <param name="defaultValue">Value to return when no matches are found. Null is a valid value for this parameter.</param>
        /// <exception cref="ArgumentException">Thrown if mappingToWrap parameter value is null.</exception>
        public PredicateMap(IDictionary<Predicate<K>, V> mappingToWrap, V defaultValue)
        {
            if (mappingToWrap == null)
            {
                throw new ArgumentException(StringResources.InvalidDictionaryConstructorParameter);
            }
            _storageMap = mappingToWrap;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Creates a PredicateMap which is has the contents of the keyList and valuesList params populated in it.
        /// The provided default value is used as a return value when the map is asked for matches to a value of type
        /// K when no matches exist. The collections for keys and values must be of the same size or an InconsistentIndexException
        /// will be thrown.
        /// </summary>
        /// <param name="keyList">Collection Predicates with parameters of type K to use as the keys collection in the map.</param>
        /// <param name="valuesList">Collection of values of type V to include in the map.</param>
        /// <param name="defaultValue">Value to return when no matches are found. Null is a valid value for this parameter.</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and dataList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either of the list parameters are null.</exception>
        public PredicateMap(List<Predicate<K>> keyList, List<V> valuesList, V defaultValue)
        {
            CheckValidityOfMultipleAddParameters(keyList, valuesList);

            _storageMap = new Dictionary<Predicate<K>, V>();
            AddAll(keyList, valuesList);
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Method to set the default value that is returned when no matches for a value are found in the 
        /// GetFirstMatch method. Setting the default value via this method ovrerides any value that has
        /// previously been set.
        /// </summary>
        /// <param name="defaultValue">Value to set the default value to. Null is a valid value for this parameter.</param>
        public void SetDefaultValue(V defaultValue)
        {
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Retrieves the Predicates of type K which are the keys in the map.
        /// </summary>
        /// <returns>Collection of all keys that are in the map.</returns>
        public IList<Predicate<K>> KeyPredicateList()
        {
            return _storageMap.Keys.ToList();
        }

        /// <summary>
        /// Retrieves collection of values of type V in the map.
        /// </summary>
        /// <returns>Collection of all values that are in the map.</returns>
        public IList<V> ValueItemList()
        {
            return _storageMap.Values.ToList();
        }

        /// <summary>
        /// Returns the total number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        public int GetCount()
        {
            return _storageMap.Count;
        }

        /// <summary>
        /// Adds a new key value pair to the map.
        /// </summary>
        /// <param name="key">Predicate key to identify the value pair in the map</param>
        /// <param name="value">Value item to be stored against the predicate key</param>
        /// <exception cref="ArgumentException">Thrown if the key parameter value is null</exception>
        public void Add(Predicate<K> key, V value)
        {
            if (key == null) 
            {
                throw new ArgumentException(StringResources.InvalidKeyParameter);
            }
            _storageMap.Add(key, value);
        }

        /// <summary>
        /// Adds all elements in key and value lists to the map, matching predicates to values in the provided collections
        /// by index which must be provided in the correct order. If the size of the keyList and valueList do not match this 
        /// method will throw an InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates of type K for use as keys</param>
        /// <param name="valueList">Collection of values of type V for use as values</param>
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
        /// Adds all elements in the provided dictionary to the map.
        /// </summary>
        /// <param name="keyValueDictionary">Dictionary containing new predicate to value pairs to add to the map. 
        /// Null is not a valid value for this parameter.</param>
        /// <exception cref="ArgumentException">Thrown if the dictionary parameter value is null.</exception>
        public void AddAll(IDictionary<Predicate<K>, V> keyValueDictionary)
        {
            if (keyValueDictionary == null)
            {
                throw new ArgumentException(StringResources.InvalidDictionaryAddAllParameter);
            }

            foreach (var pair in keyValueDictionary)
            {
                _storageMap.Add(pair);
            }
        }

        /// <summary>
        /// Method to find the value which is associated with the first predicate to resolve to true for the valueToTest parameter.
        /// If no matches are found then the default value is returned. This value is set in the constructor or via the SetDefaultValue
        /// method.
        /// 
        /// This method only finds the value associated with the first predicate which resolves to true with the value supplied through
        /// the valueToTest parameter. To find values associated with all predicates which resolve to true for the supplied
        /// parameter use GetAllMatches.
        /// </summary>
        /// <param name="valueToTest">Value to test in key predicates to find a match for.</param>
        /// <returns>Value associated with first predicate found that is true for the valueToTest parameter</returns>
        public V GetFirstMatch(K valueToTest)
        {
            var matches = _storageMap.Where(pair => pair.Key.Invoke(valueToTest));
            return matches.Any() ? matches.First().Value : _defaultValue;
        }

        /// <summary>
        /// Finds all values where valueToTest evaluates the prdicate related to the value to true.
        /// An empty list is returned if no predicates evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to test with the predicate keys</param>
        /// <returns>A list of all values whose predicate key is true for valueToTest</returns>
        public List<V> GetAllMatches(K valueToTest)
        {
            return _storageMap.Where(pair => pair.Key.Invoke(valueToTest)).Select(match => match.Value).ToList();
        }

        /// <summary>
        /// Counts the number of predicates in the map which evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to count matches for</param>
        /// <returns>The number of predicates which evaluate to true for valueToTest</returns>
        public int CountMatches(K valueToTest)
        {
            return _storageMap.AsParallel().Count(pair => pair.Key.Invoke(valueToTest));
        }

        /// <summary>
        /// Returns true if any predicates in the map evaluate to true for valueToTest otherwise false.
        /// </summary>
        /// <param name="valueToTest">Value to test predicate keys with</param>
        /// <returns>True if any predicate returns true for valueToTest, false otherwise.</returns>
        public bool AnyMatches(K valueToTest)
        {
            return _storageMap.Keys.Any((predicate) => predicate.Invoke(valueToTest));
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
    }
}
using PredicateMaps.Exceptions;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Implementation of the IPredicateToFunctionMap where V must be of type class.
    /// Implementation maps predicates taking a parameter of type K to a function which takes a parameter of type K and returns 
    /// type V. Values of type K can be tested by the predicate key and where the predicate evaluates to true the value will be passed
    /// to the value function as its parameter and the return of type V from the matching function will be returned.
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates and for value functions to take as parameters</typeparam>
    /// <typeparam name="V">Return type for the value function to return</typeparam>
    public class PredicateToFunctionMap<K, V> : IPredicateToFunctionMap<K, V> where V : class
    {
        private readonly IDictionary<Predicate<K>, Func<K, V>> _storageMap;
        private V _defaultValue;

        /// <summary>
        /// Initialises a new PredicateToFunctionMap with initialised but empty key and value lists.
        /// </summary>
        /// <param name="defaultValue">Default value to return when no matches are found for a value of K</param>
        public PredicateToFunctionMap(V defaultValue)
        {
            _storageMap = new Dictionary<Predicate<K>, Func<K, V>>();
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Initialises a new PredicateToFunctionMap with the key values from the supplied dictionary in the 
        /// map.
        /// </summary>
        /// <param name="keyValueDictionary">Prepopulated key value dictionary to enter to the new map</param>
        /// <param name="defaultValue">Default value to return when no matches are found for a value of K</param>
        /// /// <exception cref="ArgumentException">Thrown if keyValueDictionary is null</exception>
        public PredicateToFunctionMap(IDictionary<Predicate<K>, Func<K, V>> keyValueDictionary, V defaultValue)
        {
            if (keyValueDictionary == null)
            {
                var message = string.Format(StringResources.InvalidDictionaryConstructorParameter, this.GetType().Name);
                throw new ArgumentException(message);
            }
            _storageMap = keyValueDictionary;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Initialises a new PredicateToFunctionMap populated with the keys and values from the respective collections supplied.
        /// The keys and values are matched together by their indexes in the two collections. If the 2 collections are different
        /// lengths an InconsistentIndexException will be thrown.
        /// </summary>
        /// <param name="keyList"></param>
        /// <param name="functionValueList"></param>
        /// <param name="defaultValue">Default value to return when no matches are found for a value of K</param>
        /// <exception cref="ArgumentException">Thrown if keyList or functionValueList is null</exception>
        /// <exception cref="InconsistentIndexException">Thrown if the number of elements in keyList and functionValueList do not match</exception>
        public PredicateToFunctionMap(IList<Predicate<K>> keyList, IList<Func<K, V>> functionValueList, V defaultValue)
        {
            _storageMap = new Dictionary<Predicate<K>, Func<K, V>>();

            CheckValidityOfMultipleAddParameters(keyList, functionValueList);

            for (var index = 0; index < keyList.Count; index++)
            {
                _storageMap.Add(keyList[index], functionValueList[index]);
            }
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
        /// Retrieves collection of value functions in the map
        /// </summary>
        /// <returns>All value functions which have been added to the map up to this point.</returns>
        public IList<Func<K, V>> ValueFunctionList()
        {
            return _storageMap.Values.ToList();
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
        /// Gets the number of entries in this map.
        /// </summary>
        /// <returns>The number of key/value pairs in this map.</returns>
        public int GetCount()
        {
            return _storageMap.Count;
        }

        /// <summary>
        /// Method to append the provided key value pair to the map.
        /// </summary>
        /// <param name="key">Key predicate to add to the map</param>
        /// <param name="valueFunction">Value function to associate with the given key in the map</param>
        /// <exception cref="ArgumentException">Thrown if either parameter is null</exception>
        public void Add(Predicate<K> key, Func<K, V> valueFunction)
        {
            if (key == null || valueFunction == null)
            {
                var message = key == null 
                                ? StringResources.InvalidKeyParameter
                                : StringResources.InvalidDataParameter;
                throw new ArgumentException(message);
            }
            
            _storageMap.Add(key, valueFunction);
        }

        /// <summary>
        /// Method which adds all of the key value pairs from the provided dictionary to the map.
        /// </summary>
        /// <param name="keyValuesToAdd">Dictionary of key value pairs to add to the map</param>
        /// <exception cref="ArgumentException">Thrown if keyValueToAdd parameter is null.</exception>
        public void AddAll(IDictionary<Predicate<K>, Func<K, V>> keyValuesToAdd)
        {
            if (keyValuesToAdd == null)
            {
                throw new ArgumentException(StringResources.InvalidDictionaryAddAllParameter);
            }

            foreach (var pair in keyValuesToAdd)
            {
                _storageMap.Add(pair);
            }
        }

        /// <summary>
        /// Adds all elements in key and value lists to the map, matching predicates to values in the provided collections
        /// by index which must be provided in the correct order. If the size of the keyList and valueList do not match this 
        /// method will throw an InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates of type K for use as keys</param>
        /// <param name="valueList">Collection of value function of parameter type K and return type V for use as values</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public void AddAll(IList<Predicate<K>> predicateListToAdd, IList<Func<K, V>> valueListToAdd)
        {
            CheckValidityOfMultipleAddParameters(predicateListToAdd, valueListToAdd);

            for (var index = 0; index < predicateListToAdd.Count; index++)
            {
                _storageMap.Add(predicateListToAdd[index], valueListToAdd[index]);
            }
        }

        /// <summary>
        /// Finds a function associated the first predicate which resolves to true for the valueToTest parameter and evaluates this
        /// function to get a value of V. 
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
            return matches.Any()
                    ? matches.First().Value.Invoke(valueToTest)
                    : _defaultValue;
        }

        private void CheckValidityOfMultipleAddParameters(IList<Predicate<K>> keyList, IList<Func<K, V>> valueList)
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
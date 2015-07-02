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
    internal class PredicateToFunctionMap<K, V> : IPredicateToFunctionMap<K, V> where V : class
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
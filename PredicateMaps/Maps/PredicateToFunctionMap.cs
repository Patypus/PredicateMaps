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

        /// <summary>
        /// Initialises a new PredicateToFunctionMap with initialised but empty key and value lists.
        /// </summary>
        public PredicateToFunctionMap()
        {
            _storageMap = new Dictionary<Predicate<K>, Func<K, V>>();
        }

        /// <summary>
        /// Initialises a new PredicateToFunctionMap with the key values from the supplied dictionary in the 
        /// map.
        /// </summary>
        /// <param name="keyValueDictionary">Prepopulated key value dictionary to enter to the new map</param>
        public PredicateToFunctionMap(IDictionary<Predicate<K>, Func<K, V>> keyValueDictionary)
        {
            _storageMap = keyValueDictionary;
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
    }
}
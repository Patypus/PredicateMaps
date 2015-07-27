using System;
using System.Collections.Generic;

using PredicateMaps.Exceptions;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Interface which defines a class which maps predicates of type K against values of type V.
    /// Values can be tested against the predicate keys to find V type values associated with
    /// the predicates which evaluate to true for the value.
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates</typeparam>
    /// <typeparam name="V">Type of values to map to predicate keys</typeparam>
    public interface IPredicateMap<K, V>
    {
        /// <summary>
        /// Retrieves the Predicates of type K which are the keys in the map.
        /// </summary>
        /// <returns>Collection of all keys that are in the map.</returns>
        IList<Predicate<K>> KeyPredicateList();

        /// <summary>
        /// Retrieves collection of values of type V in the map.
        /// </summary>
        /// <returns>Collection of all values that are in the map.</returns>
        IList<V> ValueItemList();

        /// <summary>
        /// Returns the total number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        int GetCount();

        /// <summary>
        /// Adds a new key value pair to the map.
        /// </summary>
        /// <param name="key">Predicate key to identify the value pair in the map</param>
        /// <param name="value">Value item to be stored against the predicate key</param>
        /// <exception cref="ArgumentException">Thrown if the key parameter value is null</exception>
        void Add(Predicate<K> key, V value);

        /// <summary>
        /// Adds all elements in key and value lists to the map, matching predicates to values in the provided collections
        /// by index which must be provided in the correct order. If the size of the keyList and valueList do not match this 
        /// method will throw an InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates of type K for use as keys</param>
        /// <param name="valueList">Collection of values of type V for use as values</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        void AddAll(List<Predicate<K>> keyList, List<V> valueList);

        /// <summary>
        /// Adds all elements in the provided dictionary to the map.
        /// </summary>
        /// <param name="keyValueDictionary">Dictionary containing new predicate to value pairs to add to the map. 
        /// Null is not a valid value for this parameter.</param>
        /// <exception cref="ArgumentException">Thrown if the dictionary parameter value is null.</exception>
        void AddAll(IDictionary<Predicate<K>, V> keyValueDictionary);

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
        V GetFirstMatch(K valueToTest);

        /// <summary>
        /// Finds all values where valueToTest evaluates the prdicate related to the value to true.
        /// An empty list is returned if no predicates evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to test with the predicate keys</param>
        /// <returns>A list of all values whose predicate key is true for valueToTest</returns>
        List<V> GetAllMatches(K valueToTest);

        /// <summary>
        /// Returns true if any predicates in the map evaluate to true for valueToTest otherwise false.
        /// </summary>
        /// <param name="valueToTest">Value to test predicate keys with</param>
        /// <returns>True if any predicate returns true for valueToTest, false otherwise.</returns>
        bool AnyMatches(K valueToTest);

        /// <summary>
        /// Counts the number of predicates in the map which evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to count matches for</param>
        /// <returns>The number of predicates which evaluate to true for valueToTest</returns>
        int CountMatches(K valueToTest);

        /// <summary>
        /// Method to set the default value that is returned when no matches for a value are found in the 
        /// GetFirstMatch method. Setting the default value via this method ovrerides any value that has
        /// previously been set.
        /// </summary>
        /// <param name="defaultValue">Value to set the default value to. Null is a valid value for this parameter.</param>
        void SetDefaultValue(V defaultValue);
    }
}
using System;
using System.Collections.Generic;

using PredicateMaps.Exceptions;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Interface which defines a class which maps predicates of type K against values of type V.
    /// Values can be tested against the predicate keys to find V type values which are associated with
    /// the predicate.
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates</typeparam>
    /// <typeparam name="V">Type of values to map to predicate keys</typeparam>
    public interface IPredicateMap<K, V>
    {
         /// <summary>
        /// Returns the number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        int GetCount();

        /// <summary>
        /// Add a key value pair to the map. The value can be retrieved later with values supplied to the Get methods in implementing 
        /// classes which return true for the key Predicate supplied.
        /// </summary>
        /// <param name="key">Predicate which can be used to return the value when it evaluates to true.</param>
        /// <param name="value">Value item to be stored.</param>
        void Add(Predicate<K> key, V value);

        /// <summary>
        /// Adds all elements in key and value lists to the map. The values from both lists are matched by index and must be provided in
        /// the correct order. If the size of the keyList and valueList do not match this method will throw an 
        /// InconsistentIndexException.
        /// 
        /// Null is not a valid value for either parameter
        /// </summary>
        /// <param name="keyList">Collection of predicates for use as keys</param>
        /// <param name="valueList">Collection of values for retreival when the key predicate evaluates to true.</param>
        /// /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        void AddAll(List<Predicate<K>> keyList, List<V> valueList);

        /// <summary>
        /// Method to find the value which is associated with the first predicate to resolve to true for the valueToTest parameter.
        /// 
        /// This method only finds the value for the first predicate which resolves to true with the value supplied through
        /// the valueToTest parameter. To find values associated with all predicates which resolve to true for the supplied
        /// parameter use GetAllMatches
        /// </summary>
        /// <param name="valueToTest">Value to test in key predicates</param>
        /// <returns>Value associated with first predicate found that is true for the valueToTest parameter</returns>
        V GetFirstMatch(K valueToTest);

        /// <summary>
        /// Finds all values where valueToTest evaluates the prdicate related to the value to true.
        /// 
        /// This method favours completeness over speed and will evaluate all predicates in the collection even
        /// if only one near the start evaluates to true. If you know that the Map contains only a single matching
        /// predicate for valueToTest use GetFirstMatch for performance. 
        /// </summary>
        /// <param name="valueToTest">Value to test against the predicate keys</param>
        /// <returns>A list of all matches or an empty list of no matches are found.</returns>
        List<V> GetAllMatches(K valueToTest);

        /// <summary>
        /// Counts the number of predicates in the map which evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to evaluate predicates with.</param>
        /// <returns>The number of predicates which are true for valueToTest</returns>
        int CountMatches(K valueToTest);

        /// <summary>
        /// Method to find if the map contains any matches for the value of valueToTest.
        /// </summary>
        /// <param name="valueToTest">Value to run in predicate keys</param>
        /// <returns>true when a predicate returns true for valueToTest, false otherwise.</returns>
        bool AnyMatches(K valueToTest);

        /// <summary>
        /// Returns a collection of all the indexes of entries in the map which evaluate to true for
        /// valueToTest.
        /// </summary>
        /// <param name="valueToTest">Value to test predicates against</param>
        /// <returns>Collection of all indexes where valueToTest evaluates predicates to true</returns>
        IEnumerable<int> GetIndexesOfMatches(K valueToTest);

        /// <summary>
        /// Removes the key/value pair at the given index in the dictionary.
        /// </summary>
        /// <param name="index">Int index of the key value pair in the map to remove</param>
        void RemoveKeyValuePairAtGivenIndex(int index);

        /// <summary>
        /// Replaces the value in the map with newValue for all predicates where valueToTest evaluates them
        /// to true
        /// </summary>
        /// <param name="valueToTest">Value to test predicates against</param>
        /// <param name="newValue">New value to insert into the map</param>
        void UpdateValueInMapForPredicate(K valueToTest, V newValue);

        /// <summary>
        /// Replace the value in the map with newMap at the given index.
        /// </summary>
        /// <param name="index">Index of the location in the map to replace the existing value with newValue</param>
        /// <param name="newValue">Value to place into the map at the index given</param>
        void UpdateValueAtIndex(int index, V newValue);
    }
}
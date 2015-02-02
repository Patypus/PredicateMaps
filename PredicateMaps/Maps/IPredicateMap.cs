﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Interface which defines a class which maps predicates of type K against values of type V.
    /// Values can be tested against the predicate keys to find V type values which are associated with
    /// the predicate.
    /// This map type allows for pattern matching.
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
        /// <param name="valuesList">Collection of values for retreival when the key predicate evaluates to true.</param>
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


    }
}

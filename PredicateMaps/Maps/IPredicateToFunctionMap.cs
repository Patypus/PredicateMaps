using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
    /// <summary>
    /// Interface which defines a class which maps predicates taking a parameter of type K to a function
    /// taking a a parameter of type K and returning a value of type V. The parameter K passed to the 
    /// predicate for test can, with implementation of this interface, be passed to the function for which
    /// the predicate is true for further manipulation or testing.
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates and for value functions to take as parameters</typeparam>
    /// <typeparam name="V">Return type for the value function to return</typeparam>
    public interface IPredicateToFunctionMap<K, V>
    {
        /// <summary>
        /// Retrieves the Predicates of type K which are the keys in the map.
        /// </summary>
        /// <returns>Collection of all keys that are in the map.</returns>
        IList<Predicate<K>> KeyPredicateList();

        /// <summary>
        /// Retrieves collection of value functions in the map
        /// </summary>
        /// <returns>All value functions which have been added to the map up to this point.</returns>
        IList<Func<K, V>> ValueFunctionList();

        /// <summary>
        /// Sets the default value that is returned when no matches for a value are found in the 
        /// ResolveFirstMatch method. Setting the default value via this method ovrerides any value that has
        /// previously been set.
        /// </summary>
        /// <param name="defaultValue">Value to set the default value to. Null is a valid value for this parameter.</param>
        void SetDefaultValue(V defaultValue);

        /// <summary>
        /// Gets the number of entries in this map.
        /// </summary>
        /// <returns>The number of key/value pairs in this map.</returns>
        int GetCount();

        /// <summary>
        /// Appends the provided key value pair to the map.
        /// </summary>
        /// <param name="key">Key predicate to add to the map</param>
        /// <param name="valueFunction">Value function to associate with the given key in the map</param>
        /// <exception cref="ArgumentException">Thrown if either parameter is null</exception>
        void Add(Predicate<K> key, Func<K, V> valueFunction);

        /// <summary>
        /// Adds all of the key value pairs from the provided dictionary to the map.
        /// </summary>
        /// <param name="keyValuesToAdd">Dictionary of key value pairs to add to the map</param>
        /// <exception cref="ArgumentException">Thrown if keyValueToAdd parameter is null.</exception>
        void AddAll(IDictionary<Predicate<K>, Func<K, V>> keyValuesToAdd);

        /// <summary>
        /// Adds all elements in key and value lists to the map, matching predicates to values in the provided collections
        /// by index which must be provided in the correct order. If the size of the keyList and valueList do not match this 
        /// method will throw an InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates of type K for use as keys</param>
        /// <param name="valueList">Collection of value function of parameter type K and return type V for use as values</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        void AddAll(IList<Predicate<K>> predicateListToAdd, IList<Func<K, V>> valueListToAdd);

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
        V GetFirstMatch(K valueToTest);

        /// <summary>
        /// Finds all functions associated with predicates which evaluate to true for valueToTest parameter and evaluates them. The
        /// results are returned in a collection.
        /// 
        /// An empty list is returned if no predicates evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to test with the predicate keys</param>
        /// <returns>A list of all values whose predicate key is true for valueToTest</returns>
        List<V> GetAllMatches(K valueToTest);

        /// <summary>
        /// Returns true if any predicates in the map evaluate to true for valueToTest, otherwise false.
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
    }
}
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
    }
}

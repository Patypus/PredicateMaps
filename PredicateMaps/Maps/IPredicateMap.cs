using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMaps.Maps
{
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

﻿using System;
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
        public IList<Predicate<K>> KeyPredicateList { get; private set; }
        public IList<Func<K, V>> ValueFunctionList { get; private set; }

        /// <summary>
        /// Initialises a new PredicateToFunctionMap with initialised but empty key and value lists.
        /// </summary>
        public PredicateToFunctionMap()
        {
            KeyPredicateList = new List<Predicate<K>>();
            ValueFunctionList = new List<Func<K, V>>();
        }
    }
}
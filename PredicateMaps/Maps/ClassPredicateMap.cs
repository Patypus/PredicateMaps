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
    /// Implementation of the IPredicateMap interface for values which are classes.
    /// Represents a type of map with keys of Predicates which take objects of type K which
    /// are mapped to values of type V, which is a class. Values are selected for return by this map
    /// where key predicates evaluate to true.
    /// </summary>
    /// <typeparam name="K">Type of items to test in the key predicates</typeparam>
    /// <typeparam name="V">Class type of values</typeparam>
    public class ClassPredicateMap<K, V> : IPredicateMap<K, V> where V : class
    {
        private readonly int NO_VALUE_FOUND = -1;
        public List<Predicate<K>> KeyPredicateList { get; private set; }
        public List<V> ValueItemList { get; private set; }

        /// <summary>
        /// Creates a new PredicateMap with empty key and value lists
        /// </summary>
        public ClassPredicateMap()
        {
            KeyPredicateList = new List<Predicate<K>>();
            ValueItemList = new List<V>();
        }

        /// <summary>
        /// Creates a PredicateMap which is has the contents of the keyList and valuesList params populated in it.
        /// The collections for keys and values must be of the same size.
        /// </summary>
        /// <param name="keyList">Collection of typed Predicates to use as the keys collection in the new map.</param>
        /// <param name="valuesList">Typed collection of values to include in the map.</param>
        /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and dataList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public ClassPredicateMap(List<Predicate<K>> keyList, List<V> valuesList)
        {
            CheckValidityOfMultipleAddParameters(keyList, valuesList);

            KeyPredicateList = keyList;
            ValueItemList = valuesList;
        }

        private void CheckValidityOfMultipleAddParameters(List<Predicate<K>> keyList, List<V> valueList)
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

        /// <summary>
        /// Returns the number of elements in the map.
        /// </summary>
        /// <returns>int number of items in the map</returns>
        public int GetCount()
        {
            return KeyPredicateList.Count;
        }

        /// <summary>
        /// Add a key value pair to the map. The value can be retrieved later with values supplied to the Get methods in this class
        /// which return true for the key Predicate supplied.
        /// </summary>
        /// <param name="key">Predicate which can be used to return the value when it evaluates to true.</param>
        /// <param name="value">Value item to be stored.</param>
        public void Add(Predicate<K> key, V value)
        {
            if (key == null || value == null) 
            {
                var message = key == null ? StringResources.InvalidKeyParameter
                                          : StringResources.InvalidDataParameter;
                throw new ArgumentException(message);
            }
            KeyPredicateList.Add(key);
            ValueItemList.Add(value);
        }

        /// <summary>
        /// Method to find the value which is associated with the first predicate to resolve to true for the valueToTest parameter.
        /// 
        /// This method only finds the value for the first predicate which resolves to true with the value supplied through
        /// the valueToTest parameter. To find values associated with all predicates which resolve to true for the supplied
        /// parameter use GetAllMatches.
        /// </summary>
        /// <param name="valueToTest">Value to test in key predicates</param>
        /// <returns>Value associated with first predicate found that is true for the valueToTest parameter</returns>
        public V GetFirstMatch(K valueToTest)
        {
            var indexForValue = GetIndexOfFirstMatch(valueToTest);
            return indexForValue != NO_VALUE_FOUND ? ValueItemList[indexForValue] : null;
        }

        private int GetIndexOfFirstMatch(K valueToTest)
        {
            foreach (var keyPred in KeyPredicateList) {
                var valueMatchesPredicate = keyPred.Invoke(valueToTest);
                if (valueMatchesPredicate)
                {
                    //Quick return for performence over completeness.
                    return KeyPredicateList.IndexOf(keyPred);
                }
            }
            //Return nothing found if no predicates evaluate to true for the given value
            return NO_VALUE_FOUND;
        }

        /// <summary>
        /// Adds all elements in key and value lists to the map. The values from both lists are matched by index and must be provided in
        /// the correct order. If the size of the keyList and valueList do not match this method will throw an 
        /// InconsistentIndexException. Null is not a valid value for either parameter for this method.
        /// </summary>
        /// <param name="keyList">Collection of predicates for use as keys</param>
        /// <param name="valuesList">Collection of values for retreival when the key predicate evaluates to true.</param>
        /// /// <exception cref="InconsistentIndexException">Thrown if the sizes of keyList and valueList are not equal.</exception>
        /// <exception cref="ArgumentException">Thrown if either parameter value is null.</exception>
        public void AddAll(List<Predicate<K>> keyList, List<V> valueList)
        {
            CheckValidityOfMultipleAddParameters(keyList, valueList);

            KeyPredicateList.AddRange(keyList);
            ValueItemList.AddRange(valueList);
        }

        /// <summary>
        /// Finds all values where valueToTest evaluates the prdicate related to the value to true.
        /// 
        /// This method favours completeness over speed and will evaluate all predicates in the collection even
        /// if only one near the start evaluates to true. If you know that the Map contains only a single matching
        /// predicate for valueToTest use GetFirstMatch for performance. 
        /// </summary>
        /// <param name="valueToTest">Value to test against the predicate keys</param>
        /// <returns>A list of all matches</returns>
        public List<V> GetAllMatches(K valueToTest)
        {
            var matchIndicies = GetIndexesOfMatchingPredicates(valueToTest);
            var matchValues = matchIndicies.Select(index => ValueItemList[index]).ToList();
            
            return matchValues;
        }

        /// <summary>
        /// Counts the number of predicates in the map which evaluate to true for valueToTest
        /// </summary>
        /// <param name="valueToTest">Value to evaluate predicates with.</param>
        /// <returns>The number of predicates which are true for valueToTest</returns>
        public int CountMatches(K valueToTest)
        {
            return KeyPredicateList.AsParallel().Where(pred => pred.Invoke(valueToTest) == true).Count();
        }

        /// <summary>
        /// Returns a collection of all the indexes of entries in the map which evaluate to true for
        /// valueToTest.
        /// </summary>
        /// <param name="valueToTest">Value to test predicates against</param>
        /// <returns>Collection of all indexes where valueToTest evaluates predicates to true</returns>
        public IEnumerable<int> GetIndexesOfMatches(K valueToTest)
        {
            return GetIndexesOfMatchingPredicates(valueToTest);
        }

        /// <summary>
        /// Removes the key/value pair at the given index in the dictionary.
        /// </summary>
        /// <param name="index">Int index of the key value pair in the map to remove</param>
        public void RemoveKeyValuePairAtGivenIndex(int index)
        {
            KeyPredicateList.RemoveAt(index);
            ValueItemList.RemoveAt(index);
        }

        private IEnumerable<int> GetIndexesOfMatchingPredicates(K valueToTest)
        {
            var matchItems = KeyPredicateList.AsParallel().Where(pred => pred.Invoke(valueToTest) == true).ToList();
            var matchIndicies = matchItems.Select(match => KeyPredicateList.IndexOf(match));

            return matchIndicies;
        }
    }
}
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PredicateMaps.Maps;
using PredicateMaps.Exceptions;
using PredicateMaps.Resources;

namespace PredicateMapsTests.Maps
{
    [TestFixture]
    public class ClassPredicateMapTests
    {
        [Test]
        public void DefaultConstructorCreatesEmptyMap()
        {
            var basicMap = new ClassPredicateMap<string,string>();
            var itemsInNewMap = basicMap.GetCount();
            Assert.AreEqual(0, itemsInNewMap);
        }

        [Test]
        public void CountReturnsSizeOfMap()
        {
            var keys = new List<Predicate<int>> { (i) => i > 1, (i) => i == 1, (i) => 1 < 1 };
            var data = new List<string> { "More than 1", "Exactly 1", "Less than 1" };

            var map = new ClassPredicateMap<int, string>(keys, data);

            Assert.AreEqual(3, map.GetCount());
        }

        [Test]
        public void ParameteredConstructorCreatesMapWithSuppliedCollectionsIn()
        {
            var dataItem = "Hello to you too.";
            var keyList = new List<Predicate<string>> { (string s) => s.Contains("Hello")  };
            var dataList = new List<string> { dataItem };
           
            var populatedMap = new ClassPredicateMap<string, string>(keyList, dataList);
            var keysFromMap = populatedMap.KeyPredicateList;

            Assert.IsTrue(populatedMap.ValueItemList.Contains(dataItem));
            //Harder to test for the key as the original predicate can't be checked for equality directly.
            //Sticking with checking the number in the collection and it returns true when it should as a check for equality.
            //This is a basic test for coverage rather than checking something complex.
            Assert.AreEqual(1, keysFromMap.Count);
            Assert.IsTrue(keysFromMap.First().Invoke("This contains Hello"));
        }

        [Test]
        [ExpectedException(typeof(InconsistentIndexException))]
        public void ParameteredConstructorThrowsInconsistentIndexExceptionWhenIndexesOfSuppliedCollectionsDontMatch()
        {
            var keys = new List<Predicate<string>>();
            var data = new List<string> { "These list sizes dont match..." };

            new ClassPredicateMap<string, string>(keys, data);
        }

        [Test]
        public void InconsistentIndexExceptionMessageContainsSizeOfProvidedLists()
        {
            Exception caughtException = null;
            var keys = new List<Predicate<int>> { (i) => i == 0 };
            var data = new List<string> { "These list sizes dont match...", "No indeed" };

            try
            {
                new ClassPredicateMap<int, string>(keys, data);
            }
            catch (InconsistentIndexException iie)
            {
                caughtException = iie;
            }
            Assert.NotNull(caughtException);
            Assert.True(caughtException.Message.Contains(keys.Count.ToString()));
            Assert.True(caughtException.Message.Contains(data.Count.ToString()));
        }

        [Test]
        public void NullKeyCollectionCausesInvalidArgumentException()
        {
            ArgumentException caughtException = null;

            try
            {
                new ClassPredicateMap<string, string>(null, new List<string>());
            }
            catch (ArgumentException ae) 
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidKeyCollectionParameter, caughtException.Message);
        }

        [Test]
        public void DataCollectionCausesInvalidArgumentException()
        {
            ArgumentException caughtException = null;

            try
            {
                new ClassPredicateMap<string, string>(new List<Predicate<string>>(), null);
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidDataCollectionParameter, caughtException.Message);
        }

        [Test]
        public void AddPutsKeyAndDataItemInMap()
        {
            var map = new ClassPredicateMap<int, string>();
            
            var mapSizeBeforeAdd = map.GetCount();
            map.Add((i) => i < 5, "Value was little.");
            var mapSizeAfterAdd = map.GetCount();

            Assert.True(mapSizeBeforeAdd < mapSizeAfterAdd);
            Assert.AreEqual(1, mapSizeAfterAdd - mapSizeBeforeAdd);
        }

        [Test]
        public void NullKeyGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidKeyParameter;
            Exception caughtException = null;

            try
            {
                var map = new ClassPredicateMap<string, string>();
                map.Add(null, "someValue");
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(expectedMessage, caughtException.Message);
        }

        [Test]
        public void NullValueGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidDataParameter;
            Exception caughtException = null;

            try
            {
                var map = new ClassPredicateMap<string, string>();
                map.Add((s) => s.Equals("someKey"), null);
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(expectedMessage, caughtException.Message);
        }
        
        [Test]
        public void GetFirstMatch_ReturnsCorrectItemForKey()
        {
            var predicateList = new List<Predicate<int>> { (i) => i == 5 };
            var value = "This should get returned.";
            var valueList = new List<string> { value };
            
            var predicateMap = new ClassPredicateMap<int, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(5);

            Assert.AreEqual(value, returnedValue);
        }
        
        [Test]
        public void GetFirstMatch_RetrunsNullWhenNoMatchesExists()
        {
            var predicateList = new List<Predicate<int>> { (i) => i == 5, (i) => i == 3 };
            var valueList = new List<string> { "This should NOT get returned.", "Neither should this" };

            var predicateMap = new ClassPredicateMap<int, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(4);

            Assert.Null(returnedValue);
        }

        [Test]
        public void GetFirstMatch_AcceptsNullAsParameter()
        {
            var predicateList = new List<Predicate<string>> { (s) => s == null, (s) => s.Length > 10 };
            var value = "This will be matched for null";
            var valueList = new List<string> { value, "value for longer strings" };

            var predicateMap = new ClassPredicateMap<string, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(null);

            Assert.AreEqual(value, returnedValue);
        }

        [Test]
        public void AddAll_AddsAllItemsInCollectionToMap()
        {
            var predicates = new List<Predicate<double>> { (d) => d < 2.0, (d) => d == null, (d) => d > 100.0 };
            var values = new List<string> { "Small double", "That's null", "Large double." };
            var map = new ClassPredicateMap<double, string>();
            map.AddAll(predicates, values);

            Assert.True(map.GetCount() == predicates.Count && map.GetCount() == values.Count);
        }

        [Test]
        [ExpectedException(typeof(InconsistentIndexException))]
        public void AddAll_ThrowsInconsistentIndexExceptionWhenSizesOfParameterCollectionsDoNoMatch()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 0 };
            var values = new List<string> { "0", "4", "4", "E", "6" };
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(predicates, values);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenKeyCollectionIsNull()
        {
            var valuesCollection = new List<string>();
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(null, valuesCollection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenValueCollectionIsNull()
        {
            var predicatesCollection = new List<Predicate<string>>();
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(predicatesCollection, null);
        }

        //append to existing items
        //null key collection
        //null value collection
        //
        //
        //addAll?
        //getAll?
        //update?
        //remove?
    }
}
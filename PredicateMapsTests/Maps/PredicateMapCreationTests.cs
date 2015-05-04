using NUnit.Framework;
using PredicateMaps.Exceptions;
using PredicateMaps.Maps;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PredicateMapsTests.Maps
{
    /// <summary>
    /// Unit test class for testing the constructors of the ClassPredicateMap. These tests are separate from the rest of the
    /// functional test for ClassPredicateMap to keep construction and functionality testing separated as test classes were
    /// getting crowded.
    /// </summary>
    [TestFixture]
    public class PredicateMapCreationTests
    {
        [Test]
        public void DefaultConstructorCreatesEmptyMap()
        {
            var basicMap = new PredicateMap<string, string>("default value");
            var itemsInNewMap = basicMap.GetCount();
            Assert.AreEqual(0, itemsInNewMap);
        }

        [Test]
        public void ParameteredConstructorCreatesMapWithSuppliedCollectionsIn()
        {
            var dataItem = "Hello to you too.";
            var keyList = new List<Predicate<string>> { (s) => s.Contains("Hello") };
            var dataList = new List<string> { dataItem };

            var populatedMap = new PredicateMap<string, string>(keyList, dataList, string.Empty);
            var keysFromMap = populatedMap.KeyPredicateList();

            Assert.IsTrue(populatedMap.ValueItemList().Contains(dataItem));
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

            new PredicateMap<string, string>(keys, data, "Nothing found");
        }

        [Test]
        public void InconsistentIndexExceptionMessageContainsSizeOfProvidedLists()
        {
            Exception caughtException = null;
            var keys = new List<Predicate<int>> { (i) => i == 0 };
            var data = new List<string> { "These list sizes dont match...", "No indeed" };

            try
            {
                new PredicateMap<int, string>(keys, data, string.Empty);
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
                new PredicateMap<string, string>(null, new List<string>(), "Defaults");
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidKeyCollectionParameter, caughtException.Message);
        }

        [Test]
        public void NullDataCollectionCausesInvalidArgumentException()
        {
            ArgumentException caughtException = null;

            try
            {
                new PredicateMap<string, string>(new List<Predicate<string>>(), null, string.Empty);
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidDataCollectionParameter, caughtException.Message);
        }

        [Test]
        public void DictionaryConstricutorPopulatesInternalDictionaryWithAllValuesFromParameter()
        {
            var initialData = new Dictionary<Predicate<string>, string>
            {
                { (s) => string.IsNullOrEmpty(s), "invalid string" },
                { (s) => s.Length > 150, "Long string" },
                { (s) => s.Contains("hello"), "welcoming string" },
            };
            var map = new PredicateMap<string, string>(initialData, "No matches found");

            CollectionAssert.AreEquivalent(initialData.Keys, map.KeyPredicateList());
            CollectionAssert.AreEquivalent(initialData.Values, map.ValueItemList());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void NullDictionaryForConstructorCausesArgumentException()
        {
            new PredicateMap<int, string>(null, "Default value");
        }

        [Test]
        public void SingleParameterConstructorSetsDafaultValue()
        {
            var defaultValue = "This is the default value.";
            var map = new PredicateMap<int, string>(defaultValue);

            var result = map.GetFirstMatch(1);
            Assert.AreEqual(defaultValue, result);
        }

        [Test]
        public void DictionaryParameterConstructorSetsDafaultValue()
        {
            var defaultValue = "Expected default";
            var dictionary = new Dictionary<Predicate<Type>, string>();
            var map = new PredicateMap<Type, string>(dictionary, defaultValue);

            var result = map.GetFirstMatch(typeof(ArgumentException));
            Assert.AreEqual(defaultValue, result);
        }

        [Test]
        public void ListParameterConstructorSetsDafaultValue()
        {
            var defaultValue = "Get this when nothing matchers";
            var keyList = new List<Predicate<string>>();
            var valueList = new List<string>();
            var map = new PredicateMap<string, string>(keyList, valueList, defaultValue);

            var result = map.GetFirstMatch("Find me a match!");
            Assert.AreEqual(defaultValue, result);
        }
    }
}

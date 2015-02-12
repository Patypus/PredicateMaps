using NUnit.Framework;
using PredicateMaps.Exceptions;
using PredicateMaps.Maps;
using PredicateMaps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMapsTests.Maps
{
    /// <summary>
    /// Unit test class for testing the constructors of the ClassPredicateMap. These tests are separate from the rest of the
    /// functional test for ClassPredicateMap to keep construction and functionality testing separated as test classes were
    /// getting crowded.
    /// </summary>
    [TestFixture]
    public class ClassPredicateMapCreationTests
    {
        [Test]
        public void DefaultConstructorCreatesEmptyMap()
        {
            var basicMap = new ClassPredicateMap<string, string>();
            var itemsInNewMap = basicMap.GetCount();
            Assert.AreEqual(0, itemsInNewMap);
        }

        [Test]
        public void ParameteredConstructorCreatesMapWithSuppliedCollectionsIn()
        {
            var dataItem = "Hello to you too.";
            var keyList = new List<Predicate<string>> { (string s) => s.Contains("Hello") };
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
    }
}

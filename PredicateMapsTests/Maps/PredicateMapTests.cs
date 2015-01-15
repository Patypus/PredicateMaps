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
    public class PredicateMapTests
    {
        [Test]
        public void DefaultConstructorCreatesEmptyMap()
        {
            var basicMap = new PredicateMap<string,string>();
            var itemsInNewMap = basicMap.GetCount();
            Assert.AreEqual(0, itemsInNewMap);
        }

        [Test]
        public void CountReturnsSizeOfMap()
        {
            var keys = new List<Predicate<int>> { (i) => i > 1, (i) => i == 1, (i) => 1 < 1 };
            var data = new List<string> { "More than 1", "Exactly 1", "Less than 1" };

            var map = new PredicateMap<int, string>(keys, data);

            Assert.AreEqual(3, map.GetCount());
        }

        [Test]
        public void ParameteredConstructorCreatesMapWithSuppliedCollectionsIn()
        {
            var dataItem = "Hello to you too.";
            var keyList = new List<Predicate<string>> { (string s) => s.Contains("Hello")  };
            var dataList = new List<string> { dataItem };
           
            var populatedMap = new PredicateMap<string, string>(keyList, dataList);
            var keysFromMap = populatedMap.keyPredicateList;

            Assert.IsTrue(populatedMap.valueList.Contains(dataItem));
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

            new PredicateMap<string, string>(keys, data);
        }

        [Test]
        public void InconsistentIndexExceptionMessageContainsSizeOfProvidedLists()
        {
            Exception caughtException = null;
            var keys = new List<Predicate<int>> { (i) => i == 0 };
            var data = new List<string> { "These list sizes dont match...", "No indeed" };

            try
            {
                new PredicateMap<int, string>(keys, data);
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
        public void nullKeyCollectionCausesInvalidArgumentException()
        {
            ArgumentException caughtException = null;

            try
            {
                new PredicateMap<string, string>(null, new List<string>());
            }
            catch (ArgumentException ae) 
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidKeyCollectionParameter, caughtException.Message);
        }

        [Test]
        public void dataCollectionCausesInvalidArgumentException()
        {
            ArgumentException caughtException = null;

            try
            {
                new PredicateMap<string, string>(new List<Predicate<string>>(), null);
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(StringResources.InvalidDataCollectionParameter, caughtException.Message);
        }

        [Test]
        public void addPutsKeyAndDataItemInMap()
        {
            var map = new PredicateMap<int, string>();
            
            var mapSizeBeforeAdd = map.GetCount();
            map.Add((i) => i < 5, "Value was little.");
            var mapSizeAfterAdd = map.GetCount();

            Assert.True(mapSizeBeforeAdd < mapSizeAfterAdd);
            Assert.AreEqual(1, mapSizeAfterAdd - mapSizeBeforeAdd);
        }

        [Test]
        public void nullKeyGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidKeyParameter;
            Exception caughtException = null;

            try
            {
                var map = new PredicateMap<string, string>();
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
        public void nullValueGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidDataParameter;
            Exception caughtException = null;

            try
            {
                var map = new PredicateMap<string, string>();
                map.Add((s) => s.Equals("someKey"), null);
            }
            catch (ArgumentException ae)
            {
                caughtException = ae;
            }

            Assert.NotNull(caughtException);
            Assert.AreEqual(expectedMessage, caughtException.Message);
        }

        //add? -adds -adds at next index -null not accepted for either param...
        //getFirst?
        //addAll?
        //getAll?
        //update?
        //remove?
    }
}
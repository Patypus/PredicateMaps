using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using PredicateMaps.Maps;
using PredicateMaps.Exceptions;
using PredicateMaps.Resources;

namespace PredicateMapsTests.Maps
{
    [TestFixture]
    public class PredicateMapTests
    {
        [Test]
        public void CountReturnsSizeOfMap()
        {
            var mapping = new Dictionary<Predicate<int>, string>
            {
                { (i) => i > 1, "More than 1" },
                { (i) => i == 1, "Exactly 1" },
                { (i) => 1 < 1, "Less than 1" }
            };
            var map = new PredicateMap<int, string>(mapping, string.Empty);

            Assert.AreEqual(3, map.GetCount());
        }

        [Test]
        public void AddPutsKeyAndDataItemInMap()
        {
            var map = new PredicateMap<int, string>("Default");
            
            var mapSizeBeforeAdd = map.GetCount();
            map.Add((i) => i < 5, "Value was little.");
            var mapSizeAfterAdd = map.GetCount();

            Assert.True(mapSizeBeforeAdd < mapSizeAfterAdd);
            Assert.AreEqual(1, mapSizeAfterAdd - mapSizeBeforeAdd);
        }

        [Test]
        public void NullKeyForAddGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidKeyParameter;
            Exception caughtException = null;

            try
            {
                var map = new PredicateMap<string, string>("default value");
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
        public void NullValueForAddGivesArgumentException()
        {
            var expectedMessage = StringResources.InvalidDataParameter;
            Exception caughtException = null;

            try
            {
                var map = new PredicateMap<string, string>(string.Empty);
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
            var value = "This should get returned.";
            var mapping = new Dictionary<Predicate<int>, string> { { (i) => i == 5, value } };
            
            var predicateMap = new PredicateMap<int, string>(mapping, "Default should not be needed.");
            var returnedValue = predicateMap.GetFirstMatch(5);

            Assert.AreEqual(value, returnedValue);
        }
        
        [Test]
        public void GetFirstMatch_RetrunsDefaultValueWhenNoMatchesExists()
        {
            var defaultValue = "No matches found";
            var mapping = new Dictionary<Predicate<int>, string>
            {
                { (i) => i == 5, "This should NOT get returned." },
                { (i) => i == 3, "Neither should this" }
            };

            var predicateMap = new PredicateMap<int, string>(mapping, defaultValue);
            var returnedValue = predicateMap.GetFirstMatch(4);

            Assert.AreEqual(defaultValue, returnedValue);
        }

        [Test]
        public void GetFirstMatch_AcceptsNullAsParameter()
        {
            var predicateList = new List<Predicate<string>> { (s) => s == null, (s) => s.Length > 10 };
            var value = "This will be matched for null";
            var valueList = new List<string> { value, "value for longer strings" };

            var predicateMap = new PredicateMap<string, string>(predicateList, valueList, string.Empty);
            var returnedValue = predicateMap.GetFirstMatch(null);

            Assert.AreEqual(value, returnedValue);
        }

        [Test]
        public void AddAll_AddsAllItemsInCollectionToMap()
        {
            var predicates = new List<Predicate<double>> { (d) => d < 2.0, (d) => d < 0.0, (d) => d > 100.0 };
            var values = new List<string> { "Small double", "That's negative", "Large double." };
            var map = new PredicateMap<double, string>("Default value");
            map.AddAll(predicates, values);

            Assert.True(map.GetCount() == predicates.Count && map.GetCount() == values.Count);
        }

        [Test]
        [ExpectedException(typeof(InconsistentIndexException))]
        public void AddAll_ThrowsInconsistentIndexExceptionWhenSizesOfParameterCollectionsDoNoMatch()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 0 };
            var values = new List<string> { "0", "4", "4", "E", "6" };
            var map = new PredicateMap<string, string>(string.Empty);

            map.AddAll(predicates, values);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenKeyCollectionIsNull()
        {
            var valuesCollection = new List<string>();
            var map = new PredicateMap<string, string>(string.Empty);

            map.AddAll(null, valuesCollection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenValueCollectionIsNull()
        {
            var predicatesCollection = new List<Predicate<string>>();
            var map = new PredicateMap<string, string>(string.Empty);

            map.AddAll(predicatesCollection, null);
        }

        [Test]
        public void GetAllMatches_RetrunsAllMatches()
        {
            var predicates = new List<Predicate<int>> { (i) => i == 4, (i) => i < 5, (i) => i > 10 };
            var values = new List<string> { "It's 4", "Less than 5", "Over 10" };

            var map = new PredicateMap<int, string>(predicates, values, "default");
            var result = map.GetAllMatches(4);

            var expected = new List<string> { "It's 4", "Less than 5" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void GetAllMatches_ReturnsEmptyCollectionWhenNoMatchesFound()
        {
            var predicates = new List<Predicate<double>> { (d) => d < 1.0 };
            var values = new List<string> { "This is not the match you are looking for" };
            var map = new PredicateMap<double, string>(predicates, values, "Default value not needed for GetAllMatches");

            var result = map.GetAllMatches(3.7);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetAllMatches_ReturnsCorrectValuesWhenTwoKeyPredicatesAreIdentical()
        {
            var mapping = new Dictionary<Predicate<string>, string>
            {
                { (s) => s.Length == 1, "match one" },
                { (s) => s == "other", "No Match" },
                { (s) => s.Length == 1, "match two" }
            };
            var map = new PredicateMap<string, string>(mapping, "No default needed");

            var expected = new List<string> { "match one", "match two" };
            var result = map.GetAllMatches("s");
            
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void CountMatches_ReturnsZeroWhenMapIsEmpty()
        {
            var map = new PredicateMap<string, string>("Not used here");
            var matches = map.CountMatches("TestValue");
            Assert.AreEqual(0, matches);
        }

        [Test]
        public void CountMatches_ReturnsZeroWhenMapHasNoMatches()
        {
            var mapping = new Dictionary<Predicate<Type>, string>
            {
                { (t) => t == typeof(ArgumentException), "That was invalid" },
                { (t) => t == typeof(NullReferenceException), "Something was null" },
                { (t) => t  == typeof(StackOverflowException), "Stack has overflown" },
            };
            var map = new PredicateMap<Type, string>(mapping, "No matches found");
            var matches = map.CountMatches(("This is not an exception").GetType());
            Assert.AreEqual(0, matches);
        }

        [Test]
        public void CountMatches_ReturnsCountForMatchesWhenItemsInMapAreTrueForParameter()
        {
            var predicates = new List<Predicate<int>> { (i) => i < 9, (i) => i == 5, (i) => i > 20 };
            var values = new List<string> { "under 9", "five", "large number" };
            var map = new PredicateMap<int, string>(predicates, values, string.Empty);
            var matches = map.CountMatches(5);
            Assert.AreEqual(2, matches);
        }

        [Test]
        public void AnyMatches_ReturnsFalseWhenNothingInTheMapMatches()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 3, (s) => s == null };
            var values = new List<string> { "Three", "null" };
            var map = new PredicateMap<string, string>(predicates, values, string.Empty);

            Assert.False(map.AnyMatches("This string matches nothing."));
        }

        [Test]
        public void AnyMatches_ReturnsTrueWhenSomethingInMapIsTrueForValue()
        {
            var mapping = new Dictionary<Predicate<string>, string>
            {
                { (s) => s.Length > 10, "over 10" },
                { (s) => s != null, "not null" },
                { (s) => s.Contains("fred"), "Has a FRED" }
            };
            var map = new PredicateMap<string, string>(mapping, string.Empty);

            Assert.True(map.AnyMatches("This string matches for Most"));
        }

        [Test]
        public void SetDefaultValue_UpdatesMapDefaultValue()
        {
            var initialDefaultValue = "First version";
            var map = new PredicateMap<string, string>(initialDefaultValue);
            var secondDefaultValue = "Second version";

            var beforeUpdate = map.GetFirstMatch("bring me the default");
            map.SetDefaultValue(secondDefaultValue);
            var afterUpdate = map.GetFirstMatch("bring me the default again");

            Assert.AreNotEqual(beforeUpdate, afterUpdate);
            Assert.AreEqual(initialDefaultValue, beforeUpdate);
            Assert.AreEqual(secondDefaultValue, afterUpdate);
        }
    }
}
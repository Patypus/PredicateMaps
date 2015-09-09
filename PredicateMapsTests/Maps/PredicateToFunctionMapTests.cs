using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PredicateMaps.Maps;
using PredicateMaps.Exceptions;

namespace PredicateMapsTests.Maps
{
    [TestFixture]
    public class PredicateToFunctionMapTests
    {
        [Test]
        public void GetCount_ReturnsTheCorrectNumberOfItemsInTheMap()
        {
            var testData = new Dictionary<Predicate<int>, Func<int, string>>
            {
                { (i) => i > 100, (i) => string.Format("The number {0} is pretty large.", i) },
                { (i) => i == 0, (i) => "i is exactly zero" },
                { (i) => i < 0, (i) => "number is negative by " + (0 + i) }
            };

            var map = new PredicateToFunctionMap<int, string>(testData, "default value");
            var count = map.GetCount();

            Assert.AreEqual(testData.Count, count);
        }

        [Test]
        public void Add_AddsGivenKeyValuePairToMap()
        {
            Predicate<Type> predicateKey = (t) => t.GetType() == typeof(PredicateMap<Type, string>);
            Func<Type, string> functionValue = (t) => "Another map of the same type!";

            var map = new PredicateToFunctionMap<Type, string>("No value found for given type");
            map.Add(predicateKey, functionValue);

            Assert.True(map.KeyPredicateList().Contains(predicateKey));
            Assert.True(map.ValueFunctionList().Contains(functionValue));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Add_NullKeyValueThrowsArgumentException()
        {
            var map = new PredicateToFunctionMap<Exception, string>("default value irrelevant to this test.");
            map.Add(null, (e) => "value with no key.");
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Add_NullFunctionValueThrowsArgumentException()
        {
            var map = new PredicateToFunctionMap<object, string>("default value irrelevant to this test.");
            map.Add((o) => o != null, null);
        }

        [Test]
        public void DictionaryAddAll_AddsAllElementsFromDictionaryParameter()
        {
            var toAdd = new Dictionary<Predicate<Exception>, Func<Exception, string>>
            {
                { (e) => e.GetType().IsAssignableFrom(typeof(NullReferenceException)), (e) => "Something was null" },
                { (e) => e is ArgumentException, (e) => "A parameter was invalid: " + e.Message },
                { (e) => e.InnerException != null, (e) => String.Join(" ", "concatenated message:", e.Message, e.InnerException.Message)  },
            };

            var map = new PredicateToFunctionMap<Exception, string>("Default value");
            map.AddAll(toAdd);

            CollectionAssert.AreEquivalent(toAdd.Keys, map.KeyPredicateList());
            CollectionAssert.AreEquivalent(toAdd.Values, map.ValueFunctionList());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void DictionaryAddAll_ThrowsArgumentExceptionWhenDictionaryIsNull()
        {
            var map = new PredicateToFunctionMap<Exception, string>("Default value not related to this test");
            map.AddAll(null);
        }

        [Test]
        public void ListAddAll_AddsAllElementsToMap()
        {
            var keys = new List<Predicate<string>>
            {
                s => s.Contains("hello"), 
                s => s.Length > 500,
                s => s.StartsWith("Warning")
            };
            var values = new List<Func<string, string>>
            {
                s => "Greeting message: " + s,
                s => "Long message",
                s => "Something has gone wrong: " + s
            };
            var map = new PredicateToFunctionMap<string, string>("not needed");
            map.AddAll(keys, values);
            
            Assert.AreEqual(keys, map.KeyPredicateList());
            Assert.AreEqual(values, map.ValueFunctionList());
        }

        [Test, ExpectedException(typeof(InconsistentIndexException))]
        public void ListAddAll_InconsistentIndexExceptionThrownWhenSizesOfKeyAndValueListsDoNotMatch()
        {
            var map = new PredicateToFunctionMap<string, string>("unrelated to test");
            var keysCollection = new List<Predicate<string>> { (s) => s.Contains("some text") };
            map.AddAll(keysCollection, new List<Func<string, string>>());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ListAddAll_ArgumentExceptionThrownForNullKeyPredicateCollection()
        {
            var map = new PredicateToFunctionMap<Type, Exception>(new NullReferenceException("no result found"));
            map.AddAll(null, new List<Func<Type, Exception>>());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ListAddAll_ArgumentExceptionThrownForNullValuePredicateCollection()
        {
            var map = new PredicateToFunctionMap<Type, string>("Value not related to test");
            map.AddAll(new List<Predicate<Type>>(), null);
        }

        [Test]
        public void SetDefaultValue_UpdatesDefaultValueInMap()
        {
            var firstDefaultValue = "Initial value";
            var updatedValue = "Updated value";

            var map = new PredicateToFunctionMap<int, string>(firstDefaultValue);
            map.SetDefaultValue(updatedValue);
            var result = map.GetFirstMatch(-1);

            Assert.AreNotEqual(firstDefaultValue, result);
            Assert.AreEqual(updatedValue, result);
        }

        [Test]
        public void GetFirstMatch_ReturnsCorrectValueForKey()
        {
            var expectedValue = "This value should be returned.";
            var defaultValue = "default value not needed for test.";
            var data = new Dictionary<Predicate<Type>, Func<Type, string>>
            {
                { (t) => t.IsAssignableFrom(typeof(Exception)), (t) => expectedValue },
                { (t) => t.Namespace.Contains("PredicateMaps"), (t) => "Type is in one of our namespaces" }
            };
            var map = new PredicateToFunctionMap<Type, string>(data, defaultValue);

            var result = map.GetFirstMatch(typeof(Exception));
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetFirstMatch_ReturnsDefaultValueWhenNoMatchesExistForPassedKey()
        {
            var defaultValue = "Expected default value";
            var map = new PredicateToFunctionMap<int, string>(defaultValue);

            var result = map.GetFirstMatch(14);

            Assert.AreEqual(result, defaultValue);
        }

        [Test]
        public void GetAllMatches_ReturnsEmptyCollectionWhenNoMatchesExist()
        {
            var map = new PredicateToFunctionMap<int, string>("Default value not relevant.");
            var results = map.GetAllMatches(-1);

            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void GetAllMatches_ReturnsAllExpectedMatchesInCollection()
        {
            var data = new Dictionary<Predicate<int>, Func<int, string>>
            {
                { (i) => i > 0, (i) => string.Format("{0} is greater than 0", i) },
                { (i) => i < 5, (i) => string.Format("{0} is less than 5", i) },
                { (i) => i == 2000, (i) => "value is oddly out the area of the other values"}
            };
            var map = new PredicateToFunctionMap<int, string>(data, "default value unused in this test");

            var results = map.GetAllMatches(4);

            var expected = new List<string> { "4 is greater than 0", "4 is less than 5" };
            Assert.AreEqual(2, results.Count);
            CollectionAssert.AreEquivalent(expected, results);
        }

        [Test]
        public void AnyMatches_ReturnsFalseWhenNoMatchesExistForValueToTest()
        {
            var map = new PredicateToFunctionMap<string, List<string>>(new List<string>());

            var matches = map.AnyMatches("Are there any matches?");
            Assert.IsFalse(matches);
        }

        [Test]
        public void AnyMatches_ReturnsTrueWhenMatchesExistForValueToTest()
        {
            var data = new Dictionary<Predicate<string>, Func<string, string>> 
            {
                { (s) => s.Length > 0, (s) => "value is populated" }
            };
            var map = new PredicateToFunctionMap<string, string>(data, "Unused default value");

            var result = map.AnyMatches("a new value");
            Assert.IsTrue(result);
        }

        [Test]
        public void CountMatches_ReturnsZeroForNewlyCreatedMaps()
        {
            var map = new PredicateMap<string, string>("value irrelevant to test");

            var matches = map.CountMatches("Any value");
            Assert.AreEqual(0, 0);
        }

        [Test]
        public void CountMatches_ReturnsCorrectNumberOfMatchesForPopulatedMaps()
        {
            var data = new Dictionary<Predicate<int>, Func<int, string>>
            {
                { (i) => i == 0, (i) => "Value is 0" },
                { (i) => i > -1 && i <= 7, (i) => string.Format("value is: {0}", i) },
                { (i) => i > 8, (i) => "Value is out of range."}
            };
            var map = new PredicateToFunctionMap<int, string>(data, string.Empty);

            var matches = map.CountMatches(0);
            Assert.AreEqual(2, matches);
        }
    }
}
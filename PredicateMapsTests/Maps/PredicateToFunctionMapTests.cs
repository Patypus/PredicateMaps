using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PredicateMaps.Maps;

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
    }
}
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
    }
}

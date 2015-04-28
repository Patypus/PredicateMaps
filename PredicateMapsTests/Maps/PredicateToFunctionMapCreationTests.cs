using NUnit.Framework;
using PredicateMaps.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateMapsTests.Maps
{
    [TestFixture]
    public class PredicateToFunctionMapCreationTests
    {
        [Test]
        public void ParameterlessConstructor_InitialisesEmptyKeyAndValueLists()
        {
            var emptyMap = new PredicateToFunctionMap<int, string>();

            Assert.AreEqual(0, emptyMap.KeyPredicateList.Count);
            Assert.AreEqual(0, emptyMap.ValueFunctionList.Count);
        }
    }
}

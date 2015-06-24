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

            Assert.AreEqual(0, emptyMap.KeyPredicateList().Count);
            Assert.AreEqual(0, emptyMap.ValueFunctionList().Count);
        }

        [Test]
        public void DictionaryConstructor_InitialisesMapWithKeysAndValuesInIt()
        {
            var testData = new Dictionary<Predicate<string>, Func<string, string>>
            {
                { (s) => s.Contains("hello"), (s) => string.Join(" ", s, " contains hello.") },
                { (s) => s.Length > 50, (s) => s + " is pretty long." }
            };
            var map = new PredicateToFunctionMap<string, string>(testData);

            Assert.AreEqual(testData.Keys, map.KeyPredicateList());
            Assert.AreEqual(testData.Values, map.ValueFunctionList());
        }

        [Test, ExpectedException]
        public void DictionaryConstructor_NullDictionayCausesArgumentException()
        {
            new PredicateToFunctionMap<int, string>(null);
        }

        [Test]
        public void DictionaryConstructor_MessageInExceptionForNullConstructorContainsNameOfClass()
        {
            Exception exception = null;
            try
            {
                new PredicateToFunctionMap<int, string>(null);
            }
            catch (Exception e)
            {
                exception = e;
            }
            Assert.NotNull(exception);
            Assert.True(exception.Message.Contains(typeof(PredicateToFunctionMap<int, string>).Name));
        }
    }
}

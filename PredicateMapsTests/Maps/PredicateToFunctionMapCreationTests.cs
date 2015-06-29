using NUnit.Framework;
using PredicateMaps.Exceptions;
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

        [Test]
        public void ListConstructor_InitialisesMapWithAllKeysListsIn()
        {
            var keysList = new List<Predicate<Type>> {
                (t) => t.IsAssignableFrom(typeof(Exception)),
                (t) => t.Name == "PredicateToFunctionMapCreationTests",
                (t) => !t.Namespace.Contains("PredicateMaps")
            };
            var valuesList = new List<Func<Type, string>>()
            {
                (t) => "Something went wrong at some point",
                (t) => "Your are in the type already",
                (t) => "It is in our namespace, whole space: " + t.Namespace
 
            };

            var map = new PredicateToFunctionMap<Type, string>(keysList, valuesList);

            CollectionAssert.AreEqual(keysList, map.KeyPredicateList());
            CollectionAssert.AreEqual(valuesList, map.ValueFunctionList());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ListConstructor_ArgumentExceptionThrownForNullKeyCollection()
        {
            var valuesList = new List<Func<Type, string>>()
            {
                (t) => "Something went wrong at some point",
                (t) => "Your are in the type already",
                (t) => "It is in our namespace, whole space: " + t.Namespace
 
            };

            new PredicateToFunctionMap<Type, string>(null, valuesList);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ListConstructor_ArgumentExceptionThrownForNullValueFunctionCollection()
        {
            var keysList = new List<Predicate<Type>> {
                (t) => t.IsAssignableFrom(typeof(Exception)),
                (t) => t.Name == "PredicateToFunctionMapCreationTests",
                (t) => !t.Namespace.Contains("PredicateMaps")
            };

            new PredicateToFunctionMap<Type, string>(keysList, null);
        }

        [Test, ExpectedException(typeof(InconsistentIndexException))]
        public void ListConstructor_InconsistentIndexExceptionThrownWhenKeyAndValueCollectionsAreDifferentSizes()
        {
            var keysList = new List<Predicate<string>> { (s) => s.Contains("hello") };
            var valueList = new List<Func<string, string>> 
            { 
                s => "string is a greeting: " + s,  
                s => "string is a goodbye."
            };
            new PredicateToFunctionMap<string, string>(keysList, valueList);
        }
    }
}
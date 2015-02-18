﻿using NUnit.Framework;
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
    public class ClassPredicateMapTests
    {
        [Test]
        public void CountReturnsSizeOfMap()
        {
            var keys = new List<Predicate<int>> { (i) => i > 1, (i) => i == 1, (i) => 1 < 1 };
            var data = new List<string> { "More than 1", "Exactly 1", "Less than 1" };

            var map = new ClassPredicateMap<int, string>(keys, data);

            Assert.AreEqual(3, map.GetCount());
        }

        [Test]
        public void AddPutsKeyAndDataItemInMap()
        {
            var map = new ClassPredicateMap<int, string>();
            
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
                var map = new ClassPredicateMap<string, string>();
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
                var map = new ClassPredicateMap<string, string>();
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
            var predicateList = new List<Predicate<int>> { (i) => i == 5 };
            var value = "This should get returned.";
            var valueList = new List<string> { value };
            
            var predicateMap = new ClassPredicateMap<int, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(5);

            Assert.AreEqual(value, returnedValue);
        }
        
        [Test]
        public void GetFirstMatch_RetrunsNullWhenNoMatchesExists()
        {
            var predicateList = new List<Predicate<int>> { (i) => i == 5, (i) => i == 3 };
            var valueList = new List<string> { "This should NOT get returned.", "Neither should this" };

            var predicateMap = new ClassPredicateMap<int, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(4);

            Assert.Null(returnedValue);
        }

        [Test]
        public void GetFirstMatch_AcceptsNullAsParameter()
        {
            var predicateList = new List<Predicate<string>> { (s) => s == null, (s) => s.Length > 10 };
            var value = "This will be matched for null";
            var valueList = new List<string> { value, "value for longer strings" };

            var predicateMap = new ClassPredicateMap<string, string>(predicateList, valueList);
            var returnedValue = predicateMap.GetFirstMatch(null);

            Assert.AreEqual(value, returnedValue);
        }

        [Test]
        public void AddAll_AddsAllItemsInCollectionToMap()
        {
            var predicates = new List<Predicate<double>> { (d) => d < 2.0, (d) => d == null, (d) => d > 100.0 };
            var values = new List<string> { "Small double", "That's null", "Large double." };
            var map = new ClassPredicateMap<double, string>();
            map.AddAll(predicates, values);

            Assert.True(map.GetCount() == predicates.Count && map.GetCount() == values.Count);
        }

        [Test]
        [ExpectedException(typeof(InconsistentIndexException))]
        public void AddAll_ThrowsInconsistentIndexExceptionWhenSizesOfParameterCollectionsDoNoMatch()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 0 };
            var values = new List<string> { "0", "4", "4", "E", "6" };
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(predicates, values);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenKeyCollectionIsNull()
        {
            var valuesCollection = new List<string>();
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(null, valuesCollection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAll_ThrowsInvalidArgumentExceptionWhenValueCollectionIsNull()
        {
            var predicatesCollection = new List<Predicate<string>>();
            var map = new ClassPredicateMap<string, string>();

            map.AddAll(predicatesCollection, null);
        }

        [Test]
        public void GetAllMatches_RetrunsAllMatches()
        {
            var predicates = new List<Predicate<int>> { (i) => i == 4, (i) => i < 5, (i) => i > 10 };
            var values = new List<string> { "It's 4", "Less than 5", "Over 10" };

            var map = new ClassPredicateMap<int, string>(predicates, values);
            var result = map.GetAllMatches(4);

            var expected = new List<string> { "It's 4", "Less than 5" };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void GetAllMatches_ReturnsEmptyCollectionWhenNoMatchesFound()
        {
            var predicates = new List<Predicate<double>> { (d) => d < 1.0 };
            var values = new List<string> { "This is not the match you are looking for" };
            var map = new ClassPredicateMap<double, string>(predicates, values);

            var result = map.GetAllMatches(3.7);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetAllMatches_ReturnsCorrectValuesWhenTwoKeyPredicatesAreIdentical()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 1, (s) => s == "other", (s) => s.Length == 1 };
            var values = new List<string> { "match one", "No Match", "match two" };
            var map = new ClassPredicateMap<string, string>(predicates, values);

            var expected = new List<string> { "match one", "match two" };
            var result = map.GetAllMatches("s");
            
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void CountMatches_ReturnsZeroWhenMapIsEmpty()
        {
            var map = new ClassPredicateMap<string, string>();
            var matches = map.CountMatches("TestValue");
            Assert.AreEqual(0, matches);
        }

        [Test]
        public void CountMatches_ReturnsZeroWhenMapHasNoMatches()
        {
            var predicates = new List<Predicate<Type>> 
                                    { 
                                        (t) => t == typeof(ArgumentException), 
                                        (t) => t == typeof(NullReferenceException), 
                                        (t) => t  == typeof(StackOverflowException) 
                                    };
            var values = new List<string> { "That was invalid", "Something was null", "Stack has overflown" };
            var map = new ClassPredicateMap<Type, string>();
            var matches = map.CountMatches(("This is not an exception").GetType());
            Assert.AreEqual(0, matches);
        }

        [Test]
        public void CountMatches_ReturnsCountForMatchesWhenItemsInMapAreTrueForParameter()
        {
            var predicates = new List<Predicate<int>> { (i) => i < 9, (i) => i == 5, (i) => i > 20 };
            var values = new List<string> { "under 9", "five", "large number" };
            var map = new ClassPredicateMap<int, string>(predicates, values);
            var matches = map.CountMatches(5);
            Assert.AreEqual(2, matches);
        }

        [Test]
        public void GetIndexOfMatches_ReturnsEmptyCollectionWhenNothingMatches()
        {
            var map = new ClassPredicateMap<Type, string>();
            var result = map.GetIndexesOfMatches(typeof(Exception));
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetIndexOfMatches_ReturnsIndexOfAllMatches()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 2, (s) => s == "A string", (s) => s.Contains("string") };
            var values = new List<string> { "2 letters", "It's A string", "Meta-string" };
            var map = new ClassPredicateMap<string, string>(predicates, values);

            var matches = map.GetIndexesOfMatches("A string");
            var expected = new[] { 1, 2 };
            CollectionAssert.AreEquivalent(expected, matches);
        }

        [Test]
        public void Remove_RemovesItemsAtIndex()
        {
            var predicates = new List<Predicate<int>> { (i) => i > 5, (i) => i < 9, (i) => i != null };
            var values = new List<string> { "over 5", "under 9", "Populated." };
            var map = new ClassPredicateMap<int, string>(predicates, values);

            var countPreRemove = map.GetCount();
            map.RemoveKeyValuePairAtGivenIndex(2);
            var countPostRemove = map.GetCount();
            
            Assert.True(countPreRemove > countPostRemove);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Remove_ThrowsIndexOutOfRangeExceptionWhenIndexIsOutsideRangeOfMapsIndex()
        {
            var map = new ClassPredicateMap<Type, string>();
            map.RemoveKeyValuePairAtGivenIndex(42);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Remove_ThrowsIndexOutOfRangeExceptionWhenIndexIsEqualToCountOfMap()
        {
            var map = new ClassPredicateMap<Type, string>();
            map.Add((t) => t == typeof(Exception), "It's an exception");
            map.RemoveKeyValuePairAtGivenIndex(map.GetCount());
        }

        [Test]
        public void AnyMatches_ReturnsFalseWhenNothingInTheMapMatches()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length == 3, (s) => s == null };
            var values = new List<string> { "Three", "null" };
            var map = new ClassPredicateMap<string, string>(predicates, values);

            Assert.False(map.AnyMatches("This string matches nothing."));
        }

        [Test]
        public void AnyMatches_ReturnsTrueWhenSomethingInMapIsTrueForValue()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Length > 10, (s) => s != null, (s) => s.Contains("fred") };
            var values = new List<string> { "over 10", "not null", "Has a FRED" };
            var map = new ClassPredicateMap<string, string>(predicates, values);

            Assert.True(map.AnyMatches("This string matches for Most"));
        }

        [Test]
        public void UpdateByValue_DoesNotUpdateAnythingWhenNoPredicatesMatchForTheValue()
        {
            var predicates = new List<Predicate<int>> { (i) => i > 5, (i) => i == 20, (i) => i == -1 };
            var values = new List<string> { "these", "wont", "change" };
            var map = new ClassPredicateMap<int, string>(predicates, values);

            var valueToTryToInsert = "This value wont get in.";
            map.UpdateValueInMapForPredicate(2, valueToTryToInsert);
            var valuesAfterUpdate = map.ValueItemList;

            Assert.False(valuesAfterUpdate.Contains(valueToTryToInsert));
            CollectionAssert.AreEquivalent(values, valuesAfterUpdate);
        }

        [Test]
        public void UpdateByValue_UpdatesAllValuesWherePredicatesResolveToTrueForTheValue()
        {
            var predicates = new List<Predicate<int>> { (i) => i > 5, (i) => i == 20, (i) => i == -1 };
            var values = new List<string> { "this will change", "so wil this", "this wont change" };
            var map = new ClassPredicateMap<int, string>(predicates, values);

            var valueToUpdateTo = "This value was put in";
            map.UpdateValueInMapForPredicate(20, valueToUpdateTo);
            
            var valuesAfterUpdate = map.ValueItemList;
            var expectedCollection = new List<string> { valueToUpdateTo, valueToUpdateTo, "this wont change" };
            
            CollectionAssert.AreEqual(values, valuesAfterUpdate);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void UpdateByIndex_ThrowsIndexOutOFExceptionWhenIndexIsOutsideOfRangeOfMap()
        {
            var map = new ClassPredicateMap<string, string>();
            map.UpdateValueAtIndex(500, "This is never going to work.");
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void UpdateByIndex_ThrowsIndexOutOfRangeExceptionWhenIndexIsEqualToCountOfMap()
        {
            var map = new ClassPredicateMap<Type, string>();
            map.Add((t) => t == typeof(Exception), "Something went wrong.");
            map.UpdateValueAtIndex(map.GetCount(), "This shouldn't work");
        }

        [Test]
        public void UpdateByIndex_ChangesValueAtIndex()
        {
            var predicates = new List<Predicate<string>> { (s) => s.Contains("words"), (s) => !string.IsNullOrEmpty(s) };
            var initialValue = "Second item";
            var values = new List<string> { "one", initialValue };
            var map = new ClassPredicateMap<string, string>(predicates, values);
            var newItemToAdd = "inserted value";
            
            map.UpdateValueAtIndex(1, newItemToAdd);
            var valueAfterUpdate = map.ValueItemList[1];

            Assert.AreEqual(newItemToAdd, valueAfterUpdate);
            Assert.AreNotEqual(initialValue, valueAfterUpdate);
        }
    }
}
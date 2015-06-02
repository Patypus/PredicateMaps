#Predicate Maps Library
[![Build Status](https://travis-ci.org/Patypus/PredicateMaps.svg?branch=master)](https://travis-ci.org/Patypus/PredicateMaps)
[![NuGet](https://img.shields.io/nuget/v/PredicateMaps.svg)](http://www.nuget.org/packages/PredicateMaps)
=============
This is a libraries project which contains a wrapper class (PredicateMap) which wraps a dictionary of type Predicate((K), V). The wrapper hides the selecting values where the predicates evaluate to true for simplicity in the caller.

As a project this needs more polish in it but I'm working on it!

=============

##Getting started
#####Creating
Once you have the PredicateMaps dll included in you project and added as a reference, see the installation section for adding it, start by creating a new PredicateMap:
```C#
var map = new PredicateMap<Type, string>("No matches found");
```
The types applied refer to the parameter type for the predicate keys, *Type* in this case and the value type for the values, in this case, is *string*. So this new map wraps a *Dictionary<Predicate<Type>,string>*. The string parameter provided is used as the default value for a return if no matches are found for a Type passed to the maps. 
#####Adding values
Values can be added to the map with one of the map's add functions like this:
```C#
map.Add((type) => type == typeof(ArgumentException), "An argument was invalid");
```
Other add functions can add mutliple values at once:
```C#
AddAll(List<Predicate<K>> keyList, List<V> valueList)
```
In Addition, some of the constructors can add values as the map is created:
```C#
new PredicateMap(IDictionary<Predicate<K>, V> mappingToWrap, V defaultValue)
new PredicateMap(List<Predicate<K>> keyList, List<V> valuesList, V defaultValue)
```
All constructors take the default value that the first example constructor took in addition to the keys and values to populate the PredicateMap. 
#####Getting matches
Matches can be retrieved singly like this example for the map created above:
```C#
var exception = new ArgumentException();
map.GetFirstMatch(exception.GetType());
```
This will return "An argument was invalid" for the ArgumentException type and will return "No matches found" for types other than that of ArgumentException.
The GetAllMatches function can be used find all of the values which match a given test value if multiple matches could exist in the map for the test value.
```C#
GetAllMatches(K valueToTest);
```
This will return a collection of matching values or an empty collection if no matches are found.

=============
##Installation

The dll of the PredicateMaps project is available as a nuget package for easy integration to other projects [here at nuget.org](https://www.nuget.org/packages/PredicateMaps).

The nuget package is also bundled as an artefact for each release of the project so the required version can be taken from the projects releases page and added to a private nuget repo.

Alternatively you can always clone the repo, build the project for yourself and include the dlls wherever you like.

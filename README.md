#PredicateMaps
[![Build Status](https://travis-ci.org/Patypus/PredicateMaps.svg?branch=master)](https://travis-ci.org/Patypus/PredicateMaps)
=============
This is a libraries project which contains a wrapper class (PredicateMap) which wraps a dictionary of type Predicate((K), V). The wrapper hides the selecting values where the predicates evaluate to true for simplicity in the caller.

As a project this needs more polish in it but I'm working on it!

=============

##Getting started

Once you have the PredicateMaps dll included in you project and added as a reference, see the installation section for adding it, start by creating a new PredicateMap:
```C#
var map = new PredicateMap<Type, string>("No matches found");
```
The types applied refer to the parameter type for the predicate keys, *Type* in this case and the value type for the values, in this case *string*. So this new map wraps a *Dictionary<Predicate<Type>,string>*. The string parameter provided is used as the default value for a return in no matches are found for a Type passed to the maps. 

=============
##Installation

The dll of the PredicateMaps project is available as a nuget package for easy integration to other projects [here at nuget.org](https://www.nuget.org/packages/PredicateMaps).

The nuget package is also bundled as an artefact for each release of the project so the required version can be taken from the projects releases page and added to a private nuget repo.

Alternatively you can always clone the repo, build the project for yourself and include the dlls wherever you like.

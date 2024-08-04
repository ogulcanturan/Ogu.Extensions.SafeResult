# <img src="logo/ogu-logo.png" alt="Header" width="24"/> Ogu.Extensions.SafeResult

[![.NET](https://github.com/ogulcanturan/Ogu.Extensions.SafeResult/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/ogulcanturan/Ogu.Extensions.SafeResult/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/Ogu.Extensions.SafeResult.svg?color=1ecf18)](https://nuget.org/packages/Ogu.Extensions.SafeResult)
[![Nuget](https://img.shields.io/nuget/dt/Ogu.Extensions.SafeResult.svg?logo=nuget)](https://nuget.org/packages/Ogu.Extensions.SafeResult)

## Introduction

Ogu.Extensions.SafeResult is a library that safely creates collections from separated strings.

## Features

- **Safe Parsing:** Safely parse separated strings into collections without encountering exceptions.
- **Customization:** Specify whether parsing should stop on the first failure or continue parsing subsequent elements.
- **Supports:** List, HashSet, and Dictionary (extensions: ToSafeList, ToSafeHashSet, ToSafeDictionary)

## Installation

You can install the library via NuGet Package Manager:

```bash
dotnet add package Ogu.Extensions.SafeResult
```

## Usage

**HashSet:**
```csharp
public class Request
{
    [FromQuery(Name = "ids"), JsonIgnore]
    public string Ids { get; set; } = "1,2,3  , 4, a, 6";

    [BindNever, JsonIgnore]
    public ISafeResult<HashSet<int>> IdList => _idList ??= SafeResult<int>.HashSet(Ids, stopOnFailure: true);

    private ISafeResult<HashSet<int>> _idList;
}
```

IdList.Result =>

```bash
[1,2,3,4]
```

**EnumHashSet:**
```csharp
public enum Color
{
    Red,
    Green,
    Blue,
}

public class Request
{
    [FromQuery(Name = "ids"), JsonIgnore]
    public string Ids { get; set; } = "0, a, 16, 1, 2";

    [BindNever, JsonIgnore]
    public ISafeResult<HashSet<Color>> IdList => _idList ??= SafeResult<Color>.EnumHashSet(Ids, stopOnFailure: true);

    private ISafeResult<HashSet<Color>> _idList;
}
```

IdList.Result =>

```bash
[Red]
```

**OrderedDictionary:**
```csharp
public class Request
{
    [FromQuery(Name = "ids"), JsonIgnore]
    public string Ids { get; set; } = "1,2,3  , 4, a, 6";

    [BindNever, JsonIgnore]
    public ISafeResult<IDictionary<int, int>> IdList => _idList ??= SafeResult<int>.OrderedDictionary(Ids, stopOnFailure: false);

    private ISafeResult<IDictionary<int, int>> _idList;
}
```

IdList.Result =>

```bash
{
  "1":0,
  "2":1,
  "3":2,
  "4":3,
  "6":4
}
```

**List:**
```csharp
public class Request
{
    [FromQuery(Name = "ids"), JsonIgnore]
    public string Ids { get; set; } = "Item1,Item2,Item3,item3,25";

    [BindNever, JsonIgnore]
    public ISafeResult<List<int>> IdList => _idList ??= SafeResult<int>.List(Ids, stopOnFailure: false);

    private ISafeResult<List<int>> _idList;
}
```

IdList.Result =>

```bash
[25]
```




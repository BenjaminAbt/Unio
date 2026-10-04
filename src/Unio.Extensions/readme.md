# Unio.Extensions

Functional and fluent extensions for `Unio<T0, T1>` with allocation-conscious APIs and `Task`-based async composition.

Targets .NET 9, .NET 10 and .NET 11 RC1. Compatible with Native AOT and trimming; reference AOT metadata is verified on .NET 10 and newer.

## Features

- Branch mapping: `MapT0`, `MapT1`, `BindT0`, `BindT1`
- Async branch mapping: `BindT0Async`, `BindT1Async` (`Task`)
- Side-effects: `TapT0`, `TapT1`, `TapT0Async`, `TapT1Async`
- Recovery: `RecoverT0`, `RecoverT1`
- Validation guards: `EnsureT0`, `EnsureT1`
- Folding: `Fold`, `FoldAsync` (`Task`)
- Branch observation: `OnT0`, `OnT1`
- TryPick helpers: `PickT0Or`, `PickT1Or`
- Dual mapping: `BiMap`
- LINQ support: `Select`, `SelectMany`
- Result bridges: `ToResult`, `FromResult`

## Quick Start

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> value = "invalid";

int normalized = value
    .TapT1(static error => Console.WriteLine(error))
    .EnsureT1(static e => e.Length < 20, static _ => -1)
    .RecoverT1(static _ => -1);
```

## Async Pipeline

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> value = 21;

Unio<double, string> mapped = await value.BindT0Async(static i => Task.FromResult(i * 2.0));
Unio<double, string> result = await mapped.TapT0Async(static d =>
    {
        Console.WriteLine($"computed: {d}");
        return Task.CompletedTask;
    });
```

## LINQ Query Syntax

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> left = 4;

Unio<int, string> sum =
    from x in left
    from y in (Unio<int, string>)(x * 3)
    select x + y;
```

## Result Bridge

```csharp
using Unio;
using Unio.Extensions;
using Unio.Types;

Unio<int, string> value = 42;

Unio<Result<int>, Error<string>> wrapped = value.ToResult();
Unio<int, string> roundtrip = wrapped.FromResult();
```

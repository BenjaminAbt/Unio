# Unio.Extensions

Functional and fluent extensions for `Unio<T0, T1>` with allocation-conscious APIs and `ValueTask`-based async composition.

## Features

- Branch mapping: `MapT0`, `MapT1`, `BindT0`, `BindT1`
- Async branch mapping: `BindT0Async`, `BindT1Async` (`ValueTask`)
- Side-effects: `TapT0`, `TapT1`, `TapT0Async`, `TapT1Async`
- Recovery: `RecoverT0`, `RecoverT1`
- Validation guards: `EnsureT0`, `EnsureT1`
- Folding: `Fold`, `FoldAsync` (`ValueTask`)
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

## ValueTask Async Pipeline

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> value = 21;

Unio<double, string> result = await value
    .BindT0Async(static i => ValueTask.FromResult(i * 2.0))
    .TapT0Async(static d =>
    {
        Console.WriteLine($"computed: {d}");
        return ValueTask.CompletedTask;
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

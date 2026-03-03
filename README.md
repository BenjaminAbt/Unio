# Unio

<p align="center">
    <img src="res/unio.png" alt="Unio" width="160" />
</p>

**High-performance discriminated unions for C# - zero-allocation `readonly struct` core, source generator with class-based named types.**

High-performance discriminated unions for C# with exhaustive matching, `TryGet` patterns, full value equality semantics, a Roslyn incremental source generator for named union types and 39 ready-to-use sentinel and value-carrying types.

[![Unio](https://img.shields.io/nuget/v/Unio.svg?label=Unio)](https://www.nuget.org/packages/Unio)
[![Unio.SourceGenerator](https://img.shields.io/nuget/v/Unio.SourceGenerator.svg?label=Unio.SourceGenerator)](https://www.nuget.org/packages/Unio.SourceGenerator)
[![Unio.Types](https://img.shields.io/nuget/v/Unio.Types.svg?label=Unio.Types)](https://www.nuget.org/packages/Unio.Types)
[![Unio.AspNetCore](https://img.shields.io/nuget/v/Unio.AspNetCore.svg?label=Unio.AspNetCore)](https://www.nuget.org/packages/Unio.AspNetCore)
[![Unio.Extensions](https://img.shields.io/nuget/v/Unio.Extensions.svg?label=Unio.Extensions)](https://www.nuget.org/packages/Unio.Extensions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## Table of Contents

- [Why Unio?](#why-unio)
- [Packages](#packages)
- [Installation](#installation)
- [Quick Start](#quick-start)
  - [Generic Union Types](#generic-union-types)
  - [Named Union Types (Source Generator)](#named-union-types-source-generator)
  - [Pre-built Types](#pre-built-types)
- [Supported Arities](#supported-arities)
- [API Reference](#api-reference)
  - [Union Type Members](#union-type-members)
  - [Implicit Conversions](#implicit-conversions)
  - [Equality & Hashing](#equality--hashing)
  - [Match & Switch](#match--switch)
  - [TryGet Pattern](#tryget-pattern)
- [Unio.Types - Pre-built Sentinel & Value Types](#uniotypes--pre-built-sentinel--value-types)
  - [Marker Types (Empty Sentinels)](#marker-types-empty-sentinels)
  - [Value-Carrying Types](#value-carrying-types)
  - [Type Reference Table](#type-reference-table)
- [Source Generator](#source-generator)
  - [How It Works](#how-it-works)
  - [Generated API Surface](#generated-api-surface)
  - [Diagnostics](#diagnostics)
- [Real-World Examples](#real-world-examples)
  - [Result Pattern (API Controller)](#result-pattern-api-controller)
  - [CRUD Operations](#crud-operations)
  - [Error Handling with Recovery](#error-handling-with-recovery)
  - [Validation Pipeline](#validation-pipeline)
  - [State Machine](#state-machine)
  - [Option / Maybe Pattern](#option--maybe-pattern)
- [Performance](#performance)
  - [Design Decisions](#design-decisions)
  - [Benchmark Results](#benchmark-results)
- [Acknowledgements](#acknowledgements)
- [Code Generation Tool](#code-generation-tool)
- [Building & Testing](#building--testing)
- [Project Structure](#project-structure)
- [License](#license)

---

## Why Unio?

Discriminated unions are a powerful pattern for modeling mutually exclusive states without exceptions or null. C# does not yet have native union types, so developers rely on libraries to fill that gap - but existing solutions often use classes and `object` boxing, adding GC pressure and losing type safety at runtime.

**Unio** provides a modern approach:

- **Typed generic fields** - no casts to `object`, no boxing
- **Exhaustive matching** - `Match<TResult>` and `Switch` force handling of all cases
- **Allocation-free matching** - `Match<TState, TResult>`, `Switch<TState>`, `MatchAsync<TState, TResult>` and `SwitchAsync<TState>` pass context via a state parameter instead of a capturing closure, eliminating lambda allocation on hot paths
- **Safe access** - `TryGetT0..TryGetTn` pattern prevents runtime exceptions
- **Full value equality** - `IEquatable<T>`, `==`, `!=`, `GetHashCode`
- **Source generator** - define named unions like `StringOrInt` with zero boilerplate
- **Pre-built types** - 39 sentinel and value types for common patterns (NotFound, Success, Error, etc.)
- **Maximum performance** - `readonly struct` core type eliminates heap allocation; `[AggressiveInlining]` on all hot paths, TieredPGO/DynamicPGO enabled

---

## Packages

| Package | Description | NuGet |
|---|---|---|
| `Unio` | Core union types `Unio<T0,T1>` through `Unio<T0,...,T19>` as `readonly struct`, `UnioBase<...>` abstract base classes, marker attribute & interfaces | [![NuGet](https://img.shields.io/nuget/vpre/Unio.svg)](https://www.nuget.org/packages/Unio) |
| `Unio.SourceGenerator` | Roslyn incremental source generator for named unions | [![NuGet](https://img.shields.io/nuget/vpre/Unio.SourceGenerator.svg)](https://www.nuget.org/packages/Unio.SourceGenerator) |
| `Unio.Types` | 39 pre-built sentinel and value-carrying types | [![NuGet](https://img.shields.io/nuget/vpre/Unio.Types.svg)](https://www.nuget.org/packages/Unio.Types) |
| `Unio.AspNetCore` | ASP.NET Core Minimal API integration (`ToHttpResult()` extension) | [![NuGet](https://img.shields.io/nuget/vpre/Unio.AspNetCore.svg)](https://www.nuget.org/packages/Unio.AspNetCore) |
| `Unio.Extensions` | Fluent functional extensions for `Unio<T0,T1>` including `ValueTask` async helpers | [![NuGet](https://img.shields.io/nuget/vpre/Unio.Extensions.svg)](https://www.nuget.org/packages/Unio.Extensions) |

---

## Installation

```bash
# Core library (required)
dotnet add package Unio

# Source generator for named union types (optional)
dotnet add package Unio.SourceGenerator

# Pre-built sentinel & value types (optional)
dotnet add package Unio.Types

# ASP.NET Core Minimal API integration (optional)
dotnet add package Unio.AspNetCore

# Functional extensions (optional)
dotnet add package Unio.Extensions
```

---

## Quick Start

### Generic Union Types

```csharp
using Unio;

// Create via implicit conversion - the compiler picks the right slot
Unio<int, string> result = 42;
Unio<int, string> error = "Something went wrong";

// Exhaustive pattern matching - the compiler ensures all cases are handled
string message = result.Match(
    value => $"Success: {value}",
    err   => $"Error: {err}");

// Type checking via IsT# properties
if (result.IsT0)
    Console.WriteLine($"Got int: {result.AsT0}");

// Safe access with TryGet - no exceptions thrown
if (result.TryGetT0(out int number))
    Console.WriteLine($"Number: {number}");

// Side-effect switching
result.Switch(
    value => Console.WriteLine($"Value: {value}"),
    err   => Console.Error.WriteLine($"Error: {err}"));
```

### Named Union Types (Source Generator)

Create strongly-typed, named union types with zero boilerplate. Install `Unio.SourceGenerator`, then:

```csharp
using Unio;

[GenerateUnio]
public partial class StringOrInt : UnioBase<string, int>;

[GenerateUnio]
public partial class ApiResult : UnioBase<User, NotFound, ValidationError>;
```

The source generator produces the full union API automatically:

```csharp
StringOrInt value = "hello";

if (value.IsT0)
    Console.WriteLine(value.AsT0);  // "hello"

string result = value.Match(
    s => $"string: {s}",
    i => $"int: {i}");

// Equality works out of the box
StringOrInt a = 42;
StringOrInt b = 42;
bool equal = a == b;  // true

// TryGet, Switch - everything is generated
if (value.TryGetT0(out string str))
    Console.WriteLine(str);
```

### Pre-built Types

Install `Unio.Types` for ready-to-use sentinel types:

```csharp
using Unio;
using Unio.Types;

// API result pattern with pre-built markers
Unio<User, NotFound, Forbidden> GetUser(int id)
{
    if (!IsAuthorized()) return new Forbidden();
    var user = _repo.Find(id);
    return user is not null ? user : new NotFound();
}

// Rich results with value-carrying types
Unio<Success<Order>, ValidationError, Conflict> CreateOrder(OrderRequest req)
{
    if (!Validate(req, out var errors)) return new ValidationError(errors);
    if (HasConflict(req)) return new Conflict();
    return new Success<Order>(ProcessOrder(req));
}
```

### Functional Extensions (`Unio.Extensions`)

Install `Unio.Extensions` for fluent map/bind/tap/recover APIs and `ValueTask`-based async composition.

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> value = "invalid";

int normalized = value
    .TapT1(static error => Console.WriteLine(error))
    .EnsureT1(static e => e.Length < 20, static _ => -1)
    .RecoverT1(static _ => -1);

Unio<double, string> asyncMapped = await ((Unio<int, string>)21)
    .BindT0Async(static i => ValueTask.FromResult(i * 2.0));
```

---

## Supported Arities

Unio supports 2 to 20 type parameters (2–9 shown as examples):

| Type | Parameters |
|---|---|
| `Unio<T0, T1>` | 2 types |
| `Unio<T0, T1, T2>` | 3 types |
| `Unio<T0, T1, T2, T3>` | 4 types |
| `Unio<T0, T1, T2, T3, T4>` | 5 types |
| `Unio<T0, T1, T2, T3, T4, T5>` | 6 types |
| `Unio<T0, T1, T2, T3, T4, T5, T6>` | 7 types |
| `Unio<T0, T1, T2, T3, T4, T5, T6, T7>` | 8 types |
| `Unio<T0, T1, T2, T3, T4, T5, T6, T7, T8>` | 9 types |
| `Unio<T0, ..., T19>` | up to 20 types |

The same arities (2–20) apply to the source generator - `UnioBase<T0, ..., T19>` supports all 20 arities.

---

## API Reference

### Union Type Members

Each `Unio<...>` (both core and source-generated) provides:

| Member | Return Type | Description |
|---|---|---|
| `Index` | `int` | Zero-based index of the currently stored type |
| `Value` | `object` | Currently stored value (boxed) |
| `IsT0` .. `IsTn` | `bool` | Returns `true` if the union holds the type at that index |
| `AsT0` .. `AsTn` | `T0` .. `Tn` | Returns the value; throws `InvalidOperationException` if wrong type |
| `TryGetT0(out T0)` .. `TryGetTn(out Tn)` | `bool` | Returns `true` and sets `out` parameter if the type matches |
| `TryPickT0(out T0, out ...)` .. `TryPickTn(out Tn, out ...)` | `bool` | Returns `true` and clears remainder if the type matches; otherwise returns `false` and provides the remainder value/union |
| `Match<TResult>(Func<T0, TResult>, ...)` | `TResult` | Exhaustive functional match - one function per type |
| `Match<TState, TResult>(TState, Func<TState, T0, TResult>, ...)` | `TResult` | Allocation-free match - passes `state` to `static` lambdas instead of capturing variables |
| `Switch(Action<T0>, ...)` | `void` | Exhaustive side-effect switch - one action per type |
| `Switch<TState>(TState, Action<TState, T0>, ...)` | `void` | Allocation-free switch - passes `state` to `static` lambdas instead of capturing variables |
| `Match<TResult>(Func<T0, Task<TResult>>, ...)` | `Task<TResult>` | Exhaustive async functional match |
| `Match<TState, TResult>(TState, Func<TState, T0, Task<TResult>>, ...)` | `Task<TResult>` | Allocation-free async match - passes `state` to `static` lambdas |
| `Switch(Func<T0, Task>, ...)` | `Task` | Exhaustive async side-effect switch |
| `Switch<TState>(TState, Func<TState, T0, Task>, ...)` | `Task` | Allocation-free async switch - passes `state` to `static` lambdas |
| `Equals(other)` | `bool` | Structural equality via `IEquatable<T>` |
| `GetHashCode()` | `int` | Hash code based on index + active value |
| `ToString()` | `string` | Delegates to the active value's `ToString()` |
| `==` / `!=` | `bool` | Value equality operators |

### Implicit Conversions

Every union type has implicit conversion operators from each of its type parameters:

```csharp
Unio<int, string, bool> union;

// All of these work via implicit conversion:
union = 42;              // stores int at index 0
union = "hello";         // stores string at index 1
union = true;            // stores bool at index 2
```

### Equality & Hashing

Unions implement `IEquatable<T>` with full structural equality:

```csharp
Unio<int, string> a = 42;
Unio<int, string> b = 42;
Unio<int, string> c = "hello";

a == b;  // true  - same type, same value
a == c;  // false - different types
a != c;  // true

// Works in dictionaries and HashSets
var set = new HashSet<Unio<int, string>> { a, b, c };
// set.Count == 2  (a and b are equal)
```

### Match & Switch

`Match` and `Switch` enforce exhaustive handling - every branch must be covered:

```csharp
// Match returns a value - functional pattern
string result = union.Match(
    i => $"integer: {i}",
    s => $"string: {s}",
    b => $"boolean: {b}");

// Switch executes an action - side-effect pattern
union.Switch(
    i => Console.WriteLine($"int: {i}"),
    s => Console.WriteLine($"string: {s}"),
    b => Console.WriteLine($"bool: {b}"));
```

#### Allocation-Free Matching with `TState`

When a lambda captures a local variable, the compiler creates a new closure object on the heap every time the delegate is invoked. The `Match<TState, TResult>`, `Switch<TState>`, overloads eliminate this allocation by passing a state value directly alongside each `static` lambda:

```csharp
// ❌ captures `prefix` - allocates a new closure object per call
string result = union.Match(
    i => $"{prefix}: {i}",
    s => $"{prefix}: {s}",
    b => $"{prefix}: {b}");

// ✅ passes `prefix` as TState to static lambdas - zero allocation
string result = union.Match(prefix,
    static (p, i) => $"{p}: {i}",
    static (p, s) => $"{p}: {s}",
    static (p, b) => $"{p}: {b}");
```

For `Switch<TState>`, wrap any mutable targets you need to write to in a `ValueTuple`:

```csharp
// Passes (logger, config) as a ValueTuple state - no closure allocation
union.Switch((logger, config),
    static (s, i) => s.logger.LogInformation("int {V}", i),
    static (s, str) => s.logger.LogDebug("string {V}", str),
    static (_, b)  => { /* ... */ });
```

The same pattern applies to the async variants:

```csharp
// ❌ captures `db` - allocates per call
string result = await union.Match(
    async i => await db.GetIntAsync(i),
    async s => await db.GetStringAsync(s));

// ✅ passes `db` as TState to static lambdas - zero allocation
string result = await union.Match(db,
    static async (d, i) => await d.GetIntAsync(i),
    static async (d, s) => await d.GetStringAsync(s));

// Switch variant - pass multiple values via ValueTuple
await union.Switch((db, logger),
    static async (s, i) => { await s.db.SaveAsync(i); s.logger.LogInformation("Saved int"); },
    static async (s, str) => await s.db.LogAsync(str));
```

This pattern is especially valuable in loops, high-throughput pipelines and ASP.NET Core request handlers where per-call allocation matters.

### TryGet Pattern

Safe access without exceptions, following the `TryParse` idiom:

```csharp
Unio<int, string> result = GetResult();

if (result.TryGetT0(out int number))
{
    // number is available here, no exception possible
    ProcessNumber(number);
}
else if (result.TryGetT1(out string text))
{
    ProcessText(text);
}
```

---

## Unio.Types - Pre-built Sentinel & Value Types

The `Unio.Types` package provides 39 high-performance, pre-built types designed for common discriminated union patterns. All marker types are `readonly struct` with `IEquatable<T>`, `==`/`!=` operators and `[AggressiveInlining]` on equality checks.

### Marker Types (Empty Sentinels)

Marker types are zero-size sentinel structs with no data. They represent states or outcomes:

#### Boolean / Ternary

| Type | Description | Example |
|---|---|---|
| `Yes` | Affirmative result | `Unio<Data, Yes, No>` |
| `No` | Negative result | `Unio<Yes, No>` |
| `Maybe` | Indeterminate result | `Unio<Yes, No, Maybe>` |
| `True` | Boolean true marker | `Unio<True, False>` |
| `False` | Boolean false marker | `Unio<True, False>` |
| `Unknown` | Unknown state | `Unio<Result, Unknown>` |

#### Collection / Quantity

| Type | Description | Example |
|---|---|---|
| `All` | All items matched | `Unio<All, Some, None>` |
| `Some` | Partial match | `Unio<All, Some, None>` |
| `None` | No items / empty result | `Unio<Data, None>` |
| `Empty` | Empty / blank | `Unio<Content, Empty>` |

#### State

| Type | Description | Example |
|---|---|---|
| `Pending` | Operation in progress | `Unio<Result, Pending>` |
| `Cancelled` | Operation cancelled | `Unio<Result, Cancelled, Timeout>` |
| `Timeout` | Operation timed out | `Unio<Result, Timeout>` |
| `Skipped` | Operation was skipped | `Unio<Result, Skipped>` |
| `Invalid` | Invalid state / input | `Unio<Data, Invalid>` |
| `Disabled` | Feature / resource disabled | `Unio<Config, Disabled>` |
| `Expired` | Token / session / resource expired | `Unio<Session, Expired>` |
| `RateLimited` | Rate limit hit | `Unio<Response, RateLimited>` |

#### HTTP / API

| Type | Description | HTTP | Example |
|---|---|---|---|
| `NotFound` | Resource not found | 404 | `Unio<User, NotFound>` |
| `Forbidden` | Access denied | 403 | `Unio<User, Forbidden>` |
| `Unauthorized` | Authentication required | 401 | `Unio<Data, Unauthorized>` |
| `Conflict` | Resource conflict | 409 | `Unio<Updated, Conflict>` |
| `BadRequest` | Invalid request | 400 | `Unio<Data, BadRequest>` |
| `Accepted` | Accepted for processing | 202 | `Unio<Result, Accepted>` |
| `NoContent` | No content to return | 204 | `Unio<Data, NoContent>` |

#### CRUD Operations

| Type | Description | Example |
|---|---|---|
| `Created` | Resource was created | `Unio<Created, Conflict>` |
| `Updated` | Resource was updated | `Unio<Updated, NotFound>` |
| `Deleted` | Resource was deleted | `Unio<Deleted, NotFound>` |
| `Unchanged` | No change occurred | `Unio<Updated, Unchanged>` |

#### Result

| Type | Description | Example |
|---|---|---|
| `Success` | Operation succeeded | `Unio<Success, Error>` |
| `Error` | Operation failed | `Unio<Success, Error>` |

### Value-Carrying Types

Value-carrying types wrap a value of type `T` with semantic meaning:

| Type | Property | Description | Example |
|---|---|---|---|
| `Success<T>` | `T Value` | Success with result value | `new Success<Order>(order)` |
| `Error<T>` | `T Value` | Error with details | `new Error<string>("msg")` |
| `Result<T>` | `T Value` | Generic result wrapper | `new Result<int>(42)` |
| `NotFound<T>` | `T Value` | Not found with identifier | `new NotFound<int>(userId)` |
| `Created<T>` | `T Value` | Created with entity/ID | `new Created<int>(newId)` |
| `Updated<T>` | `T Value` | Updated with entity | `new Updated<User>(user)` |
| `ValidationError` | `string Message` | Validation error message | `new ValidationError("Name required")` |
| `ValidationError<T>` | `T Value` | Validation error details | `new ValidationError<string[]>(errors)` |

All value-carrying types support:
- **Implicit conversion** from `T` - `Success<int> s = 42;`
- **`IEquatable<T>`** - structural equality on the wrapped value
- **`==` / `!=` operators**
- **`ToString()`** - e.g. `"Success(42)"`, `"ValidationError(Name required)"`

### Type Reference Table

All 39 types at a glance:

| Category | Types |
|---|---|
| Boolean / Ternary | `Yes`, `No`, `Maybe`, `True`, `False`, `Unknown` |
| Collection | `All`, `Some`, `None`, `Empty` |
| State | `Pending`, `Cancelled`, `Timeout`, `Skipped`, `Invalid`, `Disabled`, `Expired`, `RateLimited` |
| HTTP / API | `NotFound`, `Forbidden`, `Unauthorized`, `Conflict`, `BadRequest`, `Accepted`, `NoContent` |
| CRUD | `Created`, `Updated`, `Deleted`, `Unchanged` |
| Result | `Success`, `Error` |
| Value Carriers | `Success<T>`, `Error<T>`, `Result<T>`, `NotFound<T>`, `Created<T>`, `Updated<T>`, `ValidationError`, `ValidationError<T>` |

---

## Source Generator

### How It Works

The `Unio.SourceGenerator` is a Roslyn **incremental source generator** (`IIncrementalGenerator`). It runs at compile time and generates complete union class implementations from minimal declarations.

**Step 1:** Declare a `partial class` with `[GenerateUnio]` inheriting from `UnioBase<...>`:

```csharp
using Unio;

[GenerateUnio]
public partial class StringOrInt : UnioBase<string, int>;
```

**Step 2:** The generator detects the class at compile time via:
1. **Syntactic filter** - fast check: is it a `partial class` with attributes and a base list?
2. **Semantic filter** - does it have `[GenerateUnio]`? Does it inherit from `UnioBase<...>`?
3. **Code generation** - emit a complete `.g.cs` file with the full API

**Step 3:** The generator produces a `sealed` class that **inherits from `UnioBase<string, int>`** - a pre-built abstract base class in the `Unio` package. All union operations (Match, Switch, TryGet, ValueOr, etc.) are inherited from `UnioBase`; the generated code only adds the constructor, implicit conversion operators and typed equality members. This keeps generated code minimal while naming types as full classes with class semantics.

### Generated API Surface

For a declaration like

```csharp
[GenerateUnio]
public partial class Result : UnioBase<User, NotFound, ValidationError>;
```

the generator produces a class inheriting from `UnioBase<User, NotFound, ValidationError>`. All operations from the base class are immediately available and the generator only emits:

```csharp
public sealed partial class Result : IEquatable<Result>
{
    // Constructor
    private Result(Unio<User, NotFound, ValidationError> union) : base(union) { }

    // Implicit conversion operators (one per type)
    public static implicit operator Result(User value);       // → UnioBase.IsT0 etc.
    public static implicit operator Result(NotFound value);
    public static implicit operator Result(ValidationError value);

    // Typed equality (strongly typed to Result - not UnioBase)
    public bool Equals(Result? other);
    public override bool Equals(object? obj);
    public override int GetHashCode();
    public static bool operator ==(Result? left, Result? right);
    public static bool operator !=(Result? left, Result? right);
}
```

All other members (`Index`, `IsT0`–`IsT2`, `AsT0`–`AsT2`, `TryGetT0`–`TryGetT2`, `Match<TResult>`, `Match<TState,TResult>`, `Switch`, `Switch<TState>`, `Match<TResult>`, `Match<TState,TResult>`, `MapT0`–`MapT2`, `ValueOrT0`–`ValueOrT2`, `ToString`, `IFormattable`, `ISpanFormattable`, `IUtf8SpanFormattable`) are **inherited from `UnioBase`**.

### Diagnostics

The source generator reports errors at compile time:

| Code | Severity | Description |
|---|---|---|
| `UNIO001` | Error | Class marked with `[GenerateUnio]` does not inherit from `UnioBase<...>` |
| `UNIO002` | Error | `UnioBase<...>` has unsupported arity (must be 2–20) |
| `UNIO003` | Warning | Duplicate type arguments in `UnioBase<...>` |
| `UNIO004` | Info | Union class should be declared as `sealed` |

Example:

```csharp
// UNIO001: Missing UnioBase<...> base class
[GenerateUnio]
public partial class Bad;

// UNIO002: Invalid arity
[GenerateUnio]
public partial class TooFew : UnioBase<int>;  // only 1 type - minimum is 2
```

---

## Real-World Examples

### Result Pattern (API Controller)

```csharp
using Unio;
using Unio.Types;

public Unio<User, NotFound, Forbidden> GetUser(int id, ClaimsPrincipal caller)
{
    if (!caller.IsInRole("Admin"))
        return new Forbidden();

    User? user = _repository.Find(id);
    if (user is null)
        return new NotFound();

    return user;
}

// In controller:
var result = GetUser(42, User);
var response = result.Match(
    user  => Ok(user),
    _     => NotFound(),
    _     => Forbid());
```

### CRUD Operations

```csharp
using Unio;
using Unio.Types;

public Unio<Created<int>, ValidationError, Conflict> CreateProduct(ProductDto dto)
{
    if (string.IsNullOrEmpty(dto.Name))
        return new ValidationError("Name is required");

    if (_repo.ExistsByName(dto.Name))
        return new Conflict();

    int id = _repo.Insert(dto);
    return new Created<int>(id);
}

var result = CreateProduct(dto);
result.Switch(
    created => Console.WriteLine($"Created with ID: {created.Value}"),
    error   => Console.WriteLine($"Validation failed: {error.Message}"),
    _       => Console.WriteLine("Product already exists"));
```

### Error Handling with Recovery

```csharp
using Unio;

Unio<int, FormatException, OverflowException> SafeParse(string input)
{
    try { return int.Parse(input, CultureInfo.InvariantCulture); }
    catch (FormatException ex) { return ex; }
    catch (OverflowException ex) { return ex; }
}

var parsed = SafeParse("abc");
var value = parsed.Match(
    number   => number,
    _        => -1,     // default for format errors
    _        => int.MaxValue);  // cap for overflow
```

### Validation Pipeline

```csharp
using Unio;
using Unio.Types;

public Unio<Success<Order>, ValidationError<string[]>> ValidateAndProcess(OrderRequest req)
{
    var errors = new List<string>();

    if (req.Quantity <= 0) errors.Add("Quantity must be positive");
    if (string.IsNullOrEmpty(req.ProductId)) errors.Add("ProductId is required");
    if (req.Price < 0) errors.Add("Price cannot be negative");

    if (errors.Count > 0)
        return new ValidationError<string[]>(errors.ToArray());

    var order = new Order(req.ProductId, req.Quantity, req.Price);
    return new Success<Order>(order);
}
```

### State Machine

```csharp
using Unio;
using Unio.Types;

[GenerateUnio]
public partial class JobState : UnioBase<Pending, Success<JobResult>, Error<string>, Cancelled, Timeout>;

JobState state = new Pending();

// Process...
state = new Success<JobResult>(result);

// Report
Console.WriteLine(state.Match(
    _       => "⏳ Pending...",
    success => $"✅ Done: {success.Value}",
    error   => $"❌ Failed: {error.Value}",
    _       => "🚫 Cancelled",
    _       => "⏰ Timed out"));
```

### Option / Maybe Pattern

```csharp
using Unio;
using Unio.Types;

// Simple Option<T> via union
Unio<string, None> FindName(int id)
{
    var name = _db.FindName(id);
    return name is not null ? name : new None();
}

var result = FindName(42);
var display = result.Match(
    name => name,
    _    => "(unknown)");
```

---

## Performance

```shell
| Method                              | Mean      | Error     | Ratio  | Allocated |
|------------------------------------ |----------:|----------:|-------:|----------:|
| Unio_Match_2Arity                   | 0.7800 ns | 0.8268 ns |   1.00 |      0 B  |
| OneOf_Match_2Arity                  | 0.7415 ns | 0.2101 ns |   0.95 |      0 B  |
| Unio_Match_5Arity                   | 1.0782 ns | 1.2554 ns |   1.00 |      0 B  |
| OneOf_Match_5Arity                  | 1.0814 ns | 0.1313 ns |   1.01 |      0 B  |
| Unio_Match_WithState_2Arity         | 4.8533 ns | 1.1143 ns |   1.00 |     48 B  |
| OneOf_Match_CapturingLambda_2Arity  | 5.6056 ns | 0.2197 ns |   1.16 |     72 B  |
| Unio_Match_WithState_5Arity         | 5.0281 ns | 0.6369 ns |   1.00 |     48 B  |
| OneOf_Match_CapturingLambda_5Arity  | 5.7137 ns | 0.2740 ns |   1.14 |     72 B  |
| Unio_Switch_WithState_2Arity        | 0.6918 ns | 0.0947 ns |   1.00 |      0 B  |
| OneOf_Switch_CapturingLambda_2Arity | 1.9457 ns | 0.5249 ns |   2.81 |     32 B  |
| Unio_Switch_WithState_5Arity        | 1.1188 ns | 0.1413 ns |   1.00 |      0 B  |
| OneOf_Switch_CapturingLambda_5Arity | 1.9274 ns | 0.0991 ns |   1.72 |     32 B  |
| Unio_ToString                       | 0.4430 ns | 0.0746 ns |   1.00 |      0 B  |
| OneOf_ToString                      | 5.1127 ns | 0.3844 ns |  11.54 |     56 B  |
| Unio_TryGetT0_5Arity                | 0.0014 ns | 0.0432 ns |      ? |      0 B  |
| OneOf_TryPickT0_5Arity              | 2.6490 ns | 0.1678 ns |      ? |      0 B  |
| Unio_TryGetT4_5Arity_Miss           | 0.0149 ns | 0.0103 ns |   1.00 |      0 B  |
| OneOf_TryPickT4_5Arity_Miss         | 4.1028 ns | 0.6939 ns | 276.00 |      0 B  |
| Unio_TryGetT1_Miss                  | 0.0185 ns | 0.0697 ns |   1.03 |      0 B  |
| OneOf_TryPickT1_Miss                | 0.1901 ns | 0.0629 ns |  10.52 |      0 B  |
```

### Design Decisions

Unio is built for maximum runtime performance:

| Decision | Benefit |
|---|---|
| `readonly struct` for core type | No heap allocation - stack-allocated for small value types |
| `UnioBase<...>` abstract class | Named (source-generated) union types get class semantics and reference identity |
| Typed generic fields (`T0? _value0`) | No `object` boxing - value types stored directly |
| `[MethodImpl(AggressiveInlining)]` | JIT inlines all property accessors, `TryGet`, `Match`, `Switch` and operators |
| `byte _index` discriminator | Minimal overhead: 1 byte to track the active type |
| `switch` expressions | JIT compiles to efficient jump tables |
| TieredPGO / DynamicPGO enabled | Profile-guided optimization for hot paths |
| Source-generated named types | Inherit from `UnioBase` - only constructor + implicit operators generated |
| Marker types (empty structs) | 1 byte size, zero-cost equality, `AggressiveInlining` |
| `Match<TState, TResult>` / `Switch<TState>` | State passed as parameter to `static` lambdas - capturing closures never allocated |

### Benchmark Results

Run benchmarks yourself:

```bash
dotnet run --configuration Release --project perf/Unio.Benchmarks/Unio.Benchmarks.csproj
```

Expected characteristics:
- **Creation**: Value-type construction via private constructor + implicit operator (no additional heap allocation)
- **IsT# / Index**: Single byte comparison, fully inlined
- **AsT#**: Single byte comparison + field access, fully inlined
- **TryGet**: Single byte comparison + field access + bool return, fully inlined
- **Match / Switch**: `switch` expression compiled to jump table by JIT
- **Match\<TState\> / Switch\<TState\>**: Same as above, but with delegate arguments that are `static` — **0 B allocated** vs. a closure object per call for the capturing variant
- **Equality**: Index comparison + `EqualityComparer<T>.Default`, fully inlined

---

## Acknowledgements

Unio was inspired by [OneOf](https://github.com/mcintyre321/OneOf), which pioneered discriminated unions in C#. However, a more modern, high-performance implementation was needed - with a `readonly struct` core type, `UnioBase<...>` for named class-based unions, typed generic fields and full value equality semantics.


| Feature | OneOf | Unio |
|---|---|---|
| **Core type** | `struct` (OneOf) / `class` (OneOfBase) | `readonly struct` |
| **Named union base class** | `OneOfBase<...>` abstract class | `UnioBase<...>` abstract class |
| **Value Storage** | `object` field (boxing for value types) | Typed generic fields |
| **Source Generator** | Basic: constructor + implicit operators | Inherits from `UnioBase` - only constructor + implicit operators generated |
| **Pre-built Types** | 13 types (5 in Assorted.cs + 4 named unions) | 39 types across 7 categories |
| **Try Pattern** | ✅ `TryPickT#(out value, out remainder)` | ✅ `TryGetT#(out value)` + `TryPickT#(out value, out remainder)` |
| **Allocation-free Match / Switch** | ❌ Capturing lambdas only | ✅ `Match<TState, TResult>` / `Switch<TState>` with `static` lambdas |
| **`IEquatable<T>`** | ❌ Not implemented | ✅ Full structural equality |
| **`==` / `!=` Operators** | ❌ Not available | ✅ Value equality operators |
| **`AggressiveInlining`** | ❌ Not marked | ✅ On all property accessors and methods |
| **Max Arity** | Up to 9 (OneOf.Extended) | 2–20 built-in |
| **Value-Carrying Types** | `Success<T>`, `Error<T>`, `Result<T>` | All of OneOf's + `NotFound<T>`, `Created<T>`, `Updated<T>`, `ValidationError`, `ValidationError<T>` |
| **Marker Type Implementation** | Mixed: classes (nested) + structs | All `readonly struct` with `IEquatable<T>` |
| **Target Frameworks** | netstandard2.0 | net8.0, net9.0, net10.0 |

---

## License

[MIT](LICENSE) © [BEN ABT](https://benjamin-abt.com)

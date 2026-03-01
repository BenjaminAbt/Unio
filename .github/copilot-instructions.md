# Unio - GitHub Copilot Instructions

## Project Overview

**Unio** is a high-performance discriminated union library for C#. It provides a `readonly struct`-based core (`Unio<T0, T1>` through `Unio<T0, ..., T19>`) for zero-allocation generic union types, an `UnioBase<...>` abstract class layer for source-generated named unions, a Roslyn incremental source generator, pre-built sentinel and value-carrying types and ASP.NET Core Minimal API integration.

## Architecture & Key Concepts

### Core Union Types

Generic union value types supporting 2 to 20 type parameters:

- `Unio<T0, T1>` through `Unio<T0, T1, ..., T19>`
- All are `readonly struct` - stack-allocated, zero GC pressure
- Backing fields: `byte _index`, `T0? _value0`, `T1? _value1`, etc.
- Implement `IEquatable<Unio<...>>`, `IFormattable`, `ISpanFormattable`, `IUtf8SpanFormattable`

### UnioBase Abstract Classes

Abstract base classes for source-generated named union types, one per arity 2–20:

- `UnioBase<T0, T1>` through `UnioBase<T0, T1, ..., T19>`
- All are `abstract class` - hold a `protected readonly Unio<...> _union` field
- Delegate all union operations (Index, IsT#, AsT#, TryGet#, Match, Switch, etc.) to `_union`
- Source-generated named unions inherit from these and only add: constructor, implicit operators, typed equality

### Package Structure

| Package | Purpose |
|---------|---------|
| `Unio` | Core union types, `UnioBase<...>` abstract base classes, `GenerateUnioAttribute` |
| `Unio.SourceGenerator` | Roslyn incremental source generator for named union types |
| `Unio.Types` | 39 pre-built sentinel (marker) and value-carrying types |
| `Unio.AspNetCore` | `ToHttpResult()` extension for ASP.NET Core Minimal APIs |

## Code Style & Conventions

- Respect the `.editorconfig` settings for consistent formatting
- Use explicit access modifiers (e.g., `public`, `private`) for all members
- All hot-path methods use `[MethodImpl(MethodImplOptions.AggressiveInlining)]`
- Core union types (`Unio<T0,...>`) are `readonly struct` - never add heap allocation to the generic types
- Named union types (source-generated) are `sealed class` inheriting `UnioBase<...>`

### Creating Union Values

Union values are created via implicit operators:

```csharp
// Implicit conversion from any constituent type
Unio<int, string> result = 42;
Unio<int, string> result = "hello";

// 5-arity union
Unio<int, string, bool, double, long> value = true;
```

### Accessing Union Values

```csharp
Unio<int, string> result = 42;

// Index: zero-based position of active type
int index = result.Index;          // 0

// Type checks
bool isInt = result.IsT0;          // true
bool isString = result.IsT1;       // false

// Direct access (throws InvalidOperationException if wrong type)
int intValue = result.AsT0;        // 42
// string s = result.AsT1;         // throws!

// Boxed value
object value = result.Value;       // 42 (boxed)

// Safe access via TryGet
if (result.TryGetT0(out int v))
{
    // v == 42
}

// Fallback via ValueOr
string s = result.ValueOrT1("default"); // "default"
string s = result.ValueOrT1(() => ComputeDefault()); // lazy fallback
```

### Exhaustive Pattern Matching

```csharp
Unio<int, string> result = 42;

// Match: returns a value (all cases must be handled)
string output = result.Match(
    i => $"int: {i}",
    s => $"string: {s}");

// Switch: side-effects only (all cases must be handled)
result.Switch(
    i => Console.WriteLine($"int: {i}"),
    s => Console.WriteLine($"string: {s}"));

// Async variants
string output = await result.MatchAsync(
    async i => await ProcessIntAsync(i),
    async s => await ProcessStringAsync(s));

await result.SwitchAsync(
    async i => await HandleIntAsync(i),
    async s => await HandleStringAsync(s));
```

### Allocation-Free Matching with State

When a lambda captures a local variable the compiler generates a new closure object per call. Use the `Match<TState, TResult>` and `Switch<TState>` overloads with `static` lambdas to avoid this:

```csharp
Unio<int, string> result = 42;
string prefix = "Value";

// ❌ captures `prefix` → allocates a closure object on every call
string output = result.Match(
    i => $"{prefix}: {i}",
    s => $"{prefix}: {s}");

// ✅ passes `prefix` as TState to static lambdas → zero allocation
string output = result.Match(prefix,
    static (p, i) => $"{p}: {i}",
    static (p, s) => $"{p}: {s}");

// Switch<TState> - wrap multiple values in a ValueTuple
result.Switch((prefix, Console.Out),
    static (s, i)   => s.Out.WriteLine($"{s.prefix}: {i}"),
    static (s, str) => s.Out.WriteLine($"{s.prefix}: {str}"));
```

### Mapping

```csharp
Unio<int, string> result = 42;

// Transform one branch while preserving structure
Unio<double, string> mapped = result.MapT0(i => i * 2.0);
```

### Equality & Comparison

```csharp
Unio<int, string> a = 42;
Unio<int, string> b = 42;

bool equal = a == b;         // true
bool notEqual = a != b;      // false
bool eq = a.Equals(b);       // true (IEquatable<T>)

// Comparison operators (for comparable types)
// <, >, <=, >= are supported
```

## Source Generator - Named Unions

The `Unio.SourceGenerator` creates named union types from a `partial class` declaration:

```csharp
using Unio;

[GenerateUnio]
public partial class StringOrInt : IUnio<string, int>;

[GenerateUnio]
public partial class ApiResult : IUnio<Success<User>, NotFound, ValidationError>;
```

The generator produces a `sealed partial class` that **inherits** `UnioBase<...>` - getting all operations for free. It only generates:

- Constructor calling `base(union)`
- Implicit operators from each constituent type
- Typed `IEquatable<T>`: `Equals(T?)`, `Equals(object?)`, `GetHashCode()`
- `==` and `!=` operators
- `[MethodImpl(AggressiveInlining)]` on all generated methods

All other members (`Index`, `IsT0..IsTn`, `AsT0..AsTn`, `TryGetT0..TryGetTn`, `Match<TResult>`, `Match<TState,TResult>`, `Switch`, `Switch<TState>`, `MatchAsync`, `SwitchAsync`, `ValueOrT0..ValueOrTn`, `ToString`, `IFormattable`, `ISpanFormattable`, `IUtf8SpanFormattable`) are **inherited from `UnioBase<...>`**.

### Source Generator Diagnostics

| ID | Severity | Description |
|----|----------|-------------|
| `UNIO001` | Error | Type has `[GenerateUnio]` but does not implement `IUnio<...>` |
| `UNIO002` | Error | Invalid arity (must be 2–20 type parameters) |
| `UNIO003` | Warning | Duplicate type arguments detected |
| `UNIO004` | Info | Class should be `sealed` |

### Marker Interfaces

Used only by the source generator to detect union declarations:

```csharp
public interface IUnio<T0, T1>;                     // 2 types
public interface IUnio<T0, T1, T2>;                  // 3 types
// ... up to ...
public interface IUnio<T0, T1, T2, ..., T19>;        // 20 types
```

## Pre-built Types (Unio.Types)

### Marker Types (zero-size sentinels)

All implement `IEquatable<T>`, `==`, `!=` and `GetHashCode() → 0`:

| Category | Types |
|----------|-------|
| **Boolean/Ternary** | `Yes`, `No`, `Maybe`, `True`, `False`, `Unknown` |
| **Collection** | `All`, `Some`, `None`, `Empty` |
| **State** | `Pending`, `Cancelled`, `Timeout`, `Skipped`, `Invalid`, `Disabled`, `Expired`, `RateLimited` |
| **HTTP/API** | `NotFound`, `Forbidden`, `Unauthorized`, `Conflict`, `BadRequest`, `Accepted`, `NoContent` |
| **CRUD** | `Created`, `Updated`, `Deleted`, `Unchanged` |
| **Result** | `Success`, `Error` |

### Value-Carrying Types (generic wrappers)

All have `T Value { get; }`, implement `IEquatable<T>`, support `implicit operator` from `T`:

- `Success<T>`, `Error<T>`, `Result<T>`, `NotFound<T>`, `Created<T>`, `Updated<T>`
- `ValidationError` (string message), `ValidationError<T>` (typed details)

Usage example:
```csharp
Unio<Success<User>, NotFound, ValidationError> result = new Success<User>(user);

string output = result.Match(
    success => $"Created user: {success.Value.Name}",
    notFound => "User not found",
    error => $"Invalid: {error.Value}");
```

## ASP.NET Core Integration

The `Unio.AspNetCore` package provides `ToHttpResult()` for Minimal APIs:

```csharp
app.MapGet("/users/{id}", (int id) =>
{
    Unio<Success<User>, NotFound, BadRequest> result = GetUser(id);
    return result.ToHttpResult();
});
```

HTTP status mapping:
- `BadRequest` → 400, `Unauthorized` → 401, `Forbidden` → 403, `NotFound` → 404, `Conflict` → 409
- `Created` / `Created<T>` → 201, `Accepted` → 202, `NoContent` → 204
- All other types → 200 OK with value in body

## Testing Conventions

- Tests use **xUnit v3** (3.0.1) - assertions only, no FluentAssertions
- Test files follow naming pattern: `Unio{Arity}Tests.cs` (e.g., `Unio2Tests.cs`, `Unio3Tests.cs`)
- Source generator tests: `GeneratedUnio2Tests.cs`, `GeneratedHighArityTests.cs`
- Use descriptive test method names that explain the scenario

Example test structure:
```csharp
public class Unio2Tests
{
    [Fact]
    public void Match_WithT0_CallsCorrectBranch()
    {
        // Arrange
        Unio<int, string> union = 42;

        // Act
        string result = union.Match(
            i => $"int:{i}",
            s => $"str:{s}");

        // Assert
        Assert.Equal("int:42", result);
    }
}
```

Test coverage areas per arity:
- Implicit conversion and `Index`/`IsT#` properties
- `AsT#` success and failure (InvalidOperationException)
- `TryGetT#` true/false paths
- `Match`/`Switch` correct branch execution
- `MatchAsync`/`SwitchAsync` async variants
- `Equals`, `GetHashCode`, `==`, `!=`
- `ToString`, `IFormattable`, `ISpanFormattable`

## Build & Development

### Prerequisites
- .NET SDK 8.0, 9.0 or 10.0
- Just command runner (optional, for convenience commands)

### Common Commands

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run tests with coverage
just test-cov

# Run benchmarks
dotnet run --project perf/Unio.Benchmarks/Unio.Benchmarks.csproj -c Release

# Using Just
just build
just test
just bench
just format          # Apply code formatting
just format-check    # Verify formatting
just pack            # Pack NuGet packages
just ci              # Full CI: clean → restore → format-check → build → test-cov
```

### Target Frameworks
- .NET 8.0
- .NET 9.0
- .NET 10.0

## Important Design Decisions

1. **`readonly struct` for core types** - Stack-allocated value semantics; `Unio<T0,...>` types are instantiated via private constructors and implicit operators, with zero heap allocation
2. **`UnioBase<...>` abstract classes** - Named unions (source-generated) inherit from these to get class semantics and reference identity while reusing all union logic
3. **Sealed Named Unions** - Source-generated classes are `sealed` to prevent unintended subclassing and ensure correct union semantics
3. **Implicit Operators** - Ergonomic union creation (`Unio<int, string> x = 42;`) is a core design goal
4. **Exhaustive Matching** - `Match` and `Switch` require handlers for all types, ensuring compile-time safety
5. **TryGet Pattern** - Safe access without exceptions via `TryGetT0(out T value)`
6. **Source Generator over Inheritance** - Named unions wrap `Unio<...>` via delegation, preserving class semantics
7. **AggressiveInlining** - All public methods use `[MethodImpl(AggressiveInlining)]` for performance
8. **readonly fields** - All backing fields are `readonly` for immutability after construction
9. **IEquatable<T>** - Structural equality with null-safe `Equals(T? other)` implementation
10. **IFormattable / ISpanFormattable / IUtf8SpanFormattable** - Full formatting support for all union types

## File Organization

```
src/
├── Unio/                        # Core library
│   ├── Unio{N}.Generated.cs     # Union types as readonly structs (Unio2 through Unio20)
│   ├── UnioBase.Generated.cs    # Abstract base classes for named unions (UnioBase2 through UnioBase20)
│   └── GenerateUnioAttribute.cs # [GenerateUnio] attribute
├── Unio.SourceGenerator/        # Roslyn incremental generator
│   ├── UnioGenerator.cs         # IIncrementalGenerator implementation
│   └── Diagnostics.cs           # UNIO001–UNIO004 diagnostic descriptors
├── Unio.Types/                  # Pre-built sentinel and value types
│   ├── Markers.cs               # Zero-size marker types (Success, NotFound, etc.)
│   └── ValueTypes.cs            # Value-carrying types (Success<T>, Error<T>, etc.)
└── Unio.AspNetCore/             # ASP.NET Core integration
    └── MinimalApi/              # ToHttpResult() extension methods

tests/
├── Unio.UnitTests/              # Core union type tests
├── Unio.Types.UnitTests/        # Marker and value type tests
├── Unio.SourceGenerator.UnitTests/ # Source generator tests
└── Unio.AspNetCore.UnitTests/   # ASP.NET Core integration tests

perf/
└── Unio.Benchmarks/             # BenchmarkDotNet performance benchmarks

tools/
└── Unio.CodeGen/                # Code generation tooling
```

## Guidelines (MANDATORY)

1. Ensure all tests pass with `dotnet test`
2. Check code formatting with `dotnet format --verify-no-changes`
3. Add code documentation comments and add / update XML docs as needed
4. Add tests for new functionality
5. Update benchmarks if performance-critical code is changed
6. Maintain backward compatibility
7. Follow existing naming conventions and code style
8. Ensure no errors or warnings in the build
9. Verify all build and test commands succeed
10. Fix any new warnings or errors introduced by changes

# Unio.Types

Pre-built, high-performance sentinel and value types for use with [Unio](https://github.com/BenjaminAbt/Unio) discriminated unions.

## Usage

Targets .NET 9, .NET 10 and .NET 11 RC1. Compatible with Native AOT and trimming; reference AOT metadata is verified on .NET 10 and newer. Serialization of wrapped application values still requires metadata for those types.

```csharp
using Unio;
using Unio.Types;

// Use marker types in discriminated unions
Unio<User, NotFound> GetUser(int id) => ...;
Unio<Order, Forbidden, NotFound> GetOrder(int id) => ...;
Unio<Success, ValidationError> Validate(Request req) => ...;

// Use value-carrying types for rich results
Unio<Success<Order>, Error<string>> CreateOrder(OrderRequest req) => ...;
Unio<Created<int>, ValidationError<string[]>> SaveUser(UserDto dto) => ...;
```

## Included Types

### Markers (empty sentinel structs)
`Yes`, `No`, `Maybe`, `True`, `False`, `Unknown`, `All`, `Some`, `None`, `Empty`, `Pending`, `Cancelled`, `Timeout`, `Skipped`, `Invalid`, `NotFound`, `Forbidden`, `Unauthorized`, `Conflict`, `BadRequest`, `Accepted`, `NoContent`, `Created`, `Updated`, `Deleted`, `Unchanged`, `Success`, `Error`, `Disabled`, `Expired`, `RateLimited`

### Value Carriers (structs with `Value` property)
`Success<T>`, `Error<T>`, `Result<T>`, `NotFound<T>`, `Created<T>`, `Updated<T>`, `ValidationError` (string Message), `ValidationError<T>`

All types are `readonly struct` with `IEquatable<T>`, `==`/`!=` operators and `AggressiveInlining` on hot paths.

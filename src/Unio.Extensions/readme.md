# Unio.Extensions

Functional extensions for `Unio<T0, T1>` to support fluent composition and error-recovery style pipelines.

## Features

- `BindT0` / `BindT1` for branch-specific monadic composition
- `TapT0` / `TapT1` for side-effects without value mutation
- `RecoverT1` to collapse `Unio<T0, T1>` into `T0`
- `Fold` as a semantic alias for reduction to a shared result type

## Example

```csharp
using Unio;
using Unio.Extensions;

Unio<int, string> value = "invalid";

int normalized = value
    .TapT1(static error => Console.WriteLine(error))
    .RecoverT1(static _ => -1);
```

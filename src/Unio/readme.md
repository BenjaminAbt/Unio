# Unio

High-performance discriminated unions for C#. Designed with strongly typed generic fields, full value equality semantics and allocation-free pattern matching for maximum performance and type safety.

## Usage

Targets .NET 9, .NET 10 and .NET 11 RC1. The core is compatible with Native AOT and trimming, including `UnioBase<...>` and source-generated named unions. It uses no runtime code generation or reflection.

The package enables `IsAotCompatible` and verifies reference AOT metadata on .NET 10 and newer. See the [Native AOT examples and validation](https://github.com/BenjaminAbt/Unio#native-aot) for publish commands.

Union storage and typed matching do not allocate. Accessing `Value` boxes value types; named union classes, capturing delegates and some formatting paths can allocate.

```csharp
using Unio;

// Implicit conversion
Unio<int, string> result = 42;
Unio<int, string> error = "not found";

// Exhaustive matching
string text = result.Match(
    i => $"Number: {i}",
    s => $"Text: {s}");

// Allocation-free matching - pass state instead of capturing variables
// (no closure object allocated per call)
string prefix = "Result";
string text2 = result.Match(prefix,
    static (p, i) => $"{p}: {i}",
    static (p, s) => $"{p}: {s}");

// Allocation-free side-effect switch
result.Switch((prefix, Console.Out),
    static (s, i) => s.Out.WriteLine($"{s.prefix}: {i}"),
    static (s, str) => s.Out.WriteLine($"{s.prefix}: {str}"));

// Safe TryGet pattern
if (result.TryGetT0(out int number))
    Console.WriteLine(number);
```

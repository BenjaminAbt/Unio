# Unio

High-performance discriminated unions for C#. Designed with strongly typed generic fields, full value equality semantics and allocation-free pattern matching for maximum performance and type safety.

## Usage

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

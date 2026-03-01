# Unio.SourceGenerator

Roslyn incremental source generator for [Unio](https://github.com/BenjaminAbt/Unio) discriminated unions.

Generates named, strongly-typed union wrappers from a simple declaration:

```csharp
using Unio;

[GenerateUnio]
public partial struct StringOrInt : IUnio<string, int>;
```

The generator produces the full union API: `IsT0`/`IsT1`, `AsT0`/`AsT1`, `TryGetT0`/`TryGetT1`, `Match`, `Switch`, implicit operators, equality and `ToString()`.

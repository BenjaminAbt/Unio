# Unio.SourceGenerator

Roslyn incremental source generator for [Unio](https://github.com/BenjaminAbt/Unio) discriminated unions.

Generates named, strongly-typed union wrappers from a simple declaration:

```csharp
using Unio;

[GenerateUnio]
public partial class StringOrInt : UnioBase<string, int>;
```

The generator adds constructors, implicit conversions and typed equality to a sealed partial class. The union API is inherited from `UnioBase<...>`.

Use a top-level, non-generic partial class with 2–20 distinct branch types. Nested, generic, abstract, static and file-local classes receive `UNIO005`; missing `partial` receives `UNIO006`. Duplicate branch types receive `UNIO003` as an error.

## Native AOT and requirements

The generated code supports Native AOT and trimming without runtime reflection or dynamic code. The generator itself runs inside Roslyn at build time and is packaged only as an analyzer; it is not a runtime dependency and does not need `IsAotCompatible`.

The analyzer targets `netstandard2.0` for compiler-host compatibility and requires a Roslyn 5.0-compatible compiler (.NET 10 SDK or newer). This does not add older .NET runtime support: the Unio runtime packages target .NET 9, .NET 10 and .NET 11 RC1.

// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio;

/// <summary>
/// Marks a partial class for source generation of a named discriminated union type.
/// The class must inherit <c>UnioBase&lt;T0, T1, ...&gt;</c> to define the union's type parameters.
/// </summary>
/// <example>
/// <code>
/// [GenerateUnio]
/// public partial class StringOrInt : UnioBase&lt;string, int&gt;;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GenerateUnioAttribute : Attribute;

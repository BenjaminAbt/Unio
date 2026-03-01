// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.CodeAnalysis;

namespace Unio.SourceGenerator;

/// <summary>
/// Contains <see cref="DiagnosticDescriptor"/> definitions for all diagnostics reported
/// by the <see cref="UnioGenerator"/> source generator.
/// </summary>
internal static class Diagnostics
{
    /// <summary>
    /// Reported when a type is marked with <c>[GenerateUnio]</c> but does not inherit
    /// from <c>UnioBase&lt;...&gt;</c>.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingUnioBase = new(
        id: "UNIO001",
        title: "Missing UnioBase<...> base class",
        messageFormat: "Type '{0}' is marked with [GenerateUnio] but does not inherit from UnioBase<...>",
        category: "Unio.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    /// <summary>
    /// Reported when a type inherits <c>UnioBase&lt;...&gt;</c> with a number of type arguments
    /// outside the supported range (2–20).
    /// </summary>
    public static readonly DiagnosticDescriptor InvalidArity = new(
        id: "UNIO002",
        title: "Invalid union arity",
        messageFormat: "Type '{0}' inherits UnioBase<...> with {1} type arguments, but only 2-20 are supported",
        category: "Unio.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    /// <summary>
    /// Reported when a type inherits <c>UnioBase&lt;...&gt;</c> with duplicate type arguments,
    /// which makes the union ambiguous because implicit conversions would conflict.
    /// </summary>
    public static readonly DiagnosticDescriptor DuplicateTypeArguments = new(
        id: "UNIO003",
        title: "Duplicate type arguments in union",
        messageFormat: "Type '{0}' has duplicate type argument '{1}' in UnioBase<...> which makes the union ambiguous",
        category: "Unio.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    /// <summary>
    /// Reported when a class marked with <c>[GenerateUnio]</c> is not declared as <c>sealed</c>,
    /// which is recommended to prevent subclassing and ensure correct union semantics.
    /// </summary>
    public static readonly DiagnosticDescriptor NotSealed = new(
        id: "UNIO004",
        title: "Union class should be sealed",
        messageFormat: "Class '{0}' should be declared as 'sealed' for correct union semantics",
        category: "Unio.SourceGenerator",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);
}

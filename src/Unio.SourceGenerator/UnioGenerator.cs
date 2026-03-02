// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Unio.SourceGenerator;

/// <summary>
/// Incremental source generator that generates named discriminated union types
/// from partial classes marked with [GenerateUnio] inheriting from <c>UnioBase&lt;...&gt;</c>.
/// </summary>
[Generator]
public sealed class UnioGenerator : IIncrementalGenerator
{
    private const string GenerateUnioAttributeFullName = "Unio.GenerateUnioAttribute";
    private const string UnioBasePrefix = "Unio.UnioBase";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Filter for class declarations with [GenerateUnio] attribute
        IncrementalValuesProvider<ClassDeclarationSyntax> typeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsCandidate(node),
                transform: static (ctx, _) => GetSemanticTarget(ctx))
            .Where(static s => s is not null)!;

        // Combine with compilation
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndTypes =
            context.CompilationProvider.Combine(typeDeclarations.Collect());

        // Generate source
        context.RegisterSourceOutput(compilationAndTypes,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    /// <summary>Fast syntactic filter: partial class with at least one attribute and a base list.</summary>
    private static bool IsCandidate(SyntaxNode node) =>
        node is ClassDeclarationSyntax cds
        && cds.AttributeLists.Count > 0
        && cds.BaseList is not null
        && cds.Modifiers.Any(SyntaxKind.PartialKeyword);

    /// <summary>Semantic filter: returns the class syntax node if it carries the <c>[GenerateUnio]</c> attribute; otherwise <c>null</c>.</summary>
    private static ClassDeclarationSyntax? GetSemanticTarget(GeneratorSyntaxContext context)
    {
        ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)context.Node;
        INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(classSyntax);
        if (symbol is null)
        {
            return null;
        }

        foreach (AttributeData attr in symbol.GetAttributes())
        {
            if (attr.AttributeClass?.ToDisplayString() == GenerateUnioAttributeFullName)
            {
                return classSyntax;
            }
        }

        return null;
    }

    /// <summary>
    /// Iterates over all candidate classes, resolves their <c>UnioBase&lt;...&gt;</c> base type,
    /// validates arity and emits the generated source for each valid union type.
    /// </summary>
    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> types, SourceProductionContext context)
    {
        if (types.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (ClassDeclarationSyntax classSyntax in types.Distinct())
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(classSyntax.SyntaxTree);
            INamedTypeSymbol? structSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
            if (structSymbol is null)
            {
                continue;
            }

            // Find the UnioBase<...> base type
            INamedTypeSymbol? unioBase = structSymbol.BaseType;
            if (unioBase is null || !unioBase.OriginalDefinition.ToDisplayString().StartsWith(UnioBasePrefix, StringComparison.Ordinal))
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Diagnostics.MissingUnioBase,
                    classSyntax.Identifier.GetLocation(),
                    structSymbol.Name));
                continue;
            }

            ImmutableArray<ITypeSymbol> typeArgs = unioBase.TypeArguments;
            if (typeArgs.Length < 2 || typeArgs.Length > 20)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Diagnostics.InvalidArity,
                    classSyntax.Identifier.GetLocation(),
                    structSymbol.Name,
                    typeArgs.Length));
                continue;
            }

            // Check for duplicate type arguments
            HashSet<string> seenTypes = new();
            foreach (ITypeSymbol typeArg in typeArgs)
            {
                string typeName = typeArg.ToDisplayString();
                if (!seenTypes.Add(typeName))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Diagnostics.DuplicateTypeArguments,
                        classSyntax.Identifier.GetLocation(),
                        structSymbol.Name,
                        typeName));
                }
            }

            string source = GenerateUnionClass(structSymbol, typeArgs);
            context.AddSource($"{structSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)).Replace(".", "_")}.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    /// <summary>
    /// Builds the complete C# source code for a named discriminated union class.
    /// All union operations are inherited from <c>UnioBase&lt;...&gt;</c> (declared by the user);
    /// the generated code only provides the constructor, implicit conversion operators and typed equality members.
    /// </summary>
    /// <param name="structSymbol">The symbol of the partial class to generate code for.</param>
    /// <param name="typeArgs">The resolved type arguments from the <c>UnioBase&lt;...&gt;</c> base type.</param>
    /// <returns>The generated C# source code as a string.</returns>
    private static string GenerateUnionClass(INamedTypeSymbol structSymbol, ImmutableArray<ITypeSymbol> typeArgs)
    {
        StringBuilder sb = new StringBuilder();
        string className = structSymbol.Name;
        string? ns = structSymbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : structSymbol.ContainingNamespace.ToDisplayString();
        int arity = typeArgs.Length;
        string accessibility = structSymbol.DeclaredAccessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Internal => "internal",
            Accessibility.Private => "private",
            Accessibility.Protected => "protected",
            Accessibility.ProtectedOrInternal => "protected internal",
            Accessibility.ProtectedAndInternal => "private protected",
            _ => "internal"
        };

        // Fully-qualified type names to avoid ambiguity
        string[] typeNamesGlobal = new string[arity];
        string[] typeNamesShort = new string[arity];
        for (int i = 0; i < arity; i++)
        {
            typeNamesGlobal[i] = typeArgs[i].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            typeNamesShort[i] = typeArgs[i].ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        }

        string genericArgs = string.Join(", ", typeNamesGlobal);
        string unioType = $"global::Unio.Unio<{genericArgs}>";
        string unioBaseType = $"global::Unio.UnioBase<{genericArgs}>";

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        if (ns is not null)
        {
            sb.AppendLine($"namespace {ns};");
            sb.AppendLine();
        }

        // Class declaration - adds IEquatable<T>; UnioBase<...> is already declared in the user's partial.
        sb.AppendLine($"{accessibility} sealed partial class {className} : global::System.IEquatable<{className}>");
        sb.AppendLine("{");

        // Private constructor (called by implicit operators)
        sb.AppendLine($"    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine($"    private {className}({unioType} union) : base(union) {{ }}");
        sb.AppendLine();

        // Public typed constructors - one per type argument
        for (int i = 0; i < arity; i++)
        {
            string tn = typeNamesGlobal[i];
            string tnShort = typeNamesShort[i];
            sb.AppendLine($"    /// <summary>Initializes a new instance of <see cref=\"{className}\"/> holding a value of type <see cref=\"{tnShort}\"/>.</summary>");
            sb.AppendLine($"    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine($"    public {className}({tn} value) : base(({unioType})value) {{ }}");
            sb.AppendLine();
        }

        // Implicit conversion operators - one per type argument
        for (int i = 0; i < arity; i++)
        {
            string tn = typeNamesGlobal[i];
            string tnShort = typeNamesShort[i];
            sb.AppendLine($"    /// <summary>Implicitly converts a value of type <see cref=\"{tnShort}\"/> to <see cref=\"{className}\"/>.</summary>");
            sb.AppendLine($"    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
            sb.AppendLine($"    public static implicit operator {className}({tn} value) => new(({unioType})value);");
            sb.AppendLine();
        }

        // Typed equality - IEquatable<ClassName>
        sb.AppendLine($"    /// <inheritdoc/>");
        sb.AppendLine($"    [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        sb.AppendLine($"    public bool Equals({className}? other) => other is not null && _union.Equals(other._union);");
        sb.AppendLine();

        sb.AppendLine($"    /// <inheritdoc/>");
        sb.AppendLine($"    public override bool Equals(object? obj) => obj is {className} other && Equals(other);");
        sb.AppendLine();

        sb.AppendLine($"    /// <inheritdoc/>");
        sb.AppendLine($"    public override int GetHashCode() => _union.GetHashCode();");
        sb.AppendLine();

        sb.AppendLine($"    /// <summary>Equality operator.</summary>");
        sb.AppendLine($"    public static bool operator ==({className}? left, {className}? right) => left is null ? right is null : left.Equals(right);");
        sb.AppendLine();

        sb.AppendLine($"    /// <summary>Inequality operator.</summary>");
        sb.AppendLine($"    public static bool operator !=({className}? left, {className}? right) => !(left == right);");

        sb.AppendLine("}");

        return sb.ToString();
    }
}

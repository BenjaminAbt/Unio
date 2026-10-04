// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Unio.SourceGenerator.UnitTests;

public class GeneratorDiagnosticsTests
{
    [Theory]
    [InlineData("[Unio.GenerateUnio] public partial class Missing;", "UNIO001")]
    [InlineData("[Unio.GenerateUnio] public class NonPartial : Unio.UnioBase<int, string>;", "UNIO006")]
    [InlineData("[Unio.GenerateUnio] public partial class Generic<T> : Unio.UnioBase<T, string>;", "UNIO005")]
    [InlineData("public partial class Outer { [Unio.GenerateUnio] public partial class Nested : Unio.UnioBase<int, string>; }", "UNIO005")]
    [InlineData("[Unio.GenerateUnio] public partial class Duplicate : Unio.UnioBase<string, string?>;", "UNIO003")]
    [InlineData("namespace Unio { public class UnioBaseFake<T, U>; [GenerateUnio] public partial class Wrong : UnioBaseFake<int, string>; }", "UNIO001")]
    public void InvalidDeclarations_ReportDiagnosticWithoutGeneratingBrokenCode(string source, string diagnosticId)
    {
        GeneratorDriverRunResult result = Run(source, out _);

        Assert.Contains(result.Diagnostics, diagnostic => string.Equals(diagnostic.Id, diagnosticId, StringComparison.Ordinal) && diagnostic.Severity == DiagnosticSeverity.Error);
        Assert.Empty(result.GeneratedTrees);
        Assert.Null(Assert.Single(result.Results).Exception);
    }

    [Fact]
    public void PartialDeclarations_AreGeneratedOnce()
    {
        const string source = """
            [Unio.GenerateUnio] public partial class Result : Unio.UnioBase<int, string>;
            [System.Obsolete] public partial class Result : Unio.UnioBase<int, string>;
            """;
        GeneratorDriverRunResult result = Run(source, out Compilation compilation);

        Assert.Single(result.GeneratedTrees);
        Assert.Empty(result.Diagnostics);
        Assert.DoesNotContain(compilation.GetDiagnostics(TestContext.Current.CancellationToken), static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void NamespaceAndKeywordNames_DoNotCollide()
    {
        const string source = """
            namespace A.B { [Unio.GenerateUnio] public partial class C : Unio.UnioBase<int, string>; }
            namespace A_B { [Unio.GenerateUnio] public partial class C : Unio.UnioBase<int, string>; }
            namespace Keywords { [Unio.GenerateUnio] public partial class @class : Unio.UnioBase<int, string>; }
            """;
        GeneratorDriverRunResult result = Run(source, out Compilation compilation);

        Assert.Empty(result.Diagnostics);
        Assert.Equal(3, result.GeneratedTrees.Length);
        Assert.DoesNotContain(compilation.GetDiagnostics(TestContext.Current.CancellationToken), static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
    }

    private static GeneratorDriverRunResult Run(string source, out Compilation output)
    {
        string[] assemblyPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        IEnumerable<MetadataReference> references = assemblyPaths.Select(static path => MetadataReference.CreateFromFile(path))
            .Append(MetadataReference.CreateFromFile(typeof(GenerateUnioAttribute).Assembly.Location));
        CSharpParseOptions parseOptions = new(LanguageVersion.Preview);
        CSharpCompilation compilation = CSharpCompilation.Create(
            "GeneratorTest",
            [CSharpSyntaxTree.ParseText(source, parseOptions, cancellationToken: TestContext.Current.CancellationToken)],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        GeneratorDriver driver = CSharpGeneratorDriver.Create([new UnioGenerator().AsSourceGenerator()], parseOptions: parseOptions);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out output, out _, TestContext.Current.CancellationToken);
        return driver.GetRunResult();
    }
}

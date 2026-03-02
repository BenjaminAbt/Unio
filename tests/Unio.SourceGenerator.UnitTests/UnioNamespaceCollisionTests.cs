// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

// Regression test for: generated code references `global::Unio.Unio<>` and
// `global::Unio.UnioBase<>`. Without `global::`, a project with a namespace segment
// named "Unio" (e.g. My.Unio.Features) would cause the compiler to resolve `Unio.Unio<>`
// as `My.Unio.Unio<>` — which does not exist — resulting in a CS0246/CS0234 error.
// The generator already emits `global::Unio.*` for these references, so this test
// acts as a permanent guard against any future regression.

using Unio;

// A namespace whose hierarchy contains an "Unio" segment intentionally simulates
// projects with namespaces like My.Unio.Domain.* or Company.Unio.Core.*
namespace My.Unio.Features;

/// <summary>
/// Union type declared inside a namespace containing an "Unio" segment.
/// If the generator ever emits a bare <c>Unio.Unio&lt;&gt;</c> or <c>Unio.UnioBase&lt;&gt;</c>
/// reference (without <c>global::</c>), this type will fail to compile (CS0234/CS0246).
/// </summary>
[GenerateUnio]
public partial class UnioNamespaceResult : UnioBase<string, int>;

/// <summary>
/// Verifies that <see cref="UnioNamespaceResult"/> (declared in a namespace containing
/// an "Unio" segment) compiles and operates correctly.
/// </summary>
public class UnioNamespaceCollisionTests
{
    [Fact]
    public void ImplicitConversion_T0_WorksInUnioNamespace()
    {
        UnioNamespaceResult union = "hello";

        Assert.Equal(0, union.Index);
        Assert.True(union.IsT0);
        Assert.False(union.IsT1);
    }

    [Fact]
    public void ImplicitConversion_T1_WorksInUnioNamespace()
    {
        UnioNamespaceResult union = 42;

        Assert.Equal(1, union.Index);
        Assert.False(union.IsT0);
        Assert.True(union.IsT1);
    }

    [Fact]
    public void Match_WorksInUnioNamespace()
    {
        UnioNamespaceResult union = "test";

        string result = union.Match(
            s => $"str:{s}",
            i => $"int:{i}");

        Assert.Equal("str:test", result);
    }

    [Fact]
    public void Equals_WorksInUnioNamespace()
    {
        UnioNamespaceResult a = "hello";
        UnioNamespaceResult b = "hello";

        Assert.True(a == b);
        Assert.Equal(a, b);
    }
}

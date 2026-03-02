// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

// Regression test for: generated code using bare `System.*` references would fail to compile
// when the consuming project has a user-defined namespace segment named "System"
// (e.g. MyCSharp.Portal.Features.System.Whatever), because the C# compiler would resolve
// `System.Runtime` to `<parent>.System.Runtime` instead of the global `System.Runtime`.
// Fix: all references in generated code now use `global::System.*`.

using System.Globalization;
using Unio;

// A namespace whose hierarchy contains a "System" segment intentionally simulates
// projects like MyCSharp.Portal.Features.System.*
namespace My.System.Features;

/// <summary>
/// Union type declared inside a namespace containing a "System" segment.
/// If the generator emits bare <c>System.Runtime.CompilerServices.MethodImpl</c>
/// without <c>global::</c>, this type will fail to compile (CS0234).
/// </summary>
[GenerateUnio]
public partial class SystemNamespaceResult : UnioBase<string, int>;

/// <summary>
/// Verifies that <see cref="SystemNamespaceResult"/> (declared in a namespace containing
/// a &quot;System&quot; segment) compiles and operates correctly.
/// </summary>
public class SystemNamespaceCollisionTests
{
    [Fact]
    public void ImplicitConversion_T0_WorksInSystemNamespace()
    {
        SystemNamespaceResult union = "hello";

        Assert.Equal(0, union.Index);
        Assert.True(union.IsT0);
        Assert.False(union.IsT1);
    }

    [Fact]
    public void ImplicitConversion_T1_WorksInSystemNamespace()
    {
        SystemNamespaceResult union = 42;

        Assert.Equal(1, union.Index);
        Assert.False(union.IsT0);
        Assert.True(union.IsT1);
    }

    [Fact]
    public void Match_WorksInSystemNamespace()
    {
        SystemNamespaceResult union = "test";

        string result = union.Match(
            s => $"str:{s}",
            i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"));

        Assert.Equal("str:test", result);
    }

    [Fact]
    public void Equals_WorksInSystemNamespace()
    {
        SystemNamespaceResult a = "hello";
        SystemNamespaceResult b = "hello";

        Assert.True(a == b);
        Assert.Equal(a, b);
    }
}

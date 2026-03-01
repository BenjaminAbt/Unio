// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.SourceGenerator.UnitTests;

/// <summary>
/// Unit tests for the source-generated <see cref="StringOrInt"/> (2-arity) union struct
/// verifying that the generated code delegates correctly to the underlying <c>Unio&lt;string, int&gt;</c>.
/// </summary>
public class GeneratedUnio2Tests
{
    [Fact]
    public void ImplicitConversion_T0_SetsIndexTo0()
    {
        StringOrInt union = "hello";

        Assert.Equal(0, union.Index);
        Assert.True(union.IsT0);
        Assert.False(union.IsT1);
    }

    [Fact]
    public void ImplicitConversion_T1_SetsIndexTo1()
    {
        StringOrInt union = 42;

        Assert.Equal(1, union.Index);
        Assert.False(union.IsT0);
        Assert.True(union.IsT1);
    }

    [Fact]
    public void AsT0_WhenT0_ReturnsValue()
    {
        StringOrInt union = "hello";

        Assert.Equal("hello", union.AsT0);
    }

    [Fact]
    public void AsT0_WhenT1_Throws()
    {
        StringOrInt union = 42;

        Assert.Throws<InvalidOperationException>(() => union.AsT0);
    }

    [Fact]
    public void AsT1_WhenT1_ReturnsValue()
    {
        StringOrInt union = 42;

        Assert.Equal(42, union.AsT1);
    }

    [Fact]
    public void TryGetT0_WhenT0_ReturnsTrue()
    {
        StringOrInt union = "hello";

        Assert.True(union.TryGetT0(out string? value));
        Assert.Equal("hello", value);
    }

    [Fact]
    public void TryGetT0_WhenT1_ReturnsFalse()
    {
        StringOrInt union = 42;

        Assert.False(union.TryGetT0(out _));
    }

    [Fact]
    public void TryGetT1_WhenT1_ReturnsTrue()
    {
        StringOrInt union = 42;

        Assert.True(union.TryGetT1(out int value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Value_ReturnsCurrentValue()
    {
        StringOrInt unionStr = "hello";
        StringOrInt unionInt = 42;

        Assert.Equal("hello", unionStr.Value);
        Assert.Equal(42, unionInt.Value);
    }

    [Fact]
    public void Match_WhenT0_CallsFirstFunc()
    {
        StringOrInt union = "hello";

        string result = union.Match(
            s => $"str:{s}",
            i => $"int:{i}");

        Assert.Equal("str:hello", result);
    }

    [Fact]
    public void Match_WhenT1_CallsSecondFunc()
    {
        StringOrInt union = 42;

        string result = union.Match(
            s => $"str:{s}",
            i => $"int:{i}");

        Assert.Equal("int:42", result);
    }

    [Fact]
    public void Match_WithState_WhenT1_CallsSecondFunc()
    {
        StringOrInt union = 42;

        string result = union.Match(
            "prefix",
            static (state, s) => $"{state}:str:{s}",
            static (state, i) => $"{state}:int:{i}");

        Assert.Equal("prefix:int:42", result);
    }

    [Fact]
    public void Switch_WhenT0_CallsFirstAction()
    {
        StringOrInt union = "hello";
        string? captured = null;

        union.Switch(
            s => captured = s,
            _ => { });

        Assert.Equal("hello", captured);
    }

    [Fact]
    public void Switch_WithState_WhenT1_CallsSecondAction()
    {
        StringOrInt union = 42;
        int[] state = new int[] { 10, 0 };

        union.Switch(
            state,
            static (_, _) => { },
            static (s, i) => s[1] = i + s[0]);

        Assert.Equal(52, state[1]);
    }

    [Fact]
    public void Equality_SameTypeAndValue_AreEqual()
    {
        StringOrInt a = "hello";
        StringOrInt b = "hello";

        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equality_DifferentValues_AreNotEqual()
    {
        StringOrInt a = "hello";
        StringOrInt b = "world";

        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equality_DifferentTypes_AreNotEqual()
    {
        StringOrInt a = "42";
        StringOrInt b = 42;

        Assert.False(a == b);
    }

    [Fact]
    public void GetHashCode_SameValues_AreSame()
    {
        StringOrInt a = 42;
        StringOrInt b = 42;

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        StringOrInt unionStr = "hello";
        StringOrInt unionInt = 42;

        Assert.Equal("hello", unionStr.ToString());
        Assert.Equal("42", unionInt.ToString());
    }

    [Fact]
    public void Equals_WithObject_Works()
    {
        StringOrInt a = 42;
        object b = (StringOrInt)42;

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        StringOrInt a = 42;

        Assert.False(a.Equals((object?)null));
    }
}

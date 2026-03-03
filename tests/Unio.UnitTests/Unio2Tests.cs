// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1}"/> covering implicit conversions, property access,
/// <c>TryGet</c>, <c>Match</c>, <c>Switch</c>, equality, hashing and <c>ToString</c>.
/// </summary>
public class Unio2Tests
{
    [Fact]
    public void ImplicitConversion_T0_SetsIndexTo0()
    {
        Unio<int, string> union = 42;

        Assert.Equal(0, union.Index);
        Assert.True(union.IsT0);
        Assert.False(union.IsT1);
    }

    [Fact]
    public void ImplicitConversion_T1_SetsIndexTo1()
    {
        Unio<int, string> union = "hello";

        Assert.Equal(1, union.Index);
        Assert.False(union.IsT0);
        Assert.True(union.IsT1);
    }

    [Fact]
    public void AsT0_WhenT0_ReturnsValue()
    {
        Unio<int, string> union = 42;

        Assert.Equal(42, union.AsT0);
    }

    [Fact]
    public void AsT0_WhenT1_Throws()
    {
        Unio<int, string> union = "hello";

        Assert.Throws<InvalidOperationException>(() => union.AsT0);
    }

    [Fact]
    public void AsT1_WhenT1_ReturnsValue()
    {
        Unio<int, string> union = "hello";

        Assert.Equal("hello", union.AsT1);
    }

    [Fact]
    public void AsT1_WhenT0_Throws()
    {
        Unio<int, string> union = 42;

        Assert.Throws<InvalidOperationException>(() => union.AsT1);
    }

    [Fact]
    public void TryGetT0_WhenT0_ReturnsTrue()
    {
        Unio<int, string> union = 42;

        Assert.True(union.TryGetT0(out int value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetT0_WhenT1_ReturnsFalse()
    {
        Unio<int, string> union = "hello";

        Assert.False(union.TryGetT0(out _));
    }

    [Fact]
    public void TryGetT1_WhenT1_ReturnsTrue()
    {
        Unio<int, string> union = "hello";

        Assert.True(union.TryGetT1(out string? value));
        Assert.Equal("hello", value);
    }

    [Fact]
    public void TryGetT1_WhenT0_ReturnsFalse()
    {
        Unio<int, string> union = 42;

        Assert.False(union.TryGetT1(out _));
    }

    [Fact]
    public void TryPickT0_WhenT0_ReturnsTrueAndDefaultRemainder()
    {
        Unio<int, string> union = 42;

        Assert.True(union.TryPickT0(out int value, out string? remainder));
        Assert.Equal(42, value);
        Assert.Null(remainder);
    }

    [Fact]
    public void TryPickT0_WhenT1_ReturnsFalseAndRemainder()
    {
        Unio<int, string> union = "hello";

        Assert.False(union.TryPickT0(out _, out string? remainder));
        Assert.Equal("hello", remainder);
    }

    [Fact]
    public void TryPickT1_WhenT0_ReturnsFalseAndRemainder()
    {
        Unio<int, string> union = 42;

        Assert.False(union.TryPickT1(out _, out int remainder));
        Assert.Equal(42, remainder);
    }

    [Fact]
    public void Value_ReturnsCurrentValue()
    {
        Unio<int, string> unionInt = 42;
        Unio<int, string> unionStr = "hello";

        Assert.Equal(42, unionInt.Value);
        Assert.Equal("hello", unionStr.Value);
    }

    [Fact]
    public void Match_WhenT0_CallsFirstFunc()
    {
        Unio<int, string> union = 42;

        string result = union.Match(
            i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"),
            s => $"str:{s}");

        Assert.Equal("int:42", result);
    }

    [Fact]
    public void Match_WhenT1_CallsSecondFunc()
    {
        Unio<int, string> union = "hello";

        string result = union.Match(
            i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"),
            s => $"str:{s}");

        Assert.Equal("str:hello", result);
    }

    [Fact]
    public void Match_WithState_WhenT0_CallsFirstFunc()
    {
        Unio<int, string> union = 42;

        string result = union.Match(
            "prefix",
            static (state, i) => string.Create(CultureInfo.InvariantCulture, $"{state}:int:{i}"),
            static (state, s) => $"{state}:str:{s}");

        Assert.Equal("prefix:int:42", result);
    }

    [Fact]
    public void Switch_WhenT0_CallsFirstAction()
    {
        Unio<int, string> union = 42;
        int? captured = null;

        union.Switch(
            i => captured = i,
            _ => { });

        Assert.Equal(42, captured);
    }

    [Fact]
    public void Switch_WhenT1_CallsSecondAction()
    {
        Unio<int, string> union = "hello";
        string? captured = null;

        union.Switch(
            _ => { },
            s => captured = s);

        Assert.Equal("hello", captured);
    }

    [Fact]
    public void Switch_WithState_WhenT0_CallsFirstAction()
    {
        Unio<int, string> union = 42;
        int[] state = new int[] { 10, 0 };

        union.Switch(
            state,
            static (s, i) => s[1] = i + s[0],
            static (_, _) => { });

        Assert.Equal(52, state[1]);
    }

    [Fact]
    public void Equality_SameTypeAndValue_AreEqual()
    {
        Unio<int, string> a = 42;
        Unio<int, string> b = 42;

        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Equality_DifferentValues_AreNotEqual()
    {
        Unio<int, string> a = 42;
        Unio<int, string> b = 99;

        Assert.False(a.Equals(b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equality_DifferentTypes_AreNotEqual()
    {
        Unio<int, string> a = 42;
        Unio<int, string> b = "42";

        Assert.False(a.Equals(b));
        Assert.False(a == b);
    }

    [Fact]
    public void GetHashCode_SameValues_AreSame()
    {
        Unio<int, string> a = 42;
        Unio<int, string> b = 42;

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        Unio<int, string> unionInt = 42;
        Unio<int, string> unionStr = "hello";

        Assert.Equal("42", unionInt.ToString());
        Assert.Equal("hello", unionStr.ToString());
    }

    [Fact]
    public void Equals_WithObject_Works()
    {
        Unio<int, string> a = 42;
        object b = (Unio<int, string>)42;

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        Unio<int, string> a = 42;

        Assert.False(a.Equals((object?)null));
    }
}

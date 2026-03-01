// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1,T2}"/> covering all three type positions,
/// implicit conversions, access, matching, switching, equality and <c>ToString</c>.
/// </summary>
public class Unio3Tests
{
    [Fact]
    public void ImplicitConversion_AllTypes_SetsCorrectIndex()
    {
        Unio<int, string, bool> u0 = 42;
        Unio<int, string, bool> u1 = "hello";
        Unio<int, string, bool> u2 = true;

        Assert.Equal(0, u0.Index);
        Assert.Equal(1, u1.Index);
        Assert.Equal(2, u2.Index);
    }

    [Fact]
    public void IsT_Properties_ReturnCorrectly()
    {
        Unio<int, string, bool> u0 = 42;
        Unio<int, string, bool> u1 = "hello";
        Unio<int, string, bool> u2 = true;

        Assert.True(u0.IsT0);
        Assert.False(u0.IsT1);
        Assert.False(u0.IsT2);

        Assert.False(u1.IsT0);
        Assert.True(u1.IsT1);
        Assert.False(u1.IsT2);

        Assert.False(u2.IsT0);
        Assert.False(u2.IsT1);
        Assert.True(u2.IsT2);
    }

    [Fact]
    public void AsT_Properties_ReturnValues()
    {
        Unio<int, string, bool> u0 = 42;
        Unio<int, string, bool> u1 = "hello";
        Unio<int, string, bool> u2 = true;

        Assert.Equal(42, u0.AsT0);
        Assert.Equal("hello", u1.AsT1);
        Assert.True(u2.AsT2);
    }

    [Fact]
    public void AsT_WrongType_Throws()
    {
        Unio<int, string, bool> union = 42;

        Assert.Throws<InvalidOperationException>(() => union.AsT1);
        Assert.Throws<InvalidOperationException>(() => union.AsT2);
    }

    [Fact]
    public void TryGet_AllTypes_Work()
    {
        Unio<int, string, bool> u0 = 42;
        Unio<int, string, bool> u1 = "hello";
        Unio<int, string, bool> u2 = true;

        Assert.True(u0.TryGetT0(out int v0));
        Assert.Equal(42, v0);
        Assert.False(u0.TryGetT1(out _));
        Assert.False(u0.TryGetT2(out _));

        Assert.True(u1.TryGetT1(out string? v1));
        Assert.Equal("hello", v1);

        Assert.True(u2.TryGetT2(out bool v2));
        Assert.True(v2);
    }

    [Fact]
    public void Match_AllTypes_CallsCorrectFunc()
    {
        Unio<int, string, bool> u0 = 42;
        Unio<int, string, bool> u1 = "hello";
        Unio<int, string, bool> u2 = true;

        Assert.Equal("int:42", u0.Match(i => $"int:{i}", s => $"str:{s}", b => $"bool:{b}"));
        Assert.Equal("str:hello", u1.Match(i => $"int:{i}", s => $"str:{s}", b => $"bool:{b}"));
        Assert.Equal("bool:True", u2.Match(i => $"int:{i}", s => $"str:{s}", b => $"bool:{b}"));
    }

    [Fact]
    public void Switch_AllTypes_CallsCorrectAction()
    {
        Unio<int, string, bool> union = "hello";
        string? captured = null;

        union.Switch(
            _ => { },
            s => captured = s,
            _ => { });

        Assert.Equal("hello", captured);
    }

    [Fact]
    public void Equality_Works()
    {
        Unio<int, string, bool> a = 42;
        Unio<int, string, bool> b = 42;
        Unio<int, string, bool> c = "hello";

        Assert.True(a == b);
        Assert.False(a == c);
    }

    [Fact]
    public void Value_ReturnsCurrentValue()
    {
        Unio<int, string, bool> union = true;

        Assert.Equal(true, union.Value);
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        Unio<int, string, bool> union = "world";

        Assert.Equal("world", union.ToString());
    }
}

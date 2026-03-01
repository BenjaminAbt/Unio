// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1,T2,T3}"/> covering four-type unions.
/// </summary>
public class Unio4Tests
{
    [Fact]
    public void ImplicitConversion_AllTypes_SetsCorrectIndex()
    {
        Unio<int, string, bool, double> u0 = 42;
        Unio<int, string, bool, double> u1 = "hello";
        Unio<int, string, bool, double> u2 = true;
        Unio<int, string, bool, double> u3 = 3.14;

        Assert.Equal(0, u0.Index);
        Assert.Equal(1, u1.Index);
        Assert.Equal(2, u2.Index);
        Assert.Equal(3, u3.Index);
    }

    [Fact]
    public void AsT_ReturnsCorrectValues()
    {
        Unio<int, string, bool, double> u0 = 42;
        Unio<int, string, bool, double> u3 = 3.14;

        Assert.Equal(42, u0.AsT0);
        Assert.Equal(3.14, u3.AsT3);
    }

    [Fact]
    public void TryGet_Works()
    {
        Unio<int, string, bool, double> union = 3.14;

        Assert.False(union.TryGetT0(out _));
        Assert.False(union.TryGetT1(out _));
        Assert.False(union.TryGetT2(out _));
        Assert.True(union.TryGetT3(out double val));
        Assert.Equal(3.14, val);
    }

    [Fact]
    public void Match_CallsCorrectFunc()
    {
        Unio<int, string, bool, double> union = true;

        string result = union.Match(
            i => "int", s => "str", b => "bool", d => "double");

        Assert.Equal("bool", result);
    }

    [Fact]
    public void Equality_Works()
    {
        Unio<int, string, bool, double> a = 3.14;
        Unio<int, string, bool, double> b = 3.14;

        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}

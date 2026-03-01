// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1,T2,T3,T4}"/> covering five-type unions.
/// </summary>
public class Unio5Tests
{
    [Fact]
    public void AllPositions_Work()
    {
        Unio<int, string, bool, double, long> u0 = 42;
        Unio<int, string, bool, double, long> u1 = "hello";
        Unio<int, string, bool, double, long> u2 = true;
        Unio<int, string, bool, double, long> u3 = 3.14;
        Unio<int, string, bool, double, long> u4 = 100L;

        Assert.Equal(0, u0.Index);
        Assert.Equal(1, u1.Index);
        Assert.Equal(2, u2.Index);
        Assert.Equal(3, u3.Index);
        Assert.Equal(4, u4.Index);

        Assert.Equal(42, u0.AsT0);
        Assert.Equal("hello", u1.AsT1);
        Assert.True(u2.AsT2);
        Assert.Equal(3.14, u3.AsT3);
        Assert.Equal(100L, u4.AsT4);
    }

    [Fact]
    public void Match_CallsCorrectFunc()
    {
        Unio<int, string, bool, double, long> union = 100L;

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", l => $"4:{l}");

        Assert.Equal("4:100", result);
    }

    [Fact]
    public void Equality_Works()
    {
        Unio<int, string, bool, double, long> a = 100L;
        Unio<int, string, bool, double, long> b = 100L;
        Unio<int, string, bool, double, long> c = 42;

        Assert.True(a == b);
        Assert.False(a == c);
    }
}

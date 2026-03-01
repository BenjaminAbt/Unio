// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.UnitTests;

/// <summary>
/// Tests for higher-arity unions (6–9 type parameters) focusing on boundary cases,
/// <c>Match</c>, <c>Switch</c>, equality, <c>Value</c> and <c>ToString</c>.
/// </summary>
public class UnioHighArityTests
{
    [Fact]
    public void Unio6_AllPositions()
    {
        Unio<int, string, bool, double, long, byte> u0 = 1;
        Unio<int, string, bool, double, long, byte> u5 = (byte)255;

        Assert.Equal(0, u0.Index);
        Assert.Equal(5, u5.Index);
        Assert.Equal(1, u0.AsT0);
        Assert.Equal((byte)255, u5.AsT5);
        Assert.True(u5.TryGetT5(out byte val));
        Assert.Equal(255, val);
    }

    [Fact]
    public void Unio6_Match()
    {
        Unio<int, string, bool, double, long, byte> union = (byte)42;

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", _ => "4", b => $"5:{b}");

        Assert.Equal("5:42", result);
    }

    [Fact]
    public void Unio7_AllPositions()
    {
        Unio<int, string, bool, double, long, byte, float> u6 = 1.5f;

        Assert.Equal(6, u6.Index);
        Assert.True(u6.IsT6);
        Assert.Equal(1.5f, u6.AsT6);
    }

    [Fact]
    public void Unio7_Match()
    {
        Unio<int, string, bool, double, long, byte, float> union = 1.5f;

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", _ => "4", _ => "5", f => $"6:{f.ToString(CultureInfo.InvariantCulture)}");

        Assert.Equal("6:1.5", result);
    }

    [Fact]
    public void Unio8_AllPositions()
    {
        Unio<int, string, bool, double, long, byte, float, char> u7 = 'X';

        Assert.Equal(7, u7.Index);
        Assert.True(u7.IsT7);
        Assert.Equal('X', u7.AsT7);
    }

    [Fact]
    public void Unio8_Match()
    {
        Unio<int, string, bool, double, long, byte, float, char> union = 'Z';

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", _ => "4", _ => "5", _ => "6", c => $"7:{c}");

        Assert.Equal("7:Z", result);
    }

    [Fact]
    public void Unio9_AllPositions()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> u8 = 9.99m;

        Assert.Equal(8, u8.Index);
        Assert.True(u8.IsT8);
        Assert.Equal(9.99m, u8.AsT8);
    }

    [Fact]
    public void Unio9_Match()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> union = 9.99m;

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", _ => "4",
            _ => "5", _ => "6", _ => "7", d => $"8:{d.ToString(CultureInfo.InvariantCulture)}");

        Assert.Equal("8:9.99", result);
    }

    [Fact]
    public void Unio9_Equality()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> a = 9.99m;
        Unio<int, string, bool, double, long, byte, float, char, decimal> b = 9.99m;
        Unio<int, string, bool, double, long, byte, float, char, decimal> c = 42;

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Unio9_Switch()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> union = "test";
        string? captured = null;

        union.Switch(
            _ => { }, s => captured = s, _ => { }, _ => { }, _ => { },
            _ => { }, _ => { }, _ => { }, _ => { });

        Assert.Equal("test", captured);
    }

    [Fact]
    public void Unio9_Value()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> union = 'A';

        Assert.Equal('A', union.Value);
    }

    [Fact]
    public void Unio9_ToString()
    {
        Unio<int, string, bool, double, long, byte, float, char, decimal> union = 42;

        Assert.Equal("42", union.ToString());
    }
}

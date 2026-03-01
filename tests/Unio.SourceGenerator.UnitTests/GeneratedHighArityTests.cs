// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.SourceGenerator.UnitTests;

/// <summary>
/// Unit tests for source-generated high-arity union structs (<see cref="Result4"/> and <see cref="BigUnion"/>)
/// verifying correct behaviour for 4-type and 9-type (maximum arity) unions.
/// </summary>
public class GeneratedHighArityTests
{
    [Fact]
    public void Unio4_AllPositions()
    {
        Result4 u0 = 42;
        Result4 u3 = 3.14;

        Assert.Equal(0, u0.Index);
        Assert.Equal(3, u3.Index);
        Assert.Equal(42, u0.AsT0);
        Assert.Equal(3.14, u3.AsT3);
    }

    [Fact]
    public void Unio4_Match()
    {
        Result4 union = true;

        string result = union.Match(
            _ => "int", _ => "str", _ => "bool", _ => "double");

        Assert.Equal("bool", result);
    }

    [Fact]
    public void Unio9_AllPositions()
    {
        BigUnion u0 = 42;
        BigUnion u8 = 9.99m;

        Assert.Equal(0, u0.Index);
        Assert.Equal(8, u8.Index);
        Assert.True(u0.IsT0);
        Assert.True(u8.IsT8);
        Assert.Equal(42, u0.AsT0);
        Assert.Equal(9.99m, u8.AsT8);
    }

    [Fact]
    public void Unio9_TryGet()
    {
        BigUnion union = 9.99m;

        Assert.False(union.TryGetT0(out _));
        Assert.True(union.TryGetT8(out decimal val));
        Assert.Equal(9.99m, val);
    }

    [Fact]
    public void Unio9_Match()
    {
        BigUnion union = 'X';

        string result = union.Match(
            _ => "0", _ => "1", _ => "2", _ => "3", _ => "4",
            _ => "5", _ => "6", c => $"7:{c}", _ => "8");

        Assert.Equal("7:X", result);
    }

    [Fact]
    public void Unio9_Switch()
    {
        BigUnion union = "test";
        string? captured = null;

        union.Switch(
            _ => { }, s => captured = s, _ => { }, _ => { }, _ => { },
            _ => { }, _ => { }, _ => { }, _ => { });

        Assert.Equal("test", captured);
    }

    [Fact]
    public void Unio9_Equality()
    {
        BigUnion a = 9.99m;
        BigUnion b = 9.99m;
        BigUnion c = 42;

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Unio9_Value_And_ToString()
    {
        BigUnion union = 42;

        Assert.Equal(42, union.Value);
        Assert.Equal("42", union.ToString());
    }
}

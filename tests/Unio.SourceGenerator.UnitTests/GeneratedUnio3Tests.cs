// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.SourceGenerator.UnitTests;

/// <summary>
/// Unit tests for the source-generated <see cref="Result3"/> (3-arity) union struct.
/// </summary>
public class GeneratedUnio3Tests
{
    [Fact]
    public void ImplicitConversion_AllTypes()
    {
        Result3 u0 = 42;
        Result3 u1 = "hello";
        Result3 u2 = true;

        Assert.Equal(0, u0.Index);
        Assert.Equal(1, u1.Index);
        Assert.Equal(2, u2.Index);
    }

    [Fact]
    public void IsT_Properties_ReturnCorrectly()
    {
        Result3 u1 = "hello";

        Assert.False(u1.IsT0);
        Assert.True(u1.IsT1);
        Assert.False(u1.IsT2);
    }

    [Fact]
    public void AsT_ReturnsValues()
    {
        Result3 u0 = 42;
        Result3 u1 = "hello";
        Result3 u2 = true;

        Assert.Equal(42, u0.AsT0);
        Assert.Equal("hello", u1.AsT1);
        Assert.True(u2.AsT2);
    }

    [Fact]
    public void TryGet_Works()
    {
        Result3 union = "hello";

        Assert.False(union.TryGetT0(out _));
        Assert.True(union.TryGetT1(out string? val));
        Assert.Equal("hello", val);
        Assert.False(union.TryGetT2(out _));
    }

    [Fact]
    public void Match_CallsCorrectFunc()
    {
        Result3 u0 = 42;
        Result3 u1 = "hello";
        Result3 u2 = true;

        Assert.Equal("int:42", u0.Match(i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"), s => $"str:{s}", b => $"bool:{b}"));
        Assert.Equal("str:hello", u1.Match(i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"), s => $"str:{s}", b => $"bool:{b}"));
        Assert.Equal("bool:True", u2.Match(i => string.Create(CultureInfo.InvariantCulture, $"int:{i}"), s => $"str:{s}", b => $"bool:{b}"));
    }

    [Fact]
    public void Switch_CallsCorrectAction()
    {
        Result3 union = true;
        bool? captured = null;

        union.Switch(
            _ => { },
            _ => { },
            b => captured = b);

        Assert.True(captured);
    }

    [Fact]
    public void Equality_Works()
    {
        Result3 a = 42;
        Result3 b = 42;
        Result3 c = "hello";

        Assert.True(a == b);
        Assert.False(a == c);
    }

    [Fact]
    public void Constructor_AllPositions()
    {
        Result3 u0 = new Result3(42);
        Result3 u1 = new Result3("hello");
        Result3 u2 = new Result3(true);

        Assert.Equal(0, u0.Index);
        Assert.Equal(1, u1.Index);
        Assert.Equal(2, u2.Index);
        Assert.Equal(42, u0.AsT0);
        Assert.Equal("hello", u1.AsT1);
        Assert.True(u2.AsT2);
    }
}

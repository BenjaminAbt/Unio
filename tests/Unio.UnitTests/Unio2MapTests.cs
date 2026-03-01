// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for MapT# extension methods on <see cref="Unio{T0,T1}"/>.
/// </summary>
public class Unio2MapTests
{
    [Fact]
    public void MapT0_WhenT0_TransformsValue()
    {
        Unio<int, string> union = 42;

        Unio<long, string> mapped = union.MapT0(i => (long)i * 2);

        Assert.True(mapped.IsT0);
        Assert.Equal(84L, mapped.AsT0);
    }

    [Fact]
    public void MapT0_WhenT1_PreservesValue()
    {
        Unio<int, string> union = "hello";

        Unio<long, string> mapped = union.MapT0(i => (long)i * 2);

        Assert.True(mapped.IsT1);
        Assert.Equal("hello", mapped.AsT1);
    }

    [Fact]
    public void MapT1_WhenT1_TransformsValue()
    {
        Unio<int, string> union = "hello";

        Unio<int, int> mapped = union.MapT1(s => s.Length);

        Assert.True(mapped.IsT1);
        Assert.Equal(5, mapped.AsT1);
    }

    [Fact]
    public void MapT1_WhenT0_PreservesValue()
    {
        Unio<int, string> union = 42;

        Unio<int, int> mapped = union.MapT1(s => s.Length);

        Assert.True(mapped.IsT0);
        Assert.Equal(42, mapped.AsT0);
    }
}

/// <summary>
/// Unit tests for MapT# on <see cref="Unio{T0,T1,T2}"/> (3-arity).
/// </summary>
public class Unio3MapTests
{
    [Fact]
    public void MapT0_WhenT0_TransformsValue()
    {
        Unio<int, string, bool> union = 42;

        Unio<long, string, bool> mapped = union.MapT0(i => (long)i);

        Assert.True(mapped.IsT0);
        Assert.Equal(42L, mapped.AsT0);
    }

    [Fact]
    public void MapT1_WhenT2_PreservesValue()
    {
        Unio<int, string, bool> union = true;

        Unio<int, int, bool> mapped = union.MapT1(s => s.Length);

        Assert.True(mapped.IsT2);
        Assert.True(mapped.AsT2);
    }

    [Fact]
    public void MapT2_WhenT2_TransformsValue()
    {
        Unio<int, string, bool> union = true;

        Unio<int, string, int> mapped = union.MapT2(b => b ? 1 : 0);

        Assert.True(mapped.IsT2);
        Assert.Equal(1, mapped.AsT2);
    }
}

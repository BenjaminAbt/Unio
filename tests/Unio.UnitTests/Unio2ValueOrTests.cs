// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for ValueOrT# methods on <see cref="Unio{T0,T1}"/>.
/// </summary>
public class Unio2ValueOrTests
{
    [Fact]
    public void ValueOrT0_WhenT0_ReturnsValue()
    {
        Unio<int, string> union = 42;

        Assert.Equal(42, union.ValueOrT0(0));
    }

    [Fact]
    public void ValueOrT0_WhenT1_ReturnsFallback()
    {
        Unio<int, string> union = "hello";

        Assert.Equal(0, union.ValueOrT0(0));
    }

    [Fact]
    public void ValueOrT1_WhenT1_ReturnsValue()
    {
        Unio<int, string> union = "hello";

        Assert.Equal("hello", union.ValueOrT1("default"));
    }

    [Fact]
    public void ValueOrT1_WhenT0_ReturnsFallback()
    {
        Unio<int, string> union = 42;

        Assert.Equal("default", union.ValueOrT1("default"));
    }

    [Fact]
    public void ValueOrT0_WithFactory_WhenT0_ReturnsValue()
    {
        Unio<int, string> union = 42;
        bool factoryInvoked = false;

        int result = union.ValueOrT0(() => { factoryInvoked = true; return 99; });

        Assert.Equal(42, result);
        Assert.False(factoryInvoked);
    }

    [Fact]
    public void ValueOrT0_WithFactory_WhenT1_InvokesFactory()
    {
        Unio<int, string> union = "hello";

        int result = union.ValueOrT0(() => 99);

        Assert.Equal(99, result);
    }

    [Fact]
    public void ValueOrT1_WithFactory_WhenT0_InvokesFactory()
    {
        Unio<int, string> union = 42;

        string result = union.ValueOrT1(() => "fallback");

        Assert.Equal("fallback", result);
    }
}

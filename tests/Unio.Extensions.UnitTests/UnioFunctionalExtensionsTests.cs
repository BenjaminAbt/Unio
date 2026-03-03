// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.Extensions.UnitTests;

public class UnioFunctionalExtensionsTests
{
    [Fact]
    public void BindT0_WhenUnionHoldsT0_BindsToNewUnion()
    {
        Unio<int, string> value = 21;

        Unio<double, string> result = value.BindT0(static i => i * 2.0);

        Assert.True(result.IsT0);
        Assert.Equal(42.0, result.AsT0);
    }

    [Fact]
    public void BindT0_WhenUnionHoldsT1_PassesThroughT1()
    {
        Unio<int, string> value = "invalid";

        Unio<double, string> result = value.BindT0(static i => i * 2.0);

        Assert.True(result.IsT1);
        Assert.Equal("invalid", result.AsT1);
    }

    [Fact]
    public void BindT1_WithState_WhenUnionHoldsT1_UsesStateWithoutCapture()
    {
        Unio<int, string> value = "7";

        Unio<int, int> result = value.BindT1(
            10,
            static (state, text) => int.Parse(text, CultureInfo.InvariantCulture) + state);

        Assert.True(result.IsT1);
        Assert.Equal(17, result.AsT1);
    }

    [Fact]
    public void TapT0_WhenUnionHoldsT0_ExecutesActionAndReturnsOriginalValue()
    {
        Unio<int, string> value = 42;
        int tapped = 0;

        Unio<int, string> result = value.TapT0(i => tapped = i);

        Assert.Equal(42, tapped);
        Assert.Equal(value, result);
    }

    [Fact]
    public void TapT1_WithState_WhenUnionHoldsT1_ExecutesAction()
    {
        Unio<int, string> value = "err";
        int[] state = [5, 0];

        Unio<int, string> result = value.TapT1(
            state,
            static (state, error) =>
            {
                _ = state[0];
                // state and value both flow through the allocation-free overload.
                state[1] = error.Length;
            });

        Assert.Equal(3, state[1]);
        Assert.Equal(value, result);
    }

    [Fact]
    public void RecoverT1_WhenUnionHoldsT0_ReturnsT0Value()
    {
        Unio<int, string> value = 99;

        int result = value.RecoverT1(static _ => -1);

        Assert.Equal(99, result);
    }

    [Fact]
    public void RecoverT1_WithState_WhenUnionHoldsT1_UsesRecoveryFunction()
    {
        Unio<int, string> value = "oops";

        int result = value.RecoverT1(
            100,
            static (state, error) => state + error.Length);

        Assert.Equal(104, result);
    }

    [Fact]
    public void Fold_WhenUnionHoldsT1_ReturnsProjectedResult()
    {
        Unio<int, string> value = "abc";

        string result = value.Fold(
            static i => string.Create(CultureInfo.InvariantCulture, $"i:{i}"),
            static s => $"s:{s}");

        Assert.Equal("s:abc", result);
    }
}

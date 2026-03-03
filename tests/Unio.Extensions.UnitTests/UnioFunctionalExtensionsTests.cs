// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Unio.Types;

namespace Unio.Extensions.UnitTests;

public class UnioFunctionalExtensionsTests
{
    [Fact]
    public void MapT0_WhenUnionHoldsT0_MapsValue()
    {
        Unio<int, string> value = 5;

        Unio<string, string> result = value.MapT0(static i => $"#{i}");

        Assert.True(result.IsT0);
        Assert.Equal("#5", result.AsT0);
    }

    [Fact]
    public void BiMap_WhenUnionHoldsT1_MapsSecondBranch()
    {
        Unio<int, string> value = "hello";

        Unio<double, int> result = value.BiMap(static i => i * 2.0, static s => s.Length);

        Assert.True(result.IsT1);
        Assert.Equal(5, result.AsT1);
    }

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
    public async Task BindT0Async_WhenUnionHoldsT0_MapsAsync()
    {
        Unio<int, string> value = 21;

        Unio<double, string> result = await value.BindT0Async(static async i => { await Task.Yield(); return i * 2.0; });

        Assert.True(result.IsT0);
        Assert.Equal(42.0, result.AsT0);
    }

    [Fact]
    public async Task TapT1Async_WhenUnionHoldsT1_ExecutesAction()
    {
        Unio<int, string> value = "abc";
        int len = 0;

        Unio<int, string> result = await value.TapT1Async(s =>
        {
            len = s.Length;
            return Task.CompletedTask;
        });

        Assert.Equal(3, len);
        Assert.Equal(value, result);
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
    public void RecoverT0_WhenUnionHoldsT0_ReturnsRecoveredT1()
    {
        Unio<int, string> value = 9;

        string result = value.RecoverT0(static i => $"N:{i}");

        Assert.Equal("N:9", result);
    }

    [Fact]
    public void EnsureT0_WhenPredicateFails_ConvertsToT1()
    {
        Unio<int, string> value = 3;

        Unio<int, string> result = value.EnsureT0(static i => i > 5, static i => $"too-small:{i}");

        Assert.True(result.IsT1);
        Assert.Equal("too-small:3", result.AsT1);
    }

    [Fact]
    public void PickT0Or_WhenUnionHoldsT1_UsesFallback()
    {
        Unio<int, string> value = "x";

        int result = value.PickT0Or(static s => s.Length);

        Assert.Equal(1, result);
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

    [Fact]
    public async Task FoldAsync_WhenUnionHoldsT0_UsesAsyncBranch()
    {
        Unio<int, string> value = 7;

        string result = await value.FoldAsync(
            static i => Task.FromResult($"i:{i}"),
            static s => Task.FromResult($"s:{s}"));

        Assert.Equal("i:7", result);
    }

    [Fact]
    public void SelectMany_WhenBothAreT0_ProjectsValue()
    {
        Unio<int, string> value = 5;

        Unio<int, string> result =
            from left in value
            from right in (Unio<int, string>)(left * 2)
            select left + right;

        Assert.True(result.IsT0);
        Assert.Equal(15, result.AsT0);
    }

    [Fact]
    public void ToResult_And_FromResult_Roundtrip()
    {
        Unio<int, string> value = 42;

        Unio<Result<int>, Error<string>> wrapped = value.ToResult();
        Unio<int, string> roundtrip = wrapped.FromResult();

        Assert.Equal(value, roundtrip);
    }
}

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

        Unio<int, string> errValue = "bad";
        Unio<Result<int>, Error<string>> wrappedErr = errValue.ToResult();
        Assert.Equal(errValue, wrappedErr.FromResult());
    }

    [Fact]
    public void MapAndBind_AllOverloads_CoverBothBranches()
    {
        Unio<int, string> t0 = 5;
        Unio<int, string> t1 = "7";

        Assert.Equal("#5", UnioFunctionalExtensions.MapT0(t0, static i => $"#{i}").AsT0);
        Assert.Equal("7", UnioFunctionalExtensions.MapT0(t1, static i => $"#{i}").AsT1);

        Assert.Equal("v=5", t0.MapT0("v=", static (p, i) => $"{p}{i}").AsT0);
        Assert.Equal("7", t1.MapT0("v=", static (p, i) => $"{p}{i}").AsT1);

        Assert.Equal(5, UnioFunctionalExtensions.MapT1(t0, static s => s.Length).AsT0);
        Assert.Equal(1, UnioFunctionalExtensions.MapT1(t1, static s => s.Length).AsT1);

        Assert.Equal(5, t0.MapT1(10, static (o, s) => s.Length + o).AsT0);
        Assert.Equal(11, t1.MapT1(10, static (o, s) => s.Length + o).AsT1);

        Assert.Equal(15, t0.BindT0(10, static (o, i) => i + o).AsT0);
        Assert.Equal("7", t1.BindT0(10, static (o, i) => i + o).AsT1);

        Assert.Equal(5, t0.BindT1(static s => s.Length).AsT0);
        Assert.Equal(1, t1.BindT1(static s => s.Length).AsT1);

        Assert.Equal(5, t0.BindT1(10, static (o, s) => s.Length + o).AsT0);

        Assert.Equal(10.0, t0.BiMap(static i => i * 2.0, static s => s.Length).AsT0);
    }

    [Fact]
    public async Task AsyncExtensions_CoverBothBranches()
    {
        Unio<int, string> t0 = 5;
        Unio<int, string> t1 = "abc";

        Assert.Equal("abc", (await t1.BindT0Async(static i => Task.FromResult(i * 2.0))).AsT1);
        Assert.Equal(5, (await t0.BindT1Async(static s => Task.FromResult(s.Length))).AsT0);
        Assert.Equal(3, (await t1.BindT1Async(static async s => { await Task.Yield(); return s.Length; })).AsT1);

        Assert.Equal("s:abc", await t1.FoldAsync(static i => Task.FromResult($"i:{i}"), static s => Task.FromResult($"s:{s}")));

        int count = 0;
        Assert.Equal(t0, await t0.TapT0Async(i => { count += i; return Task.CompletedTask; }));
        Assert.Equal(5, count);
        Assert.Equal(t1, await t1.TapT0Async(_ => { count += 100; return Task.CompletedTask; }));
        Assert.Equal(5, count);
        Assert.Equal(t0, await t0.TapT1Async(_ => { count += 100; return Task.CompletedTask; }));
        Assert.Equal(5, count);
    }

    [Fact]
    public void EnsureTapRecoverFoldOnPickAndLinq_CoverRemainingBranches()
    {
        Unio<int, string> t0 = 10;
        Unio<int, string> t1 = "err";

        Assert.Equal(t0, t0.EnsureT0(static i => i > 5, static _ => "bad"));
        Assert.Equal(t1, t1.EnsureT0(static i => i > 5, static _ => "bad"));

        Assert.Equal(t0, t0.EnsureT1(static s => s.Length < 10, static _ => -1));
        Assert.Equal(t1, t1.EnsureT1(static s => s.Length < 10, static _ => -1));
        Assert.Equal(-1, t1.EnsureT1(static s => s.Length > 10, static _ => -1).AsT0);

        int sum = 0;
        Assert.Equal(t1, t1.TapT0(i => sum += i));
        Assert.Equal(t0, t0.TapT0(2, (s, i) => sum += s * i));
        Assert.Equal(t1, t1.TapT0(2, (s, i) => sum += s * i));
        Assert.Equal(20, sum);

        Assert.Equal(t0, t0.TapT1(_ => sum += 100));
        Assert.Equal(t1, t1.TapT1(s => sum += s.Length));
        Assert.Equal(t0, t0.TapT1(1, (st, _) => sum += st));
        Assert.Equal(23, sum);

        Assert.Equal(3, t1.RecoverT1(static s => s.Length));
        Assert.Equal(10, t0.RecoverT1(5, static (st, s) => st + s.Length));
        Assert.Equal("err", t1.RecoverT0(static i => $"{i}"));
        Assert.Equal("p:10", t0.RecoverT0("p:", static (p, i) => $"{p}{i}"));
        Assert.Equal("err", t1.RecoverT0("p:", static (p, i) => $"{p}{i}"));

        Assert.Equal("i:10", t0.Fold(static i => $"i:{i}", static s => $"s:{s}"));
        Assert.Equal("p:10", t0.Fold("p:", static (p, i) => $"{p}{i}", static (p, s) => $"{p}{s}"));
        Assert.Equal("p:err", t1.Fold("p:", static (p, i) => $"{p}{i}", static (p, s) => $"{p}{s}"));

        int onHits = 0;
        Assert.True(t0.OnT0(i => onHits += i));
        Assert.False(t1.OnT0(_ => onHits += 100));
        Assert.False(t0.OnT1(_ => onHits += 100));
        Assert.True(t1.OnT1(s => onHits += s.Length));
        Assert.Equal(13, onHits);

        Assert.Equal(10, t0.PickT0Or(static s => s.Length));
        Assert.Equal("err", t1.PickT1Or(static i => $"{i}"));
        Assert.Equal("10", t0.PickT1Or(static i => $"{i}"));

        Assert.Equal(20, (from x in t0 select x * 2).AsT0);
        Assert.Equal("err", t1.SelectMany(static x => (Unio<int, string>)(x * 2), static (a, b) => a + b).AsT1);
        Assert.Equal("mid-err", t0.SelectMany(static _ => (Unio<int, string>)"mid-err", static (a, b) => a + b).AsT1);
    }
}

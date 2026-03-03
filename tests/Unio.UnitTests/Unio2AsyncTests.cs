// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1}.MatchAsync"/>, <see cref="Unio{T0,T1}.MatchAsync{TState,TResult}"/>,
/// <see cref="Unio{T0,T1}.SwitchAsync"/> and <see cref="Unio{T0,T1}.SwitchAsync{TState}"/>.
/// </summary>
public class Unio2AsyncTests
{
    [Fact]
    public async Task MatchAsync_WhenT0_CallsFirstFunc()
    {
        Unio<int, string> union = 42;

        string result = await union.Match(
            i => ValueTask.FromResult(string.Create(CultureInfo.InvariantCulture, $"int:{i}")),
            s => ValueTask.FromResult($"str:{s}"));

        Assert.Equal("int:42", result);
    }

    [Fact]
    public async Task MatchAsync_WhenT1_CallsSecondFunc()
    {
        Unio<int, string> union = "hello";

        string result = await union.Match(
            i => ValueTask.FromResult(string.Create(CultureInfo.InvariantCulture, $"int:{i}")),
            s => ValueTask.FromResult($"str:{s}"));

        Assert.Equal("str:hello", result);
    }

    [Fact]
    public async Task SwitchAsync_WhenT0_CallsFirstAction()
    {
        Unio<int, string> union = 42;
        int? captured = null;

        await union.Switch(
            i => { captured = i; return ValueTask.CompletedTask; },
            _ => ValueTask.CompletedTask);

        Assert.Equal(42, captured);
    }

    [Fact]
    public async Task SwitchAsync_WhenT1_CallsSecondAction()
    {
        Unio<int, string> union = "hello";
        string? captured = null;

        await union.Switch(
            _ => ValueTask.CompletedTask,
            s => { captured = s; return ValueTask.CompletedTask; });

        Assert.Equal("hello", captured);
    }

    [Fact]
    public async Task MatchAsync_WithState_WhenT0_PassesStateAndCallsFirstFunc()
    {
        Unio<int, string> union = 42;
        string prefix = "Value";

        string result = await union.Match(prefix,
            static (p, i) => ValueTask.FromResult(string.Create(CultureInfo.InvariantCulture, $"{p}:{i}")),
            static (p, s) => ValueTask.FromResult($"{p}:{s}"));

        Assert.Equal("Value:42", result);
    }

    [Fact]
    public async Task MatchAsync_WithState_WhenT1_PassesStateAndCallsSecondFunc()
    {
        Unio<int, string> union = "hello";
        string prefix = "Value";

        string result = await union.Match(prefix,
            static (p, i) => ValueTask.FromResult(string.Create(CultureInfo.InvariantCulture, $"{p}:{i}")),
            static (p, s) => ValueTask.FromResult($"{p}:{s}"));

        Assert.Equal("Value:hello", result);
    }

    [Fact]
    public async Task MatchAsync_WithState_WhenT0_DoesNotInvokeT1Func()
    {
        Unio<int, string> union = 42;
        bool t1Invoked = false;

        await union.Match(0,
            static (_, i) => ValueTask.FromResult(i),
            (_, s) => { t1Invoked = true; return ValueTask.FromResult(s.Length); });

        Assert.False(t1Invoked);
    }

    [Fact]
    public async Task SwitchAsync_WithState_WhenT0_PassesStateAndCallsFirstAction()
    {
        Unio<int, string> union = 42;
        string prefix = "v";
        List<string> log = [];

        await union.Switch((prefix, log),
            static (s, i) => { s.log.Add(string.Create(CultureInfo.InvariantCulture, $"{s.prefix}:{i}")); return ValueTask.CompletedTask; },
            static (_, _) => ValueTask.CompletedTask);

        Assert.Equal("v:42", Assert.Single(log));
    }

    [Fact]
    public async Task SwitchAsync_WithState_WhenT1_PassesStateAndCallsSecondAction()
    {
        Unio<int, string> union = "hello";
        string prefix = "v";
        List<string> log = [];

        await union.Switch((prefix, log),
            static (_, _) => ValueTask.CompletedTask,
            static (s, str) => { s.log.Add($"{s.prefix}:{str}"); return ValueTask.CompletedTask; });

        Assert.Equal("v:hello", Assert.Single(log));
    }

    [Fact]
    public async Task SwitchAsync_WithState_WhenT0_DoesNotInvokeT1Action()
    {
        Unio<int, string> union = 42;
        bool t1Invoked = false;

        await union.Switch(0,
            static (_, _) => ValueTask.CompletedTask,
            (_, _) => { t1Invoked = true; return ValueTask.CompletedTask; });

        Assert.False(t1Invoked);
    }
}

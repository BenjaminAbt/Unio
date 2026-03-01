// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="Unio{T0,T1}.MatchAsync"/> and <see cref="Unio{T0,T1}.SwitchAsync"/>.
/// </summary>
public class Unio2AsyncTests
{
    [Fact]
    public async Task MatchAsync_WhenT0_CallsFirstFunc()
    {
        Unio<int, string> union = 42;

        string result = await union.MatchAsync(
            i => Task.FromResult($"int:{i}"),
            s => Task.FromResult($"str:{s}"));

        Assert.Equal("int:42", result);
    }

    [Fact]
    public async Task MatchAsync_WhenT1_CallsSecondFunc()
    {
        Unio<int, string> union = "hello";

        string result = await union.MatchAsync(
            i => Task.FromResult($"int:{i}"),
            s => Task.FromResult($"str:{s}"));

        Assert.Equal("str:hello", result);
    }

    [Fact]
    public async Task SwitchAsync_WhenT0_CallsFirstAction()
    {
        Unio<int, string> union = 42;
        int? captured = null;

        await union.SwitchAsync(
            i => { captured = i; return Task.CompletedTask; },
            _ => Task.CompletedTask);

        Assert.Equal(42, captured);
    }

    [Fact]
    public async Task SwitchAsync_WhenT1_CallsSecondAction()
    {
        Unio<int, string> union = "hello";
        string? captured = null;

        await union.SwitchAsync(
            _ => Task.CompletedTask,
            s => { captured = s; return Task.CompletedTask; });

        Assert.Equal("hello", captured);
    }
}

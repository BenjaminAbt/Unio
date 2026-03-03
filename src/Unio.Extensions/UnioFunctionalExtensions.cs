// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

#nullable enable

using System.Runtime.CompilerServices;
using Unio.Types;

namespace Unio.Extensions;

/// <summary>
/// Provides functional extension methods for <see cref="Unio{T0, T1}"/> to simplify fluent pipelines.
/// </summary>
public static class UnioFunctionalExtensions
{
    /// <summary>
    /// Maps the <c>T0</c> branch to a new type while passing through <c>T1</c> unchanged.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = 21;
    /// Unio&lt;double, string&gt; mapped = value.MapT0(static i => i * 2.0);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> MapT0<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T0, TOut> map) =>
        value.BindT0(map);

    /// <summary>
    /// Maps the <c>T0</c> branch to a new type while passing caller state to avoid captures.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = 21;
    /// Unio&lt;string, string&gt; mapped = value.MapT0("v=", static (prefix, i) => $"{prefix}{i}");
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> MapT0<TState, T0, T1, TOut>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T0, TOut> map) =>
        value.BindT0(state, map);

    /// <summary>
    /// Maps the <c>T1</c> branch to a new type while passing through <c>T0</c> unchanged.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = "7";
    /// Unio&lt;int, int&gt; mapped = value.MapT1(static s => int.Parse(s));
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, TOut> MapT1<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T1, TOut> map) =>
        value.BindT1(map);

    /// <summary>
    /// Maps the <c>T1</c> branch to a new type while passing caller state to avoid captures.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = "7";
    /// Unio&lt;int, int&gt; mapped = value.MapT1(10, static (offset, s) => int.Parse(s) + offset);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, TOut> MapT1<TState, T0, T1, TOut>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T1, TOut> map) =>
        value.BindT1(state, map);

    /// <summary>
    /// Binds the <c>T0</c> branch to a new union while passing through <c>T1</c> unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> BindT0<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T0, TOut> bind) =>
        value.Match(bind, static (map, t0) => (Unio<TOut, T1>)map(t0), static (_, t1) => (Unio<TOut, T1>)t1);

    /// <summary>
    /// Binds the <c>T0</c> branch to a new union while passing caller state to avoid captures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> BindT0<TState, T0, T1, TOut>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T0, TOut> bind)
    {
        (TState State, Func<TState, T0, TOut> Bind) context = (state, bind);

        return value.Match(
            context,
            static (ctx, t0) => (Unio<TOut, T1>)ctx.Bind(ctx.State, t0),
            static (_, t1) => (Unio<TOut, T1>)t1);
    }

    /// <summary>
    /// Binds the <c>T1</c> branch to a new union while passing through <c>T0</c> unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, TOut> BindT1<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T1, TOut> bind) =>
        value.Match(bind, static (_, t0) => (Unio<T0, TOut>)t0, static (map, t1) => (Unio<T0, TOut>)map(t1));

    /// <summary>
    /// Binds the <c>T1</c> branch to a new union while passing caller state to avoid captures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, TOut> BindT1<TState, T0, T1, TOut>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T1, TOut> bind)
    {
        (TState State, Func<TState, T1, TOut> Bind) context = (state, bind);

        return value.Match(
            context,
            static (_, t0) => (Unio<T0, TOut>)t0,
            static (ctx, t1) => (Unio<T0, TOut>)ctx.Bind(ctx.State, t1));
    }

    /// <summary>
    /// Asynchronously binds the <c>T0</c> branch to a new union value.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = 21;
    /// Unio&lt;double, string&gt; mapped = await value.BindT0Async(static async i => i * 2.0);
    /// </code>
    /// </example>
    public static async Task<Unio<TOut, T1>> BindT0Async<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T0, Task<TOut>> bind)
    {
        if (value.IsT0)
        {
            T0 t0 = value.AsT0;
            return (Unio<TOut, T1>)await bind(t0).ConfigureAwait(false);
        }

        return (Unio<TOut, T1>)value.AsT1;
    }

    /// <summary>
    /// Asynchronously binds the <c>T1</c> branch to a new union value.
    /// </summary>
    public static async Task<Unio<T0, TOut>> BindT1Async<T0, T1, TOut>(
        this Unio<T0, T1> value,
        Func<T1, Task<TOut>> bind)
    {
        if (value.IsT1)
        {
            T1 t1 = value.AsT1;
            return (Unio<T0, TOut>)await bind(t1).ConfigureAwait(false);
        }

        return (Unio<T0, TOut>)value.AsT0;
    }

    /// <summary>
    /// Asynchronously folds both branches into a shared result type.
    /// </summary>
    public static Task<TResult> FoldAsync<T0, T1, TResult>(
        this Unio<T0, T1> value,
        Func<T0, Task<TResult>> whenT0,
        Func<T1, Task<TResult>> whenT1) =>
        value.IsT0 ? whenT0(value.AsT0) : whenT1(value.AsT1);

    /// <summary>
    /// Asynchronously executes an action for the active <c>T0</c> branch and returns the original union unchanged.
    /// </summary>
    public static async Task<Unio<T0, T1>> TapT0Async<T0, T1>(
        this Unio<T0, T1> value,
        Func<T0, Task> tap)
    {
        if (value.IsT0)
        {
            T0 t0 = value.AsT0;
            await tap(t0).ConfigureAwait(false);
        }

        return value;
    }

    /// <summary>
    /// Asynchronously executes an action for the active <c>T1</c> branch and returns the original union unchanged.
    /// </summary>
    public static async Task<Unio<T0, T1>> TapT1Async<T0, T1>(
        this Unio<T0, T1> value,
        Func<T1, Task> tap)
    {
        if (value.IsT1)
        {
            T1 t1 = value.AsT1;
            await tap(t1).ConfigureAwait(false);
        }

        return value;
    }

    /// <summary>
    /// Transforms both branches at once and returns a union with both transformed branch types.
    /// </summary>
    /// <example>
    /// <code>
    /// Unio&lt;int, string&gt; value = "7";
    /// Unio&lt;double, int&gt; mapped = value.BiMap(static i => i * 2.0, static s => s.Length);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut0, TOut1> BiMap<T0, T1, TOut0, TOut1>(
        this Unio<T0, T1> value,
        Func<T0, TOut0> mapT0,
        Func<T1, TOut1> mapT1) =>
        value.Match(
            (MapT0: mapT0, MapT1: mapT1),
            static (maps, t0) => (Unio<TOut0, TOut1>)maps.MapT0(t0),
            static (maps, t1) => (Unio<TOut0, TOut1>)maps.MapT1(t1));

    /// <summary>
    /// Ensures that a <c>T0</c> value satisfies a predicate; otherwise converts it into the <c>T1</c> branch.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> EnsureT0<T0, T1>(
        this Unio<T0, T1> value,
        Func<T0, bool> predicate,
        Func<T0, T1> onFailure)
    {
        if (!value.IsT0)
        {
            return value;
        }

        T0 t0 = value.AsT0;

        return predicate(t0) ? value : (Unio<T0, T1>)onFailure(t0);
    }

    /// <summary>
    /// Ensures that a <c>T1</c> value satisfies a predicate; otherwise converts it into the <c>T0</c> branch.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> EnsureT1<T0, T1>(
        this Unio<T0, T1> value,
        Func<T1, bool> predicate,
        Func<T1, T0> onFailure)
    {
        if (!value.IsT1)
        {
            return value;
        }

        T1 t1 = value.AsT1;

        return predicate(t1) ? value : (Unio<T0, T1>)onFailure(t1);
    }

    /// <summary>
    /// Executes an action for the active <c>T0</c> branch and returns the original union unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> TapT0<T0, T1>(
        this Unio<T0, T1> value,
        Action<T0> tap)
    {
        value.Switch(tap, static _ => { });
        return value;
    }

    /// <summary>
    /// Executes an action for the active <c>T0</c> branch and returns the original union unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> TapT0<TState, T0, T1>(
        this Unio<T0, T1> value,
        TState state,
        Action<TState, T0> tap)
    {
        value.Switch(state, tap, static (_, _) => { });
        return value;
    }

    /// <summary>
    /// Executes an action for the active <c>T1</c> branch and returns the original union unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> TapT1<T0, T1>(
        this Unio<T0, T1> value,
        Action<T1> tap)
    {
        value.Switch(static _ => { }, tap);
        return value;
    }

    /// <summary>
    /// Executes an action for the active <c>T1</c> branch and returns the original union unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> TapT1<TState, T0, T1>(
        this Unio<T0, T1> value,
        TState state,
        Action<TState, T1> tap)
    {
        value.Switch(state, static (_, _) => { }, tap);
        return value;
    }

    /// <summary>
    /// Recovers a <c>T1</c> value into <c>T0</c>, collapsing the union into the success branch type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T0 RecoverT1<T0, T1>(
        this Unio<T0, T1> value,
        Func<T1, T0> recover) =>
        value.Match(static t0 => t0, recover);

    /// <summary>
    /// Recovers a <c>T1</c> value into <c>T0</c> using caller state to avoid captures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T0 RecoverT1<TState, T0, T1>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T1, T0> recover) =>
        value.Match(state, static (_, t0) => t0, recover);

    /// <summary>
    /// Recovers a <c>T0</c> value into <c>T1</c>, collapsing the union into the second branch type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T1 RecoverT0<T0, T1>(
        this Unio<T0, T1> value,
        Func<T0, T1> recover) =>
        value.Match(recover, static t1 => t1);

    /// <summary>
    /// Recovers a <c>T0</c> value into <c>T1</c> using caller state to avoid captures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T1 RecoverT0<TState, T0, T1>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T0, T1> recover) =>
        value.Match(state, recover, static (_, t1) => t1);

    /// <summary>
    /// Folds both branches into a shared result type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Fold<T0, T1, TResult>(
        this Unio<T0, T1> value,
        Func<T0, TResult> whenT0,
        Func<T1, TResult> whenT1) =>
        value.Match(whenT0, whenT1);

    /// <summary>
    /// Folds both branches into a shared result type while passing caller state to avoid captures.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Fold<TState, T0, T1, TResult>(
        this Unio<T0, T1> value,
        TState state,
        Func<TState, T0, TResult> whenT0,
        Func<TState, T1, TResult> whenT1) =>
        value.Match(state, whenT0, whenT1);

    /// <summary>
    /// Executes an action when the union contains <c>T0</c> and returns whether the action ran.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool OnT0<T0, T1>(this Unio<T0, T1> value, Action<T0> action)
    {
        if (!value.IsT0)
        {
            return false;
        }

        T0 t0 = value.AsT0;

        action(t0);
        return true;
    }

    /// <summary>
    /// Executes an action when the union contains <c>T1</c> and returns whether the action ran.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool OnT1<T0, T1>(this Unio<T0, T1> value, Action<T1> action)
    {
        if (!value.IsT1)
        {
            return false;
        }

        T1 t1 = value.AsT1;

        action(t1);
        return true;
    }

    /// <summary>
    /// Returns the <c>T0</c> value when present, otherwise computes it from the <c>T1</c> remainder.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T0 PickT0Or<T0, T1>(this Unio<T0, T1> value, Func<T1, T0> fallback) =>
        value.IsT0 ? value.AsT0 : fallback(value.AsT1);

    /// <summary>
    /// Returns the <c>T1</c> value when present, otherwise computes it from the <c>T0</c> remainder.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T1 PickT1Or<T0, T1>(this Unio<T0, T1> value, Func<T0, T1> fallback) =>
        value.IsT1 ? value.AsT1 : fallback(value.AsT0);

    /// <summary>
    /// Converts <c>Unio&lt;T0, T1&gt;</c> into <c>Unio&lt;Result&lt;T0&gt;, Error&lt;T1&gt;&gt;</c>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<Result<T0>, Error<T1>> ToResult<T0, T1>(this Unio<T0, T1> value) =>
        value.Match(
            static t0 => (Unio<Result<T0>, Error<T1>>)new Result<T0>(t0),
            static t1 => (Unio<Result<T0>, Error<T1>>)new Error<T1>(t1));

    /// <summary>
    /// Converts <c>Unio&lt;Result&lt;T0&gt;, Error&lt;T1&gt;&gt;</c> back to <c>Unio&lt;T0, T1&gt;</c>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<T0, T1> FromResult<T0, T1>(this Unio<Result<T0>, Error<T1>> value) =>
        value.Match(
            static ok => (Unio<T0, T1>)ok.Value,
            static err => (Unio<T0, T1>)err.Value);

    /// <summary>
    /// LINQ <c>select</c> support for the <c>T0</c> branch.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> Select<T0, T1, TOut>(this Unio<T0, T1> value, Func<T0, TOut> selector) =>
        value.MapT0(selector);

    /// <summary>
    /// LINQ <c>select many</c> support for composing <c>Unio&lt;T0, T1&gt;</c> values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unio<TOut, T1> SelectMany<T0, T1, TMid, TOut>(
        this Unio<T0, T1> value,
        Func<T0, Unio<TMid, T1>> bind,
        Func<T0, TMid, TOut> project)
    {
        if (!value.IsT0)
        {
            return (Unio<TOut, T1>)value.AsT1;
        }

        T0 t0 = value.AsT0;

        Unio<TMid, T1> bound = bind(t0);
        return bound.Match(
            (Left: t0, Project: project),
            static (ctx, mid) => (Unio<TOut, T1>)ctx.Project(ctx.Left, mid),
            static (_, right) => (Unio<TOut, T1>)right);
    }
}

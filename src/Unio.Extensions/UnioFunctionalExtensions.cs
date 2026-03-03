// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

#nullable enable

using System.Runtime.CompilerServices;

namespace Unio.Extensions;

/// <summary>
/// Provides functional extension methods for <see cref="Unio{T0, T1}"/> to simplify fluent pipelines.
/// </summary>
public static class UnioFunctionalExtensions
{
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
}

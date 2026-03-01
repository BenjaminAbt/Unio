// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Unio.Benchmarks;

/// <summary>
/// Comprehensive comparison of Unio (readonly struct) vs OneOf (class-based)
/// across creation, property access, Try methods, Match and Switch operations.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class UnioVsOneOfBenchmarks
{
    private Unio<int, string> _unio2 = 42;
    private OneOf.OneOf<int, string> _oneOf2;

    private Unio<int, string, bool, double, long> _unio5 = 42;
    private OneOf.OneOf<int, string, bool, double, long> _oneOf5;

    private int _sink;
    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        _unio2 = 42;
        _oneOf2 = 42;
        _unio5 = 42;
        _oneOf5 = 42;
    }

    // ──────────────────────────────────────────────
    //  Creation
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Creation"), Benchmark(Baseline = true)]
    public Unio<int, string> Unio_Create_Int()
        => 42;

    [BenchmarkCategory("Creation"), Benchmark]
    public OneOf.OneOf<int, string> OneOf_Create_Int()
        => 42;

    [BenchmarkCategory("Creation_String"), Benchmark(Baseline = true)]
    public Unio<int, string> Unio_Create_String()
        => "hello";

    [BenchmarkCategory("Creation_String"), Benchmark]
    public OneOf.OneOf<int, string> OneOf_Create_String()
        => "hello";

    [BenchmarkCategory("Creation_5Arity"), Benchmark(Baseline = true)]
    public Unio<int, string, bool, double, long> Unio_Create_5Arity()
        => 42;

    [BenchmarkCategory("Creation_5Arity"), Benchmark]
    public OneOf.OneOf<int, string, bool, double, long> OneOf_Create_5Arity()
        => 42;

    // ──────────────────────────────────────────────
    //  Access – AsT0
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Access_AsT0"), Benchmark(Baseline = true)]
    public int Unio_AsT0()
        => _unio2.AsT0;

    [BenchmarkCategory("Access_AsT0"), Benchmark]
    public int OneOf_AsT0()
        => _oneOf2.AsT0;

    // ──────────────────────────────────────────────
    //  Access – IsT0
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Access_IsT0"), Benchmark(Baseline = true)]
    public bool Unio_IsT0()
        => _unio2.IsT0;

    [BenchmarkCategory("Access_IsT0"), Benchmark]
    public bool OneOf_IsT0()
        => _oneOf2.IsT0;

    // ──────────────────────────────────────────────
    //  Access – Index
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Access_Index"), Benchmark(Baseline = true)]
    public int Unio_Index()
        => _unio2.Index;

    [BenchmarkCategory("Access_Index"), Benchmark]
    public int OneOf_Index()
        => _oneOf2.Index;

    // ──────────────────────────────────────────────
    //  Access – Value (object)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Access_Value"), Benchmark(Baseline = true)]
    public object Unio_Value()
        => _unio2.Value;

    [BenchmarkCategory("Access_Value"), Benchmark]
    public object OneOf_Value()
        => _oneOf2.Value;

    // ──────────────────────────────────────────────
    //  Try – TryGetT0 / TryPickT0 (hit)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Try_Hit"), Benchmark(Baseline = true)]
    public bool Unio_TryGetT0_Hit()
        => _unio2.TryGetT0(out _);

    [BenchmarkCategory("Try_Hit"), Benchmark]
    public bool OneOf_TryPickT0_Hit()
        => _oneOf2.TryPickT0(out _, out _);

    // ──────────────────────────────────────────────
    //  Try – TryGetT1 / TryPickT1 (miss)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Try_Miss"), Benchmark(Baseline = true)]
    public bool Unio_TryGetT1_Miss()
        => _unio2.TryGetT1(out _);

    [BenchmarkCategory("Try_Miss"), Benchmark]
    public bool OneOf_TryPickT1_Miss()
        => _oneOf2.TryPickT1(out _, out _);

    // ──────────────────────────────────────────────
    //  Try – 5-arity TryGet / TryPick (hit)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Try_5Arity_Hit"), Benchmark(Baseline = true)]
    public bool Unio_TryGetT0_5Arity()
        => _unio5.TryGetT0(out _);

    [BenchmarkCategory("Try_5Arity_Hit"), Benchmark]
    public bool OneOf_TryPickT0_5Arity()
        => _oneOf5.TryPickT0(out _, out _);

    // ──────────────────────────────────────────────
    //  Try – 5-arity TryGet / TryPick (miss at T4)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Try_5Arity_Miss"), Benchmark(Baseline = true)]
    public bool Unio_TryGetT4_5Arity_Miss()
        => _unio5.TryGetT4(out _);

    [BenchmarkCategory("Try_5Arity_Miss"), Benchmark]
    public bool OneOf_TryPickT4_5Arity_Miss()
        => _oneOf5.TryPickT4(out _, out _);  // OneOf: (out T4 value, out OneOf<T0..T3> remainder)

    // ──────────────────────────────────────────────
    //  Match – 2-arity
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_2Arity"), Benchmark(Baseline = true)]
    public string Unio_Match_2Arity()
        => _unio2.Match(
            i => i.ToString(CultureInfo.InvariantCulture),
            s => s);

    [BenchmarkCategory("Match_2Arity"), Benchmark]
    public string OneOf_Match_2Arity()
        => _oneOf2.Match(
            i => i.ToString(CultureInfo.InvariantCulture),
            s => s);

    // ──────────────────────────────────────────────
    //  Match – 5-arity
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_5Arity"), Benchmark(Baseline = true)]
    public string Unio_Match_5Arity()
        => _unio5.Match(
            i => i.ToString(CultureInfo.InvariantCulture),
            s => s,
            b => b.ToString(CultureInfo.InvariantCulture),
            d => d.ToString(CultureInfo.InvariantCulture),
            l => l.ToString(CultureInfo.InvariantCulture));

    [BenchmarkCategory("Match_5Arity"), Benchmark]
    public string OneOf_Match_5Arity()
        => _oneOf5.Match(
            i => i.ToString(CultureInfo.InvariantCulture),
            s => s,
            b => b.ToString(CultureInfo.InvariantCulture),
            d => d.ToString(CultureInfo.InvariantCulture),
            l => l.ToString(CultureInfo.InvariantCulture));

    // ──────────────────────────────────────────────
    //  Switch – 2-arity
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Switch_2Arity"), Benchmark(Baseline = true)]
    public void Unio_Switch_2Arity()
        => _unio2.Switch(
            i => _sink = i,
            _ => _sink = 0);

    [BenchmarkCategory("Switch_2Arity"), Benchmark]
    public void OneOf_Switch_2Arity()
        => _oneOf2.Switch(
            i => _sink = i,
            _ => _sink = 0);

    // ──────────────────────────────────────────────
    //  Switch – 5-arity
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Switch_5Arity"), Benchmark(Baseline = true)]
    public void Unio_Switch_5Arity()
        => _unio5.Switch(
            i => _sink = i,
            _ => _sink = 0,
            _ => _sink = 0,
            _ => _sink = 0,
            _ => _sink = 0);

    [BenchmarkCategory("Switch_5Arity"), Benchmark]
    public void OneOf_Switch_5Arity()
        => _oneOf5.Switch(
            i => _sink = i,
            _ => _sink = 0,
            _ => _sink = 0,
            _ => _sink = 0,
            _ => _sink = 0);

    // ──────────────────────────────────────────────
    //  Match – 2-arity with state (Unio static lambda vs OneOf capturing lambda)
    // ──────────────────────────────────────────────

    /// <summary>
    /// Unio uses <c>Match&lt;TState, TResult&gt;</c> with a <see langword="static"/> lambda
    /// — no closure object is allocated regardless of the local value.
    /// </summary>
    [BenchmarkCategory("Match_State_2Arity"), Benchmark(Baseline = true)]
    public string Unio_Match_WithState_2Arity()
    {
        int offset = _iteration++;
        return _unio2.Match(offset,
            static (o, i) => (i + o).ToString(CultureInfo.InvariantCulture),
            static (_, s) => s);
    }

    /// <summary>
    /// OneOf has no <c>Match&lt;TState&gt;</c> overload; capturing a local forces a new
    /// closure object on every call.
    /// </summary>
    [BenchmarkCategory("Match_State_2Arity"), Benchmark]
    public string OneOf_Match_CapturingLambda_2Arity()
    {
        int offset = _iteration++;
        return _oneOf2.Match(
            i => (i + offset).ToString(CultureInfo.InvariantCulture),
            s => s);
    }

    // ──────────────────────────────────────────────
    //  Match – 5-arity with state
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_State_5Arity"), Benchmark(Baseline = true)]
    public string Unio_Match_WithState_5Arity()
    {
        int offset = _iteration++;
        return _unio5.Match(offset,
            static (o, i) => (i + o).ToString(CultureInfo.InvariantCulture),
            static (_, s) => s,
            static (o, b) => (o + (b ? 1 : 0)).ToString(CultureInfo.InvariantCulture),
            static (o, d) => (o + d).ToString(CultureInfo.InvariantCulture),
            static (o, l) => (o + l).ToString(CultureInfo.InvariantCulture));
    }

    [BenchmarkCategory("Match_State_5Arity"), Benchmark]
    public string OneOf_Match_CapturingLambda_5Arity()
    {
        int offset = _iteration++;
        return _oneOf5.Match(
            i => (i + offset).ToString(CultureInfo.InvariantCulture),
            s => s,
            b => (offset + (b ? 1 : 0)).ToString(CultureInfo.InvariantCulture),
            d => (offset + d).ToString(CultureInfo.InvariantCulture),
            l => (offset + l).ToString(CultureInfo.InvariantCulture));
    }

    // ──────────────────────────────────────────────
    //  Switch – 2-arity with state
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Switch_State_2Arity"), Benchmark(Baseline = true)]
    public void Unio_Switch_WithState_2Arity()
    {
        int offset = _iteration++;
        _unio2.Switch((b: this, offset),
            static (s, i) => s.b._sink = i + s.offset,
            static (s, _) => s.b._sink = -s.offset);
    }

    [BenchmarkCategory("Switch_State_2Arity"), Benchmark]
    public void OneOf_Switch_CapturingLambda_2Arity()
    {
        int offset = _iteration++;
        _oneOf2.Switch(
            i => _sink = i + offset,
            _ => _sink = -offset);
    }

    // ──────────────────────────────────────────────
    //  Switch – 5-arity with state
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Switch_State_5Arity"), Benchmark(Baseline = true)]
    public void Unio_Switch_WithState_5Arity()
    {
        int offset = _iteration++;
        _unio5.Switch((b: this, offset),
            static (s, i) => s.b._sink = i + s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset);
    }

    [BenchmarkCategory("Switch_State_5Arity"), Benchmark]
    public void OneOf_Switch_CapturingLambda_5Arity()
    {
        int offset = _iteration++;
        _oneOf5.Switch(
            i => _sink = i + offset,
            _ => _sink = -offset,
            _ => _sink = -offset,
            _ => _sink = -offset,
            _ => _sink = -offset);
    }

    // ──────────────────────────────────────────────
    //  Equality
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Equality"), Benchmark(Baseline = true)]
    public bool Unio_Equals()
        => _unio2.Equals(_unio2);

    [BenchmarkCategory("Equality"), Benchmark]
    public bool OneOf_Equals()
        => _oneOf2.Equals(_oneOf2);

    // ──────────────────────────────────────────────
    //  GetHashCode
    // ──────────────────────────────────────────────

    [BenchmarkCategory("GetHashCode"), Benchmark(Baseline = true)]
    public int Unio_GetHashCode()
        => _unio2.GetHashCode();

    [BenchmarkCategory("GetHashCode"), Benchmark]
    public int OneOf_GetHashCode()
        => _oneOf2.GetHashCode();

    // ──────────────────────────────────────────────
    //  ToString
    // ──────────────────────────────────────────────

    [BenchmarkCategory("ToString"), Benchmark(Baseline = true)]
    public string? Unio_ToString()
        => _unio2.ToString();

    [BenchmarkCategory("ToString"), Benchmark]
    public string OneOf_ToString()
        => _oneOf2.ToString();
}

// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

#if NET11_0_OR_GREATER
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Unio.Benchmarks;

/// <summary>
/// Built-in C# 15 2-arity union used for benchmark comparison against <see cref="Unio{T0,T1}"/>.
/// </summary>
public readonly union BuiltInIntOrString(int, string);

/// <summary>
/// Built-in C# 15 5-arity union used for benchmark comparison against <see cref="Unio{T0,T1,T2,T3,T4}"/>.
/// </summary>
public readonly union BuiltInFiveUnion(int, string, bool, double, long);

/// <summary>
/// Comparison of <see cref="Unio{T0,T1}"/> (zero-allocation <c>readonly struct</c> with typed fields)
/// vs. C# 15 built-in <c>union</c> (<c>struct</c> backed by <c>object?</c>).
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class UnioVsBuiltInUnionBenchmarks
{
    private Unio<int, string> _unio2;
    private Unio<int, string> _unio2Other;
    private BuiltInIntOrString _builtIn2;
    private BuiltInIntOrString _builtIn2Other;

    private Unio<int, string, bool, double, long> _unio5;
    private BuiltInFiveUnion _builtIn5;

    private int _iteration;

    [GlobalSetup]
    public void Setup()
    {
        _unio2 = 42;
        _unio2Other = 42;
        _builtIn2 = 42;
        _builtIn2Other = 42;

        _unio5 = 42;
        _builtIn5 = 42;
    }

    // ──────────────────────────────────────────────
    //  Creation – Value Type (int)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Create_ValueType"), Benchmark(Baseline = true)]
    public Unio<int, string> Unio_Create_Int()
        => 42;

    [BenchmarkCategory("Create_ValueType"), Benchmark]
    public BuiltInIntOrString BuiltIn_Create_Int()
        => 42;

    // ──────────────────────────────────────────────
    //  Creation – Reference Type (string)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Create_RefType"), Benchmark(Baseline = true)]
    public Unio<int, string> Unio_Create_String()
        => "hello";

    [BenchmarkCategory("Create_RefType"), Benchmark]
    public BuiltInIntOrString BuiltIn_Create_String()
        => "hello";

    // ──────────────────────────────────────────────
    //  Creation – 5-Arity Value Type (int)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Create_5Arity"), Benchmark(Baseline = true)]
    public Unio<int, string, bool, double, long> Unio_Create_5Arity()
        => 42;

    [BenchmarkCategory("Create_5Arity"), Benchmark]
    public BuiltInFiveUnion BuiltIn_Create_5Arity()
        => 42;

    // ──────────────────────────────────────────────
    //  Type Check & Extract – Hit (int)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("TryGet_Hit"), Benchmark(Baseline = true)]
    public int Unio_TryGetT0_Hit()
        => _unio2.TryGetT0(out int v) ? v : 0;

    [BenchmarkCategory("TryGet_Hit"), Benchmark]
    public int BuiltIn_IsPattern_Hit()
        => _builtIn2 is int v ? v : 0;

    // ──────────────────────────────────────────────
    //  Type Check & Extract – Miss (string)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("TryGet_Miss"), Benchmark(Baseline = true)]
    public int Unio_TryGetT1_Miss()
        => _unio2.TryGetT1(out string? s) ? s.Length : 0;

    [BenchmarkCategory("TryGet_Miss"), Benchmark]
    public int BuiltIn_IsPattern_Miss()
        => _builtIn2 is string s ? s.Length : 0;

    // ──────────────────────────────────────────────
    //  Match – 2-Arity (no state)
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_2Arity"), Benchmark(Baseline = true)]
    public int Unio_Match_2Arity()
        => _unio2.Match(
            static i => i + 1,
            static s => s.Length);

    [BenchmarkCategory("Match_2Arity"), Benchmark]
    public int BuiltIn_Switch_2Arity()
        => _builtIn2 switch
        {
            int i => i + 1,
            string s => s.Length,
            null => 0,
        };

    // ──────────────────────────────────────────────
    //  Match – 2-Arity with local state
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_State_2Arity"), Benchmark(Baseline = true)]
    public int Unio_Match_WithState_2Arity()
    {
        int offset = _iteration++;
        return _unio2.Match(offset,
            static (o, i) => i + o,
            static (o, s) => s.Length + o);
    }

    [BenchmarkCategory("Match_State_2Arity"), Benchmark]
    public int BuiltIn_Switch_WithLocal_2Arity()
    {
        int offset = _iteration++;
        return _builtIn2 switch
        {
            int i => i + offset,
            string s => s.Length + offset,
            null => 0,
        };
    }

    // ──────────────────────────────────────────────
    //  Match – 5-Arity
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Match_5Arity"), Benchmark(Baseline = true)]
    public int Unio_Match_5Arity()
        => _unio5.Match(
            static i => i + 1,
            static s => s.Length,
            static b => b ? 1 : 0,
            static d => (int)d,
            static l => (int)l);

    [BenchmarkCategory("Match_5Arity"), Benchmark]
    public int BuiltIn_Switch_5Arity()
        => _builtIn5 switch
        {
            int i => i + 1,
            string s => s.Length,
            bool b => b ? 1 : 0,
            double d => (int)d,
            long l => (int)l,
            null => 0,
        };

    // ──────────────────────────────────────────────
    //  Round-trip: Create (int) + Match
    // ──────────────────────────────────────────────

    [BenchmarkCategory("RoundTrip_ValueType"), Benchmark(Baseline = true)]
    public int Unio_CreateAndMatch_Int()
    {
        Unio<int, string> union = _iteration++;
        return union.Match(
            static i => i + 1,
            static s => s.Length);
    }

    [BenchmarkCategory("RoundTrip_ValueType"), Benchmark]
    public int BuiltIn_CreateAndSwitch_Int()
    {
        BuiltInIntOrString union = _iteration++;
        return union switch
        {
            int i => i + 1,
            string s => s.Length,
            null => 0,
        };
    }

    // ──────────────────────────────────────────────
    //  Equality
    // ──────────────────────────────────────────────

    [BenchmarkCategory("Equality"), Benchmark(Baseline = true)]
    public bool Unio_Equals()
        => _unio2.Equals(_unio2Other);

    [BenchmarkCategory("Equality"), Benchmark]
    public bool BuiltIn_Equals()
    {
#pragma warning disable MA0065 // Intentional: built-in union does not generate IEquatable<T> and falls back to ValueType.Equals
        return _builtIn2.Equals(_builtIn2Other);
#pragma warning restore MA0065
    }
}
#endif

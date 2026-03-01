// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Unio.Benchmarks;

/// <summary>
/// Benchmarks measuring the cost of creating <see cref="Unio{T0,T1}"/> instances
/// via implicit conversion for various arities.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UnioCreationBenchmarks
{
    /// <summary>Creates a 2-arity union holding an <see cref="int"/> (T0).</summary>
    [Benchmark(Baseline = true)]
    public Unio<int, string> Create_T0_Int()
    {
        return 42;
    }

    /// <summary>Creates a 2-arity union holding a <see cref="string"/> (T1).</summary>
    [Benchmark]
    public Unio<int, string> Create_T1_String()
    {
        return "hello";
    }

    /// <summary>Creates a 5-arity union holding an <see cref="int"/> (T0).</summary>
    [Benchmark]
    public Unio<int, string, bool, double, long> Create_T0_5Arity()
    {
        return 42;
    }

    /// <summary>Creates a 9-arity union holding an <see cref="int"/> (T0).</summary>
    [Benchmark]
    public Unio<int, string, bool, double, long, byte, float, char, decimal> Create_T0_9Arity()
    {
        return 42;
    }
}

/// <summary>
/// Benchmarks measuring the cost of <see cref="Unio{T0,T1}.Match{TResult}"/>
/// for various arities.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UnioMatchBenchmarks
{
    private Unio<int, string> _union2 = 42;
    private Unio<int, string, bool, double, long> _union5 = 42;
    private Unio<int, string, bool, double, long, byte, float, char, decimal> _union9 = 42;

    /// <summary>Initializes the union instances used across match benchmarks.</summary>
    [GlobalSetup]
    public void Setup()
    {
        _union2 = 42;
        _union5 = 42;
        _union9 = 42;
    }

    /// <summary>Matches a 2-arity union.</summary>
    [Benchmark(Baseline = true)]
    public string Match_2Arity()
    {
        return _union2.Match(i => i.ToString(CultureInfo.InvariantCulture), s => s);
    }

    /// <summary>Matches a 5-arity union.</summary>
    [Benchmark]
    public string Match_5Arity()
    {
        return _union5.Match(
            i => i.ToString(CultureInfo.InvariantCulture), s => s, b => b.ToString(CultureInfo.InvariantCulture),
            d => d.ToString(CultureInfo.InvariantCulture), l => l.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Matches a 9-arity union.</summary>
    [Benchmark]
    public string Match_9Arity()
    {
        return _union9.Match(
            i => i.ToString(CultureInfo.InvariantCulture), s => s, b => b.ToString(CultureInfo.InvariantCulture),
            d => d.ToString(CultureInfo.InvariantCulture), l => l.ToString(CultureInfo.InvariantCulture), by => by.ToString(CultureInfo.InvariantCulture),
            f => f.ToString(CultureInfo.InvariantCulture), c => c.ToString(CultureInfo.InvariantCulture), dec => dec.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// Benchmarks measuring the cost of property and method access on <see cref="Unio{T0,T1}"/>:
/// <see cref="Unio{T0,T1}.AsT0"/>, <see cref="Unio{T0,T1}.TryGetT0"/>,
/// <see cref="Unio{T0,T1}.IsT0"/> and <see cref="Unio{T0,T1}.Index"/>.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UnioAccessBenchmarks
{
    private Unio<int, string> _union = 42;

    /// <summary>Initializes the union instance used across access benchmarks.</summary>
    [GlobalSetup]
    public void Setup()
    {
        _union = 42;
    }

    /// <summary>Accesses the stored value via <c>AsT0</c>.</summary>
    [Benchmark(Baseline = true)]
    public int Access_AsT0()
    {
        return _union.AsT0;
    }

    /// <summary>Attempts to retrieve the value via <c>TryGetT0</c>.</summary>
    [Benchmark]
    public bool Access_TryGetT0()
    {
        return _union.TryGetT0(out _);
    }

    /// <summary>Checks the type discriminator via <c>IsT0</c>.</summary>
    [Benchmark]
    public bool Access_IsT0()
    {
        return _union.IsT0;
    }

    /// <summary>Reads the <c>Index</c> property.</summary>
    [Benchmark]
    public int Access_Index()
    {
        return _union.Index;
    }
}

/// <summary>
/// Benchmarks comparing <see cref="Unio{T0,T1}.Match{TResult}"/> with a capturing lambda
/// (allocates a new closure object per call when the captured local changes)
/// against the allocation-free <see cref="Unio{T0,T1}.Match{TState,TResult}"/> overload
/// using a <see langword="static"/> lambda.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UnioMatchWithStateBenchmarks
{
    private Unio<int, string> _union2 = 42;
    private Unio<int, string, bool, double, long> _union5 = 42;
    private Unio<int, string, bool, double, long, byte, float, char, decimal> _union9 = 42;
    private int _iteration;

    /// <summary>Initializes the union instances used across benchmarks.</summary>
    [GlobalSetup]
    public void Setup()
    {
        _union2 = 42;
        _union5 = 42;
        _union9 = 42;
    }

    // ── 2-arity ──

    /// <summary>Matches a 2-arity union via a capturing lambda (allocates a new closure per call).</summary>
    [Benchmark(Baseline = true)]
    public string Match_2Arity_CapturingLambda()
    {
        int offset = _iteration++;
        return _union2.Match(
            i => (i + offset).ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Matches a 2-arity union via a static lambda + state object (zero closure allocation).</summary>
    [Benchmark]
    public string Match_WithState_2Arity()
    {
        int offset = _iteration++;
        return _union2.Match(offset,
            static (o, i) => (i + o).ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture));
    }

    // ── 5-arity ──

    /// <summary>Matches a 5-arity union via a capturing lambda (allocates a new closure per call).</summary>
    [Benchmark]
    public string Match_5Arity_CapturingLambda()
    {
        int offset = _iteration++;
        return _union5.Match(
            i => (i + offset).ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Matches a 5-arity union via a static lambda + state object (zero closure allocation).</summary>
    [Benchmark]
    public string Match_WithState_5Arity()
    {
        int offset = _iteration++;
        return _union5.Match(offset,
            static (o, i) => (i + o).ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture));
    }

    // ── 9-arity ──

    /// <summary>Matches a 9-arity union via a capturing lambda (allocates a new closure per call).</summary>
    [Benchmark]
    public string Match_9Arity_CapturingLambda()
    {
        int offset = _iteration++;
        return _union9.Match(
            i => (i + offset).ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture),
            _ => offset.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>Matches a 9-arity union via a static lambda + state object (zero closure allocation).</summary>
    [Benchmark]
    public string Match_WithState_9Arity()
    {
        int offset = _iteration++;
        return _union9.Match(offset,
            static (o, i) => (i + o).ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture),
            static (o, _) => o.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// Benchmarks comparing <see cref="Unio{T0,T1}.Switch"/> with a capturing lambda
/// (allocates a new closure object per call when the captured local changes)
/// against the allocation-free <see cref="Unio{T0,T1}.Switch{TState}"/> overload
/// using a <see langword="static"/> lambda.
/// </summary>
[MemoryDiagnoser]
[ShortRunJob]
public class UnioSwitchWithStateBenchmarks
{
    private Unio<int, string> _union2 = 42;
    private Unio<int, string, bool, double, long> _union5 = 42;
    private int _sink;
    private int _iteration;

    /// <summary>Initializes the union instances used across benchmarks.</summary>
    [GlobalSetup]
    public void Setup()
    {
        _union2 = 42;
        _union5 = 42;
    }

    // ── 2-arity ──

    /// <summary>Dispatches a 2-arity union via a capturing lambda (allocates a new closure per call).</summary>
    [Benchmark(Baseline = true)]
    public void Switch_2Arity_CapturingLambda()
    {
        int offset = _iteration++;
        _union2.Switch(
            i => _sink = i + offset,
            _ => _sink = -offset);
    }

    /// <summary>Dispatches a 2-arity union via a static lambda + state object (zero closure allocation).</summary>
    [Benchmark]
    public void Switch_WithState_2Arity()
    {
        int offset = _iteration++;
        _union2.Switch((b: this, offset),
            static (s, i) => s.b._sink = i + s.offset,
            static (s, _) => s.b._sink = -s.offset);
    }

    // ── 5-arity ──

    /// <summary>Dispatches a 5-arity union via a capturing lambda (allocates a new closure per call).</summary>
    [Benchmark]
    public void Switch_5Arity_CapturingLambda()
    {
        int offset = _iteration++;
        _union5.Switch(
            i => _sink = i + offset,
            _ => _sink = -offset,
            _ => _sink = -offset,
            _ => _sink = -offset,
            _ => _sink = -offset);
    }

    /// <summary>Dispatches a 5-arity union via a static lambda + state object (zero closure allocation).</summary>
    [Benchmark]
    public void Switch_WithState_5Arity()
    {
        int offset = _iteration++;
        _union5.Switch((b: this, offset),
            static (s, i) => s.b._sink = i + s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset,
            static (s, _) => s.b._sink = -s.offset);
    }
}

// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Reflection;
using System.Text;

namespace Unio.UnitTests;

/// <summary>
/// Exhaustive behavioral and branch-coverage tests across all generated union arities (2–20)
/// for both <c>Unio&lt;...&gt;</c> value types and <c>UnioBase&lt;...&gt;</c> abstract classes.
/// </summary>
public class UnioAllAritiesTests
{
    private static readonly Type[] s_allBranchTypes =
    [
        typeof(B0), typeof(B1), typeof(B2), typeof(B3), typeof(B4),
        typeof(B5), typeof(B6), typeof(B7), typeof(B8), typeof(B9),
        typeof(B10), typeof(B11), typeof(B12), typeof(B13), typeof(B14),
        typeof(B15), typeof(B16), typeof(B17), typeof(B18), typeof(B19)
    ];

    private static readonly Type[] s_openUnioTypes =
    [
        typeof(Unio<,>),
        typeof(Unio<,,>),
        typeof(Unio<,,,>),
        typeof(Unio<,,,,>),
        typeof(Unio<,,,,,>),
        typeof(Unio<,,,,,,>),
        typeof(Unio<,,,,,,,>),
        typeof(Unio<,,,,,,,,>),
        typeof(Unio<,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,,,,,>),
        typeof(Unio<,,,,,,,,,,,,,,,,,,,>)
    ];

    private static readonly Type[] s_concreteBaseTypes =
    [
        typeof(TestBase2), typeof(TestBase3), typeof(TestBase4), typeof(TestBase5), typeof(TestBase6),
        typeof(TestBase7), typeof(TestBase8), typeof(TestBase9), typeof(TestBase10), typeof(TestBase11),
        typeof(TestBase12), typeof(TestBase13), typeof(TestBase14), typeof(TestBase15), typeof(TestBase16),
        typeof(TestBase17), typeof(TestBase18), typeof(TestBase19), typeof(TestBase20)
    ];

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(20)]
    public async Task UnioAndUnioBase_AllMembersAndBranches_BehaveCorrectly(int arity)
    {
        Type[] branchTypes = s_allBranchTypes[..arity];
        Type unioType = s_openUnioTypes[arity - 2].MakeGenericType(branchTypes);
        Type concreteBaseType = s_concreteBaseTypes[arity - 2];
        Type abstractBaseType = concreteBaseType.BaseType!;
        ConstructorInfo baseCtor = concreteBaseType.GetConstructors()[0];

        MethodInfo[] implicitOps = new MethodInfo[arity];
        foreach (MethodInfo m in unioType.GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (string.Equals(m.Name, "op_Implicit", StringComparison.Ordinal))
            {
                Type paramType = m.GetParameters()[0].ParameterType;
                int idx = Array.IndexOf(branchTypes, paramType);
                implicitOps[idx] = m;
            }
        }

        object CreateUnion(int index, object? branchValue) => implicitOps[index].Invoke(null, [branchValue])!;
        object CreateBase(object unionVal) => baseCtor.Invoke([unionVal]);

        BranchBase[] valuesA = new BranchBase[arity];
        BranchBase[] valuesB = new BranchBase[arity];
        BranchBase[] valuesNullStr = new BranchBase[arity];
        BranchBase[] valuesFormattable = new BranchBase[arity];
        for (int i = 0; i < arity; i++)
        {
            valuesA[i] = CreatePlainBranch(i, id: 10 + i, nullText: false);
            valuesB[i] = CreatePlainBranch(i, id: 50 + i, nullText: false);
            valuesNullStr[i] = CreatePlainBranch(i, id: 90 + i, nullText: true);
            valuesFormattable[i] = CreateFormattableBranch(i, id: 10 + i);
        }

        object[] unionsA = new object[arity];
        object[] unionsB = new object[arity];
        object[] basesA = new object[arity];
        object[] basesB = new object[arity];
        for (int i = 0; i < arity; i++)
        {
            unionsA[i] = CreateUnion(i, valuesA[i]);
            unionsB[i] = CreateUnion(i, valuesB[i]);
            basesA[i] = CreateBase(unionsA[i]);
            basesB[i] = CreateBase(unionsB[i]);
        }

        await ExerciseCommonOperationsAsync(unioType, unionsA, valuesA, arity, isStruct: true);
        await ExerciseCommonOperationsAsync(abstractBaseType, basesA, valuesA, arity, isStruct: false);

        // Equality, operators, and hashing on Unio<...>
        MethodInfo unioEqualsTyped = unioType.GetMethod("Equals", [unioType])!;
        MethodInfo unioEqualsObj = unioType.GetMethod("Equals", [typeof(object)])!;
        MethodInfo eqOp = unioType.GetMethod("op_Equality", BindingFlags.Public | BindingFlags.Static)!;
        MethodInfo neqOp = unioType.GetMethod("op_Inequality", BindingFlags.Public | BindingFlags.Static)!;

        Assert.False((bool)unioEqualsTyped.Invoke(unionsA[0], [unionsA[1]])!);
        Assert.False((bool)unioEqualsObj.Invoke(unionsA[0], [null])!);
        Assert.False((bool)unioEqualsObj.Invoke(unionsA[0], ["other"])!);
        Assert.True((bool)eqOp.Invoke(null, [unionsA[0], CreateUnion(0, CreatePlainBranch(0, 10, false))])!);
        Assert.False((bool)eqOp.Invoke(null, [unionsA[0], unionsA[1]])!);
        Assert.False((bool)neqOp.Invoke(null, [unionsA[0], CreateUnion(0, CreatePlainBranch(0, 10, false))])!);
        Assert.True((bool)neqOp.Invoke(null, [unionsA[0], unionsA[1]])!);

        for (int i = 0; i < arity; i++)
        {
            object same = CreateUnion(i, CreatePlainBranch(i, 10 + i, false));
            Assert.True((bool)unioEqualsTyped.Invoke(unionsA[i], [same])!);
            Assert.False((bool)unioEqualsTyped.Invoke(unionsA[i], [unionsB[i]])!);
            Assert.True((bool)unioEqualsObj.Invoke(unionsA[i], [same])!);
            Assert.False((bool)unioEqualsObj.Invoke(unionsA[i], [unionsB[i]])!);
            Assert.Equal(unionsA[i].GetHashCode(), same.GetHashCode());
        }

        // Equality and hashing on UnioBase<...>
        MethodInfo baseEqualsTyped = abstractBaseType.GetMethod("Equals", [abstractBaseType])!;
        MethodInfo baseEqualsObj = abstractBaseType.GetMethod("Equals", [typeof(object)])!;
        object sameBase0 = CreateBase(CreateUnion(0, CreatePlainBranch(0, 10, false)));
        Assert.True((bool)baseEqualsTyped.Invoke(basesA[0], [sameBase0])!);
        Assert.False((bool)baseEqualsTyped.Invoke(basesA[0], [null])!);
        Assert.False((bool)baseEqualsTyped.Invoke(basesA[0], [basesB[0]])!);
        Assert.True((bool)baseEqualsObj.Invoke(basesA[0], [sameBase0])!);
        Assert.False((bool)baseEqualsObj.Invoke(basesA[0], [basesB[0]])!);
        Assert.False((bool)baseEqualsObj.Invoke(basesA[0], [null])!);
        Assert.False((bool)baseEqualsObj.Invoke(basesA[0], ["other"])!);
        Assert.Equal(basesA[0].GetHashCode(), sameBase0.GetHashCode());

        // Formatting (ToString, IFormattable, ISpanFormattable, IUtf8SpanFormattable) across all branches and modes
        for (int i = 0; i < arity; i++)
        {
            object plainUnion = unionsA[i];
            object nullValueUnion = CreateUnion(i, null);
            object nullToStringUnion = CreateUnion(i, valuesNullStr[i]);
            object formattableUnion = CreateUnion(i, valuesFormattable[i]);

            Assert.Equal(string.Create(CultureInfo.InvariantCulture, $"B{i}:{10 + i}"), plainUnion.ToString());
            Assert.Equal(string.Empty, nullValueUnion.ToString());
            Assert.Equal(string.Empty, nullToStringUnion.ToString());

            Assert.Equal(string.Create(CultureInfo.InvariantCulture, $"B{i}:{10 + i}"), ((IFormattable)plainUnion).ToString("X", CultureInfo.InvariantCulture));
            Assert.Equal(string.Empty, ((IFormattable)nullValueUnion).ToString("X", CultureInfo.InvariantCulture));
            Assert.Equal(string.Empty, ((IFormattable)nullToStringUnion).ToString("X", CultureInfo.InvariantCulture));
            Assert.Equal(string.Create(CultureInfo.InvariantCulture, $"FMT{i}:{10 + i}:X"), ((IFormattable)formattableUnion).ToString("X", CultureInfo.InvariantCulture));

            ExerciseSpanFormatting(plainUnion, expectedEmptyFmt: string.Create(CultureInfo.InvariantCulture, $"B{i}:{10 + i}"));
            ExerciseSpanFormatting(formattableUnion, expectedEmptyFmt: string.Create(CultureInfo.InvariantCulture, $"SF{i}:{10 + i}:"));

            // Also exercise UnioBase formatting pass-throughs on i == 0
            if (i == 0)
            {
                object baseObj = basesA[0];
                Assert.Equal("B0:10", baseObj.ToString());
                Assert.Equal("B0:10", ((IFormattable)baseObj).ToString("G", CultureInfo.InvariantCulture));
                ExerciseSpanFormatting(baseObj, expectedEmptyFmt: "B0:10");
            }
        }

        // Corrupt discriminator (_index = 255) to verify all defensive InvalidOperationException / fallback arms
        ExerciseCorruptedState(unioType, branchTypes, arity);
    }

    private static async Task ExerciseCommonOperationsAsync(
        Type targetType,
        object[] instances,
        BranchBase[] values,
        int arity,
        bool isStruct)
    {
        Type[] branchTypes = s_allBranchTypes[..arity];
        PropertyInfo indexProp = targetType.GetProperty("Index")!;
        PropertyInfo valueProp = targetType.GetProperty("Value")!;

        (MethodInfo match, MethodInfo matchState, MethodInfo matchAsync, MethodInfo matchStateAsync,
            MethodInfo sw, MethodInfo swState, MethodInfo swAsync, MethodInfo swStateAsync) = ResolveMatchAndSwitchMethods(targetType, arity);

        object[] matchFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchFunc));
        object[] matchStateFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchStateFunc));
        object[] matchAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchAsyncFunc));
        object[] matchStateAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchStateAsyncFunc));

        int[] switchSink = [0];
        object[] switchActions = BuildArrayWithArg(arity, branchTypes, nameof(MakeSwitchAction), switchSink);
        object[] switchStateActions = BuildArray(arity, branchTypes, nameof(MakeSwitchStateAction));
        object[] switchAsyncFuncs = BuildArrayWithArg(arity, branchTypes, nameof(MakeSwitchAsyncFunc), switchSink);
        object[] switchStateAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeSwitchStateAsyncFunc));

        object[] matchStateArgs = new object[arity + 1];
        matchStateArgs[0] = 1000;
        Array.Copy(matchStateFuncs, 0, matchStateArgs, 1, arity);

        object[] matchStateAsyncArgs = new object[arity + 1];
        matchStateAsyncArgs[0] = 2000;
        Array.Copy(matchStateAsyncFuncs, 0, matchStateAsyncArgs, 1, arity);

        int[] stateSink = [0];
        object[] swStateArgs = new object[arity + 1];
        swStateArgs[0] = stateSink;
        Array.Copy(switchStateActions, 0, swStateArgs, 1, arity);

        object[] swStateAsyncArgs = new object[arity + 1];
        swStateAsyncArgs[0] = stateSink;
        Array.Copy(switchStateAsyncFuncs, 0, swStateAsyncArgs, 1, arity);

        for (int active = 0; active < arity; active++)
        {
            object inst = instances[active];
            Assert.Equal(active, (int)indexProp.GetValue(inst)!);
            Assert.Same(values[active], valueProp.GetValue(inst));

            Assert.Equal((active * 100) + values[active].Id, (int)match.Invoke(inst, matchFuncs)!);
            Assert.Equal(1000 + (active * 100) + values[active].Id, (int)matchState.Invoke(inst, matchStateArgs)!);
            Assert.Equal((active * 100) + values[active].Id, await ((Task<int>)matchAsync.Invoke(inst, matchAsyncFuncs)!).ConfigureAwait(false));
            Assert.Equal(2000 + (active * 100) + values[active].Id, await ((Task<int>)matchStateAsync.Invoke(inst, matchStateAsyncArgs)!).ConfigureAwait(false));

            switchSink[0] = -1;
            sw.Invoke(inst, switchActions);
            Assert.Equal(active, switchSink[0]);

            stateSink[0] = -1;
            swState.Invoke(inst, swStateArgs);
            Assert.Equal(active + 100, stateSink[0]);

            switchSink[0] = -1;
            await ((Task)swAsync.Invoke(inst, switchAsyncFuncs)!).ConfigureAwait(false);
            Assert.Equal(active + 200, switchSink[0]);

            stateSink[0] = -1;
            await ((Task)swStateAsync.Invoke(inst, swStateAsyncArgs)!).ConfigureAwait(false);
            Assert.Equal(active + 300, stateSink[0]);

            for (int target = 0; target < arity; target++)
            {
                PropertyInfo isProp = targetType.GetProperty(string.Create(CultureInfo.InvariantCulture, $"IsT{target}"))!;
                PropertyInfo asProp = targetType.GetProperty(string.Create(CultureInfo.InvariantCulture, $"AsT{target}"))!;
                MethodInfo tryGet = targetType.GetMethod(string.Create(CultureInfo.InvariantCulture, $"TryGetT{target}"))!;
                MethodInfo tryPick = targetType.GetMethod(string.Create(CultureInfo.InvariantCulture, $"TryPickT{target}"))!;
                MethodInfo mapMethod = targetType.GetMethod(string.Create(CultureInfo.InvariantCulture, $"MapT{target}"))!.MakeGenericMethod(typeof(string));

                MethodInfo[] valueOrOverloads = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => string.Equals(m.Name, string.Create(CultureInfo.InvariantCulture, $"ValueOrT{target}"), StringComparison.Ordinal))
                    .ToArray();
                MethodInfo valueOrDirect = valueOrOverloads.Single(static m => !m.IsGenericMethodDefinition && m.GetParameters()[0].ParameterType.BaseType == typeof(BranchBase));
                MethodInfo valueOrFactory = valueOrOverloads.Single(static m => !m.IsGenericMethodDefinition && m.GetParameters()[0].ParameterType.IsGenericType);
                MethodInfo valueOrState = valueOrOverloads.Single(static m => m.IsGenericMethodDefinition).MakeGenericMethod(typeof(int));

                BranchBase fallbackVal = CreatePlainBranch(target, 777, false);
                object fallbackFactory = InvokeHelper(nameof(MakeFallbackFactory), branchTypes[target], fallbackVal);
                object fallbackStateFactory = InvokeHelper(nameof(MakeFallbackStateFactory), branchTypes[target], target);
                object mapDelegate = InvokeHelper(nameof(MakeMapFunc), branchTypes[target], target);

                object?[] tryGetArgs = [null];
                bool got = (bool)tryGet.Invoke(inst, tryGetArgs)!;

                object?[] tryPickArgs = [null, null];
                bool picked = (bool)tryPick.Invoke(inst, tryPickArgs)!;

                object mapped = mapMethod.Invoke(inst, [mapDelegate])!;
                int mappedIndex = (int)mapped.GetType().GetProperty("Index")!.GetValue(mapped)!;
                Assert.Equal(active, mappedIndex);

                if (active == target)
                {
                    Assert.True((bool)isProp.GetValue(inst)!);
                    Assert.Same(values[active], asProp.GetValue(inst));
                    Assert.True(got);
                    Assert.Same(values[active], tryGetArgs[0]);
                    Assert.True(picked);
                    Assert.Same(values[active], tryPickArgs[0]);
                    Assert.Same(values[active], valueOrDirect.Invoke(inst, [fallbackVal]));
                    Assert.Same(values[active], valueOrFactory.Invoke(inst, [fallbackFactory]));
                    Assert.Same(values[active], valueOrState.Invoke(inst, [99, fallbackStateFactory]));
                }
                else
                {
                    Assert.False((bool)isProp.GetValue(inst)!);
                    if (isStruct)
                    {
                        TargetInvocationException tie = Assert.Throws<TargetInvocationException>(() => asProp.GetValue(inst));
                        Assert.IsType<InvalidOperationException>(tie.InnerException);
                    }
                    else if (active == 0)
                    {
                        TargetInvocationException tie = Assert.Throws<TargetInvocationException>(() => asProp.GetValue(inst));
                        Assert.IsType<InvalidOperationException>(tie.InnerException);
                    }

                    Assert.False(got);
                    Assert.Null(tryGetArgs[0]);
                    Assert.False(picked);
                    Assert.NotNull(tryPickArgs[1]);
                    Assert.Same(fallbackVal, valueOrDirect.Invoke(inst, [fallbackVal]));
                    Assert.Same(fallbackVal, valueOrFactory.Invoke(inst, [fallbackFactory]));
                    BranchBase stateCreated = (BranchBase)valueOrState.Invoke(inst, [99, fallbackStateFactory])!;
                    Assert.Equal(99, stateCreated.Id);
                }
            }
        }
    }

    private static void ExerciseCorruptedState(Type unioType, Type[] branchTypes, int arity)
    {
        FieldInfo indexField = unioType.GetField("_index", BindingFlags.NonPublic | BindingFlags.Instance)!;
        object corruptA = Activator.CreateInstance(unioType)!;
        indexField.SetValue(corruptA, (byte)255);
        object corruptB = Activator.CreateInstance(unioType)!;
        indexField.SetValue(corruptB, (byte)255);

        PropertyInfo valueProp = unioType.GetProperty("Value")!;
        AssertThrowsInvalidOp(() => valueProp.GetValue(corruptA));

        (MethodInfo match, MethodInfo matchState, MethodInfo matchAsync, MethodInfo matchStateAsync,
            MethodInfo sw, MethodInfo swState, MethodInfo swAsync, MethodInfo swStateAsync) = ResolveMatchAndSwitchMethods(unioType, arity);

        object[] matchFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchFunc));
        object[] matchStateFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchStateFunc));
        object[] matchAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchAsyncFunc));
        object[] matchStateAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeMatchStateAsyncFunc));
        int[] sink = [0];
        object[] switchActions = BuildArrayWithArg(arity, branchTypes, nameof(MakeSwitchAction), sink);
        object[] switchStateActions = BuildArray(arity, branchTypes, nameof(MakeSwitchStateAction));
        object[] switchAsyncFuncs = BuildArrayWithArg(arity, branchTypes, nameof(MakeSwitchAsyncFunc), sink);
        object[] switchStateAsyncFuncs = BuildArray(arity, branchTypes, nameof(MakeSwitchStateAsyncFunc));

        object[] matchStateArgs = new object[arity + 1];
        matchStateArgs[0] = 1;
        Array.Copy(matchStateFuncs, 0, matchStateArgs, 1, arity);

        object[] matchStateAsyncArgs = new object[arity + 1];
        matchStateAsyncArgs[0] = 1;
        Array.Copy(matchStateAsyncFuncs, 0, matchStateAsyncArgs, 1, arity);

        object[] swStateArgs = new object[arity + 1];
        swStateArgs[0] = sink;
        Array.Copy(switchStateActions, 0, swStateArgs, 1, arity);

        object[] swStateAsyncArgs = new object[arity + 1];
        swStateAsyncArgs[0] = sink;
        Array.Copy(switchStateAsyncFuncs, 0, swStateAsyncArgs, 1, arity);

        AssertThrowsInvalidOp(() => match.Invoke(corruptA, matchFuncs));
        AssertThrowsInvalidOp(() => matchState.Invoke(corruptA, matchStateArgs));
        AssertThrowsInvalidOp(() => matchAsync.Invoke(corruptA, matchAsyncFuncs));
        AssertThrowsInvalidOp(() => matchStateAsync.Invoke(corruptA, matchStateAsyncArgs));
        AssertThrowsInvalidOp(() => sw.Invoke(corruptA, switchActions));
        AssertThrowsInvalidOp(() => swState.Invoke(corruptA, swStateArgs));
        AssertThrowsInvalidOp(() => swAsync.Invoke(corruptA, switchAsyncFuncs));
        AssertThrowsInvalidOp(() => swStateAsync.Invoke(corruptA, swStateAsyncArgs));

        for (int target = 0; target < arity; target++)
        {
            MethodInfo tryPick = unioType.GetMethod(string.Create(CultureInfo.InvariantCulture, $"TryPickT{target}"))!;
            AssertThrowsInvalidOp(() => tryPick.Invoke(corruptA, [null, null]));

            MethodInfo mapMethod = unioType.GetMethod(string.Create(CultureInfo.InvariantCulture, $"MapT{target}"))!.MakeGenericMethod(typeof(string));
            object mapDelegate = InvokeHelper(nameof(MakeMapFunc), branchTypes[target], target);
            AssertThrowsInvalidOp(() => mapMethod.Invoke(corruptA, [mapDelegate]));
        }

        MethodInfo equalsTyped = unioType.GetMethod("Equals", [unioType])!;
        Assert.False((bool)equalsTyped.Invoke(corruptA, [corruptB])!);
        Assert.Equal(255 * 397, corruptA.GetHashCode());
        Assert.Equal(string.Empty, corruptA.ToString());
        Assert.Equal(string.Empty, ((IFormattable)corruptA).ToString("G", CultureInfo.InvariantCulture));
        ExerciseSpanFormatting(corruptA, expectedEmptyFmt: string.Empty);
    }

    private static void ExerciseSpanFormatting(object formattableInstance, string expectedEmptyFmt)
    {
        ISpanFormattable sf = (ISpanFormattable)formattableInstance;
        IUtf8SpanFormattable uf = (IUtf8SpanFormattable)formattableInstance;

        Span<char> charBuf = stackalloc char[32];
        Assert.True(sf.TryFormat(charBuf, out int charsWritten, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture));
        Assert.Equal(expectedEmptyFmt, new string(charBuf[..charsWritten]));

        Span<byte> utf8Buf = stackalloc byte[32];
        Assert.True(uf.TryFormat(utf8Buf, out int bytesWritten, ReadOnlySpan<char>.Empty, CultureInfo.InvariantCulture));
        Assert.Equal(expectedEmptyFmt, Encoding.UTF8.GetString(utf8Buf[..bytesWritten]));

        if (expectedEmptyFmt.Length > 0)
        {
            Assert.False(sf.TryFormat(Span<char>.Empty, out int zeroChars, "G", CultureInfo.InvariantCulture));
            Assert.Equal(0, zeroChars);

            Assert.False(uf.TryFormat(Span<byte>.Empty, out int zeroBytes, "G", CultureInfo.InvariantCulture));
            Assert.Equal(0, zeroBytes);
        }
    }

    private static void AssertThrowsInvalidOp(Action invoke)
    {
        TargetInvocationException tie = Assert.Throws<TargetInvocationException>(invoke);
        Assert.IsType<InvalidOperationException>(tie.InnerException);
    }

    private static (MethodInfo match, MethodInfo matchState, MethodInfo matchAsync, MethodInfo matchStateAsync,
        MethodInfo sw, MethodInfo swState, MethodInfo swAsync, MethodInfo swStateAsync)
        ResolveMatchAndSwitchMethods(Type type, int arity)
    {
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        MethodInfo match = methods.Single(m =>
            string.Equals(m.Name, "Match", StringComparison.Ordinal)
            && m.GetGenericArguments().Length == 1
            && m.GetParameters().Length == arity
            && !m.ReturnType.IsGenericType)
            .MakeGenericMethod(typeof(int));

        MethodInfo matchState = methods.Single(m =>
            string.Equals(m.Name, "Match", StringComparison.Ordinal)
            && m.GetGenericArguments().Length == 2
            && m.GetParameters().Length == arity + 1
            && !m.ReturnType.IsGenericType)
            .MakeGenericMethod(typeof(int), typeof(int));

        MethodInfo matchAsync = methods.Single(m =>
            string.Equals(m.Name, "Match", StringComparison.Ordinal)
            && m.GetGenericArguments().Length == 1
            && m.GetParameters().Length == arity
            && m.ReturnType.IsGenericType)
            .MakeGenericMethod(typeof(int));

        MethodInfo matchStateAsync = methods.Single(m =>
            string.Equals(m.Name, "Match", StringComparison.Ordinal)
            && m.GetGenericArguments().Length == 2
            && m.GetParameters().Length == arity + 1
            && m.ReturnType.IsGenericType)
            .MakeGenericMethod(typeof(int), typeof(int));

        MethodInfo sw = methods.Single(m =>
            string.Equals(m.Name, "Switch", StringComparison.Ordinal)
            && !m.IsGenericMethodDefinition
            && m.ReturnType == typeof(void));

        MethodInfo swState = methods.Single(m =>
            string.Equals(m.Name, "Switch", StringComparison.Ordinal)
            && m.IsGenericMethodDefinition
            && m.ReturnType == typeof(void))
            .MakeGenericMethod(typeof(int[]));

        MethodInfo swAsync = methods.Single(m =>
            string.Equals(m.Name, "Switch", StringComparison.Ordinal)
            && !m.IsGenericMethodDefinition
            && m.ReturnType == typeof(Task));

        MethodInfo swStateAsync = methods.Single(m =>
            string.Equals(m.Name, "Switch", StringComparison.Ordinal)
            && m.IsGenericMethodDefinition
            && m.ReturnType == typeof(Task))
            .MakeGenericMethod(typeof(int[]));

        return (match, matchState, matchAsync, matchStateAsync, sw, swState, swAsync, swStateAsync);
    }

    private static object[] BuildArray(int arity, Type[] branchTypes, string helperName)
    {
        object[] arr = new object[arity];
        for (int i = 0; i < arity; i++)
        {
            arr[i] = InvokeHelper(helperName, branchTypes[i], i);
        }

        return arr;
    }

    private static object[] BuildArrayWithArg(int arity, Type[] branchTypes, string helperName, object extraArg)
    {
        MethodInfo open = typeof(UnioAllAritiesTests).GetMethod(helperName, BindingFlags.NonPublic | BindingFlags.Static)!;
        object[] arr = new object[arity];
        for (int i = 0; i < arity; i++)
        {
            arr[i] = open.MakeGenericMethod(branchTypes[i]).Invoke(null, [i, extraArg])!;
        }

        return arr;
    }

    private static object InvokeHelper(string helperName, Type branchType, object arg)
    {
        MethodInfo open = typeof(UnioAllAritiesTests).GetMethod(helperName, BindingFlags.NonPublic | BindingFlags.Static)!;
        return open.MakeGenericMethod(branchType).Invoke(null, [arg])!;
    }

    private static Func<T, int> MakeMatchFunc<T>(int index) where T : BranchBase? =>
        v => (index * 100) + (v?.Id ?? 0);

    private static Func<int, T, int> MakeMatchStateFunc<T>(int index) where T : BranchBase? =>
        (state, v) => state + (index * 100) + (v?.Id ?? 0);

    private static Func<T, Task<int>> MakeMatchAsyncFunc<T>(int index) where T : BranchBase? =>
        v => Task.FromResult((index * 100) + (v?.Id ?? 0));

    private static Func<int, T, Task<int>> MakeMatchStateAsyncFunc<T>(int index) where T : BranchBase? =>
        (state, v) => Task.FromResult(state + (index * 100) + (v?.Id ?? 0));

    private static Action<T> MakeSwitchAction<T>(int index, int[] sink) where T : BranchBase? =>
        _ => sink[0] = index;

    private static Action<int[], T> MakeSwitchStateAction<T>(int index) where T : BranchBase? =>
        (state, _) => state[0] = index + 100;

    private static Func<T, Task> MakeSwitchAsyncFunc<T>(int index, int[] sink) where T : BranchBase? =>
        _ => { sink[0] = index + 200; return Task.CompletedTask; };

    private static Func<int[], T, Task> MakeSwitchStateAsyncFunc<T>(int index) where T : BranchBase? =>
        (state, _) => { state[0] = index + 300; return Task.CompletedTask; };

    private static Func<T> MakeFallbackFactory<T>(BranchBase fallback) where T : BranchBase =>
        () => (T)fallback;

    private static Func<int, T> MakeFallbackStateFactory<T>(int branchIndex) where T : BranchBase =>
        state => (T)CreatePlainBranch(branchIndex, state, nullText: false);

    private static Func<T, string> MakeMapFunc<T>(int branchIndex) where T : BranchBase? =>
        v => string.Create(CultureInfo.InvariantCulture, $"M{branchIndex}:{v?.Id ?? 0}");

    private static BranchBase CreatePlainBranch(int branchIndex, int id, bool nullText) =>
        (BranchBase)Activator.CreateInstance(s_allBranchTypes[branchIndex], [id, nullText])!;

    private static BranchBase CreateFormattableBranch(int branchIndex, int id) =>
        (BranchBase)Activator.CreateInstance(s_allFormattableBranchTypes[branchIndex], [id])!;

    private abstract class BranchBase(int branchIndex, int id, bool nullText) : IEquatable<BranchBase>
    {
        public int BranchIndex { get; } = branchIndex;
        public int Id { get; } = id;
        public bool NullText { get; } = nullText;

        public bool Equals(BranchBase? other) =>
            other is not null && BranchIndex == other.BranchIndex && Id == other.Id && NullText == other.NullText;

        public override bool Equals(object? obj) => Equals(obj as BranchBase);
        public override int GetHashCode() => HashCode.Combine(BranchIndex, Id, NullText);
        public override string? ToString() => NullText ? null : string.Create(CultureInfo.InvariantCulture, $"B{BranchIndex}:{Id}");
    }

    private class B0(int id, bool nullText) : BranchBase(0, id, nullText);
    private class B1(int id, bool nullText) : BranchBase(1, id, nullText);
    private class B2(int id, bool nullText) : BranchBase(2, id, nullText);
    private class B3(int id, bool nullText) : BranchBase(3, id, nullText);
    private class B4(int id, bool nullText) : BranchBase(4, id, nullText);
    private class B5(int id, bool nullText) : BranchBase(5, id, nullText);
    private class B6(int id, bool nullText) : BranchBase(6, id, nullText);
    private class B7(int id, bool nullText) : BranchBase(7, id, nullText);
    private class B8(int id, bool nullText) : BranchBase(8, id, nullText);
    private class B9(int id, bool nullText) : BranchBase(9, id, nullText);
    private class B10(int id, bool nullText) : BranchBase(10, id, nullText);
    private class B11(int id, bool nullText) : BranchBase(11, id, nullText);
    private class B12(int id, bool nullText) : BranchBase(12, id, nullText);
    private class B13(int id, bool nullText) : BranchBase(13, id, nullText);
    private class B14(int id, bool nullText) : BranchBase(14, id, nullText);
    private class B15(int id, bool nullText) : BranchBase(15, id, nullText);
    private class B16(int id, bool nullText) : BranchBase(16, id, nullText);
    private class B17(int id, bool nullText) : BranchBase(17, id, nullText);
    private class B18(int id, bool nullText) : BranchBase(18, id, nullText);
    private class B19(int id, bool nullText) : BranchBase(19, id, nullText);

    private static string FormatCore(int idx, int id, ReadOnlySpan<char> format) =>
        string.Create(CultureInfo.InvariantCulture, $"FMT{idx}:{id}:{format}");

    private static bool TryFormatCharsCore(int idx, int id, Span<char> dest, out int written, ReadOnlySpan<char> format)
    {
        string s = string.Create(CultureInfo.InvariantCulture, $"SF{idx}:{id}:{format}");
        if (s.AsSpan().TryCopyTo(dest)) { written = s.Length; return true; }
        written = 0;
        return false;
    }

    private static bool TryFormatUtf8Core(int idx, int id, Span<byte> dest, out int written, ReadOnlySpan<char> format)
    {
        string s = string.Create(CultureInfo.InvariantCulture, $"SF{idx}:{id}:{format}");
        int count = Encoding.UTF8.GetByteCount(s);
        if (count <= dest.Length) { written = Encoding.UTF8.GetBytes(s, dest); return true; }
        written = 0;
        return false;
    }

    private sealed class FB0(int id) : B0(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(0, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(0, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(0, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB1(int id) : B1(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(1, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(1, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(1, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB2(int id) : B2(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(2, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(2, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(2, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB3(int id) : B3(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(3, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(3, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(3, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB4(int id) : B4(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(4, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(4, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(4, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB5(int id) : B5(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(5, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(5, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(5, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB6(int id) : B6(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(6, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(6, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(6, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB7(int id) : B7(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(7, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(7, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(7, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB8(int id) : B8(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(8, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(8, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(8, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB9(int id) : B9(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(9, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(9, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(9, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB10(int id) : B10(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(10, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(10, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(10, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB11(int id) : B11(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(11, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(11, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(11, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB12(int id) : B12(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(12, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(12, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(12, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB13(int id) : B13(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(13, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(13, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(13, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB14(int id) : B14(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(14, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(14, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(14, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB15(int id) : B15(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(15, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(15, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(15, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB16(int id) : B16(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(16, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(16, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(16, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB17(int id) : B17(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(17, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(17, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(17, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB18(int id) : B18(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(18, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(18, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(18, Id, utf8Destination, out bytesWritten, format);
    }
    private sealed class FB19(int id) : B19(id, false), IFormattable, ISpanFormattable, IUtf8SpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => FormatCore(19, Id, format);
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatCharsCore(19, Id, destination, out charsWritten, format);
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TryFormatUtf8Core(19, Id, utf8Destination, out bytesWritten, format);
    }

    private static readonly Type[] s_allFormattableBranchTypes =
    [
        typeof(FB0), typeof(FB1), typeof(FB2), typeof(FB3), typeof(FB4),
        typeof(FB5), typeof(FB6), typeof(FB7), typeof(FB8), typeof(FB9),
        typeof(FB10), typeof(FB11), typeof(FB12), typeof(FB13), typeof(FB14),
        typeof(FB15), typeof(FB16), typeof(FB17), typeof(FB18), typeof(FB19)
    ];

    private sealed class TestBase2(Unio<B0, B1> u) : UnioBase<B0, B1>(u);
    private sealed class TestBase3(Unio<B0, B1, B2> u) : UnioBase<B0, B1, B2>(u);
    private sealed class TestBase4(Unio<B0, B1, B2, B3> u) : UnioBase<B0, B1, B2, B3>(u);
    private sealed class TestBase5(Unio<B0, B1, B2, B3, B4> u) : UnioBase<B0, B1, B2, B3, B4>(u);
    private sealed class TestBase6(Unio<B0, B1, B2, B3, B4, B5> u) : UnioBase<B0, B1, B2, B3, B4, B5>(u);
    private sealed class TestBase7(Unio<B0, B1, B2, B3, B4, B5, B6> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6>(u);
    private sealed class TestBase8(Unio<B0, B1, B2, B3, B4, B5, B6, B7> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7>(u);
    private sealed class TestBase9(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8>(u);
    private sealed class TestBase10(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9>(u);
    private sealed class TestBase11(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10>(u);
    private sealed class TestBase12(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11>(u);
    private sealed class TestBase13(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12>(u);
    private sealed class TestBase14(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13>(u);
    private sealed class TestBase15(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14>(u);
    private sealed class TestBase16(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15>(u);
    private sealed class TestBase17(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16>(u);
    private sealed class TestBase18(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17>(u);
    private sealed class TestBase19(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17, B18> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17, B18>(u);
    private sealed class TestBase20(Unio<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17, B18, B19> u) : UnioBase<B0, B1, B2, B3, B4, B5, B6, B7, B8, B9, B10, B11, B12, B13, B14, B15, B16, B17, B18, B19>(u);
}

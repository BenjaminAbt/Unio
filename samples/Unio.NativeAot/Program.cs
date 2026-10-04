// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Unio;
using Unio.Extensions;
using Unio.Types;

namespace Unio.NativeAot;

internal static class Program
{
    private static async Task Main()
    {
        Check(!RuntimeFeature.IsDynamicCodeSupported, "Run the published native executable.");

        Unio<int, string> number = 42;
        Check(number.Match(static n => n + 1, static s => s.Length) == 43, "Match");
        Check(number.Match(8, static (state, n) => state + n, static (_, s) => s.Length) == 50, "Stateful Match");
        Check(number.TryGetT0(out int value) && value == 42, "TryGet");
        Check(number == (Unio<int, string>)42 && number != (Unio<int, string>)"42", "Equality");
        Check(number.GetHashCode() == ((Unio<int, string>)42).GetHashCode(), "Hashing");
        Check(default(Unio<string, int>).AsT0 is null, "Default reference branch");
        Check(number.MapT0(static n => n * 2).AsT0 == 84, "Map");
        Check(number.ToResult().AsT0.Value == 42, "Value carrier");
        Check(new Success<int>(42) == new Success<int>(42), "Carrier equality");
        Check(new NotFound() == default(NotFound), "Marker equality");
        Check((await number.Match(static n => Task.FromResult(n + 1), static s => Task.FromResult(s.Length)).ConfigureAwait(false)) == 43, "Async Match");
        Check((await number.BindT0Async(static n => Task.FromResult(n * 2)).ConfigureAwait(false)).AsT0 == 84, "Async extension");

        int[] switched = new int[1];
        number.Switch(switched, static (state, n) => state[0] = n, static (state, s) => state[0] = s.Length);
        Check(switched[0] == 42, "Stateful Switch");
        Unio<int, string, bool> three = true;
        Check(!three.TryPickT0(out _, out Unio<string, bool> remainder) && remainder.AsT1, "TryPick");

        NumberOrError named = 42;
        Check(named.AsT0 == 42 && named == (NumberOrError)42, "Source-generated union");
        Check(string.Equals(named.ToString("X", CultureInfo.InvariantCulture), "2A", StringComparison.Ordinal), "Named formatting");
        CheckFormatting();
        ArityChecks.Run();

        Console.WriteLine("NativeAOT smoke tests passed: core, generated unions, types and extensions.");
    }

    private static void CheckFormatting()
    {
        Unio<FormatOnlyValue, string> union = new FormatOnlyValue();
        Span<char> chars = stackalloc char[16];
        Span<byte> bytes = stackalloc byte[16];
        Check(union.TryFormat(chars, out int written, "F2", CultureInfo.InvariantCulture) && chars[..written].SequenceEqual("12.50"), "UTF-16 fallback");
        Check(union.TryFormat(bytes, out written, "F2", CultureInfo.InvariantCulture) && string.Equals(Encoding.UTF8.GetString(bytes[..written]), "12.50", StringComparison.Ordinal), "UTF-8 fallback");
        Check(!union.TryFormat(chars[..2], out written, "F2", CultureInfo.InvariantCulture) && written == 0, "Small formatting buffer");
    }

    internal static void Check(bool condition, string operation)
    {
        if (!condition)
        {
            throw new InvalidOperationException(operation);
        }
    }

    private sealed class FormatOnlyValue : IFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider)
            => 12.5m.ToString(format, formatProvider);
    }
}

[GenerateUnio]
internal partial class NumberOrError : UnioBase<int, string>;

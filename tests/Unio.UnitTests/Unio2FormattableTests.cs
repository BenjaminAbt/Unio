// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.UnitTests;

/// <summary>
/// Unit tests for <see cref="IFormattable"/>, <see cref="ISpanFormattable"/>,
/// and <see cref="IUtf8SpanFormattable"/> implementations on <see cref="Unio{T0,T1}"/>.
/// </summary>
public class Unio2FormattableTests
{
    [Fact]
    public void IFormattable_ToString_WhenT0IsFormattable_UsesFormat()
    {
        Unio<int, string> union = 42;
        IFormattable formattable = union;

        string result = formattable.ToString("X", CultureInfo.InvariantCulture);

        Assert.Equal("2A", result);
    }

    [Fact]
    public void IFormattable_ToString_WhenT1IsNotFormattable_FallsBackToToString()
    {
        Unio<int, string> union = "hello";
        IFormattable formattable = union;

        string result = formattable.ToString(null, CultureInfo.InvariantCulture);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void ISpanFormattable_TryFormat_WhenT0IsInt_FormatsCorrectly()
    {
        Unio<int, string> union = 42;
        Span<char> buffer = stackalloc char[10];

        bool success = union.TryFormat(buffer, out int written, default, CultureInfo.InvariantCulture);

        Assert.True(success);
        Assert.Equal("42", buffer[..written].ToString());
    }

    [Fact]
    public void ISpanFormattable_TryFormat_WhenBufferTooSmall_ReturnsFalse()
    {
        Unio<int, string> union = "hello world";
        Span<char> buffer = stackalloc char[3];

        bool success = union.TryFormat(buffer, out int written, default, null);

        Assert.False(success);
        Assert.Equal(0, written);
    }

    [Fact]
    public void ISpanFormattable_TryFormat_WhenT0IsFormattable_UsesFormat()
    {
        Unio<int, string> union = 255;
        Span<char> buffer = stackalloc char[10];

        bool success = ((ISpanFormattable)union).TryFormat(buffer, out int written, "X", CultureInfo.InvariantCulture);

        Assert.True(success);
        Assert.Equal("FF", buffer[..written].ToString());
    }

    [Fact]
    public void IUtf8SpanFormattable_TryFormat_WhenT0IsInt_FormatsCorrectly()
    {
        Unio<int, string> union = 42;
        Span<byte> buffer = stackalloc byte[10];

        bool success = union.TryFormat(buffer, out int written, default, CultureInfo.InvariantCulture);

        Assert.True(success);
        Assert.Equal("42", System.Text.Encoding.UTF8.GetString(buffer[..written]));
    }

    [Fact]
    public void IUtf8SpanFormattable_TryFormat_WhenBufferTooSmall_ReturnsFalse()
    {
        Unio<int, string> union = "hello world";
        Span<byte> buffer = stackalloc byte[3];

        bool success = union.TryFormat(buffer, out int written, default, null);

        Assert.False(success);
        Assert.Equal(0, written);
    }
}

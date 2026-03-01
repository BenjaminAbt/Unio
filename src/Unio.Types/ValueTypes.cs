// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;

namespace Unio.Types;

// =====================================================================
// Value-Carrying Result Types
// =====================================================================

/// <summary>Represents a success result carrying a value of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the success value.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Success<T> : IEquatable<Success<T>>
{
    /// <summary>Gets the success value.</summary>
    public T Value { get; }

    /// <summary>Creates a new success result with the specified value.</summary>
    public Success(T value) => Value = value;

    /// <summary>Implicitly converts a value to a <see cref="Success{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator Success<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Success<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Success<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Success({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Success<T> left, Success<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Success<T> left, Success<T> right) => !left.Equals(right);
}

/// <summary>Represents an error result carrying a value of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the error value.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Error<T> : IEquatable<Error<T>>
{
    /// <summary>Gets the error value.</summary>
    public T Value { get; }

    /// <summary>Creates a new error result with the specified value.</summary>
    public Error(T value) => Value = value;

    /// <summary>Implicitly converts a value to an <see cref="Error{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator Error<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Error<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Error<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Error({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Error<T> left, Error<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Error<T> left, Error<T> right) => !left.Equals(right);
}

/// <summary>Represents a result carrying a value of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the result value.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Result<T> : IEquatable<Result<T>>
{
    /// <summary>Gets the result value.</summary>
    public T Value { get; }

    /// <summary>Creates a new result with the specified value.</summary>
    public Result(T value) => Value = value;

    /// <summary>Implicitly converts a value to a <see cref="Result{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Result<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Result({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Result<T> left, Result<T> right) => !left.Equals(right);
}

/// <summary>Represents a "not found" result carrying an identifier of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the identifier that was not found.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct NotFound<T> : IEquatable<NotFound<T>>
{
    /// <summary>Gets the identifier that was not found.</summary>
    public T Value { get; }

    /// <summary>Creates a new not-found result with the specified identifier.</summary>
    public NotFound(T value) => Value = value;

    /// <summary>Implicitly converts a value to a <see cref="NotFound{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator NotFound<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(NotFound<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is NotFound<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "NotFound({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(NotFound<T> left, NotFound<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(NotFound<T> left, NotFound<T> right) => !left.Equals(right);
}

/// <summary>Represents a "created" result carrying the created entity or ID of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the created value.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Created<T> : IEquatable<Created<T>>
{
    /// <summary>Gets the created value.</summary>
    public T Value { get; }

    /// <summary>Creates a new created result with the specified value.</summary>
    public Created(T value) => Value = value;

    /// <summary>Implicitly converts a value to a <see cref="Created{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator Created<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Created<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Created<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Created({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Created<T> left, Created<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Created<T> left, Created<T> right) => !left.Equals(right);
}

/// <summary>Represents an "updated" result carrying the updated entity of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the updated value.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct Updated<T> : IEquatable<Updated<T>>
{
    /// <summary>Gets the updated value.</summary>
    public T Value { get; }

    /// <summary>Creates a new updated result with the specified value.</summary>
    public Updated(T value) => Value = value;

    /// <summary>Implicitly converts a value to an <see cref="Updated{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator Updated<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Updated<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Updated<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Updated({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(Updated<T> left, Updated<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Updated<T> left, Updated<T> right) => !left.Equals(right);
}

// =====================================================================
// Validation Types
// =====================================================================

/// <summary>Represents a validation error carrying a message string.</summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ValidationError : IEquatable<ValidationError>
{
    /// <summary>Gets the validation error message.</summary>
    public string Message { get; }

    /// <summary>Creates a new validation error with the specified message.</summary>
    public ValidationError(string message) => Message = message;

    /// <summary>Implicitly converts a string to a <see cref="ValidationError"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValidationError(string message) => new(message);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(ValidationError other) => string.Equals(Message, other.Message, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ValidationError other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Message is not null ? StringComparer.Ordinal.GetHashCode(Message) : 0;

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "ValidationError({0})", Message);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(ValidationError left, ValidationError right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(ValidationError left, ValidationError right) => !left.Equals(right);
}

/// <summary>Represents a validation error carrying a value of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the validation error details.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ValidationError<T> : IEquatable<ValidationError<T>>
{
    /// <summary>Gets the validation error details.</summary>
    public T Value { get; }

    /// <summary>Creates a new validation error with the specified details.</summary>
    public ValidationError(T value) => Value = value;

    /// <summary>Implicitly converts a value to a <see cref="ValidationError{T}"/>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValidationError<T>(T value) => new(value);

    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(ValidationError<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is ValidationError<T> other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value!);

    /// <inheritdoc/>
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "ValidationError({0})", Value);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(ValidationError<T> left, ValidationError<T> right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(ValidationError<T> left, ValidationError<T> right) => !left.Equals(right);
}

// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace Unio.Types;

// =====================================================================
// Boolean / Ternary Markers
// =====================================================================

/// <summary>Represents an affirmative (yes) sentinel type for discriminated unions.</summary>
public readonly struct Yes : IEquatable<Yes>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Yes other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Yes;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Yes);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Yes left, Yes right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Yes left, Yes right) => false;
}

/// <summary>Represents a negative (no) sentinel type for discriminated unions.</summary>
public readonly struct No : IEquatable<No>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(No other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is No;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(No);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(No left, No right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(No left, No right) => false;
}

/// <summary>Represents an indeterminate (maybe) sentinel type for discriminated unions.</summary>
public readonly struct Maybe : IEquatable<Maybe>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Maybe other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Maybe;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Maybe);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Maybe left, Maybe right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Maybe left, Maybe right) => false;
}

/// <summary>Represents a boolean true sentinel type for discriminated unions.</summary>
public readonly struct True : IEquatable<True>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(True other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is True;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(True);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(True left, True right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(True left, True right) => false;
}

/// <summary>Represents a boolean false sentinel type for discriminated unions.</summary>
public readonly struct False : IEquatable<False>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(False other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is False;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(False);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(False left, False right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(False left, False right) => false;
}

/// <summary>Represents an unknown state sentinel type for discriminated unions.</summary>
public readonly struct Unknown : IEquatable<Unknown>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unknown other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Unknown;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Unknown);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Unknown left, Unknown right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Unknown left, Unknown right) => false;
}

// =====================================================================
// Collection / Quantity Markers
// =====================================================================

/// <summary>Represents an "all items" sentinel type for discriminated unions.</summary>
public readonly struct All : IEquatable<All>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(All other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is All;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(All);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(All left, All right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(All left, All right) => false;
}

/// <summary>Represents a "some items" sentinel type for discriminated unions.</summary>
public readonly struct Some : IEquatable<Some>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Some other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Some;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Some);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Some left, Some right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Some left, Some right) => false;
}

/// <summary>Represents a "no items / empty result" sentinel type for discriminated unions.</summary>
public readonly struct None : IEquatable<None>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(None other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is None;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(None);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(None left, None right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(None left, None right) => false;
}

/// <summary>Represents an empty / blank sentinel type for discriminated unions.</summary>
public readonly struct Empty : IEquatable<Empty>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Empty other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Empty;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Empty);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Empty left, Empty right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Empty left, Empty right) => false;
}

// =====================================================================
// State Markers
// =====================================================================

/// <summary>Represents a pending / in-progress sentinel type for discriminated unions.</summary>
public readonly struct Pending : IEquatable<Pending>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Pending other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Pending;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Pending);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Pending left, Pending right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Pending left, Pending right) => false;
}

/// <summary>Represents a cancelled operation sentinel type for discriminated unions.</summary>
public readonly struct Cancelled : IEquatable<Cancelled>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Cancelled other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Cancelled;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Cancelled);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Cancelled left, Cancelled right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Cancelled left, Cancelled right) => false;
}

/// <summary>Represents a timeout sentinel type for discriminated unions.</summary>
public readonly struct Timeout : IEquatable<Timeout>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Timeout other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Timeout;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Timeout);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Timeout left, Timeout right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Timeout left, Timeout right) => false;
}

/// <summary>Represents a skipped operation sentinel type for discriminated unions.</summary>
public readonly struct Skipped : IEquatable<Skipped>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Skipped other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Skipped;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Skipped);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Skipped left, Skipped right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Skipped left, Skipped right) => false;
}

/// <summary>Represents an invalid state / input sentinel type for discriminated unions.</summary>
public readonly struct Invalid : IEquatable<Invalid>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Invalid other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Invalid;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Invalid);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Invalid left, Invalid right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Invalid left, Invalid right) => false;
}

/// <summary>Represents a disabled sentinel type for discriminated unions.</summary>
public readonly struct Disabled : IEquatable<Disabled>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Disabled other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Disabled;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Disabled);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Disabled left, Disabled right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Disabled left, Disabled right) => false;
}

/// <summary>Represents an expired (token, session, resource) sentinel type for discriminated unions.</summary>
public readonly struct Expired : IEquatable<Expired>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Expired other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Expired;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Expired);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Expired left, Expired right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Expired left, Expired right) => false;
}

/// <summary>Represents a rate-limited sentinel type for discriminated unions.</summary>
public readonly struct RateLimited : IEquatable<RateLimited>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(RateLimited other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is RateLimited;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(RateLimited);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(RateLimited left, RateLimited right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(RateLimited left, RateLimited right) => false;
}

// =====================================================================
// HTTP / API Markers
// =====================================================================

/// <summary>Represents a "not found" (404) sentinel type for discriminated unions.</summary>
public readonly struct NotFound : IEquatable<NotFound>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(NotFound other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is NotFound;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(NotFound);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(NotFound left, NotFound right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(NotFound left, NotFound right) => false;
}

/// <summary>Represents a "forbidden" (403) sentinel type for discriminated unions.</summary>
public readonly struct Forbidden : IEquatable<Forbidden>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Forbidden other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Forbidden;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Forbidden);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Forbidden left, Forbidden right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Forbidden left, Forbidden right) => false;
}

/// <summary>Represents an "unauthorized" (401) sentinel type for discriminated unions.</summary>
public readonly struct Unauthorized : IEquatable<Unauthorized>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unauthorized other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Unauthorized;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Unauthorized);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Unauthorized left, Unauthorized right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Unauthorized left, Unauthorized right) => false;
}

/// <summary>Represents a "conflict" (409) sentinel type for discriminated unions.</summary>
public readonly struct Conflict : IEquatable<Conflict>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Conflict other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Conflict;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Conflict);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Conflict left, Conflict right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Conflict left, Conflict right) => false;
}

/// <summary>Represents a "bad request" (400) sentinel type for discriminated unions.</summary>
public readonly struct BadRequest : IEquatable<BadRequest>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(BadRequest other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BadRequest;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(BadRequest);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(BadRequest left, BadRequest right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(BadRequest left, BadRequest right) => false;
}

/// <summary>Represents an "accepted" (202) sentinel type for discriminated unions.</summary>
public readonly struct Accepted : IEquatable<Accepted>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Accepted other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Accepted;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Accepted);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Accepted left, Accepted right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Accepted left, Accepted right) => false;
}

/// <summary>Represents a "no content" (204) sentinel type for discriminated unions.</summary>
public readonly struct NoContent : IEquatable<NoContent>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(NoContent other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is NoContent;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(NoContent);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(NoContent left, NoContent right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(NoContent left, NoContent right) => false;
}

// =====================================================================
// CRUD Markers
// =====================================================================

/// <summary>Represents a "created" sentinel type for discriminated unions.</summary>
public readonly struct Created : IEquatable<Created>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Created other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Created;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Created);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Created left, Created right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Created left, Created right) => false;
}

/// <summary>Represents an "updated" sentinel type for discriminated unions.</summary>
public readonly struct Updated : IEquatable<Updated>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Updated other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Updated;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Updated);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Updated left, Updated right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Updated left, Updated right) => false;
}

/// <summary>Represents a "deleted" sentinel type for discriminated unions.</summary>
public readonly struct Deleted : IEquatable<Deleted>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Deleted other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Deleted;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Deleted);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Deleted left, Deleted right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Deleted left, Deleted right) => false;
}

/// <summary>Represents an "unchanged" sentinel type for discriminated unions.</summary>
public readonly struct Unchanged : IEquatable<Unchanged>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unchanged other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Unchanged;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Unchanged);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Unchanged left, Unchanged right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Unchanged left, Unchanged right) => false;
}

// =====================================================================
// Result Markers
// =====================================================================

/// <summary>Represents a success sentinel type for discriminated unions.</summary>
public readonly struct Success : IEquatable<Success>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Success other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Success;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Success);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Success left, Success right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Success left, Success right) => false;
}

/// <summary>Represents an error sentinel type for discriminated unions.</summary>
public readonly struct Error : IEquatable<Error>
{
    /// <inheritdoc/>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public bool Equals(Error other) => true;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Error;

    /// <inheritdoc/>
    public override int GetHashCode() => 0;

    /// <inheritdoc/>
    public override string ToString() => nameof(Error);

    /// <summary>Equality operator. All instances are equal.</summary>
    public static bool operator ==(Error left, Error right) => true;

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(Error left, Error right) => false;
}

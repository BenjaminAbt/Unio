// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Unio.Types;

namespace Unio.Types.UnitTests;

/// <summary>
/// Unit tests for all sentinel marker structs in <see cref="Unio.Types"/> (Yes, No, Maybe, True, False,
/// Unknown, All, Some, None, Empty, Pending, Cancelled, Timeout, Skipped, Invalid, Disabled, Expired,
/// RateLimited, NotFound, Forbidden, Unauthorized, Conflict, BadRequest, Accepted, NoContent,
/// Created, Updated, Deleted, Unchanged, Success, Error) verifying equality, hashing and <c>ToString</c>.
/// </summary>
public class MarkerTests
{
    // ====== Yes ======

    [Fact]
    public void Yes_Equals_SameType_ReturnsTrue()
    {
        Yes a = new();
        Yes b = new();
        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void Yes_Equals_Object_ReturnsTrue()
    {
        Yes a = new();
        Assert.True(a.Equals((object)new Yes()));
        Assert.False(a.Equals((object?)null));
        Assert.False(a.Equals("not a Yes"));
    }

    [Fact]
    public void Yes_GetHashCode_IsConsistent()
    {
        Assert.Equal(new Yes().GetHashCode(), new Yes().GetHashCode());
    }

    [Fact]
    public void Yes_ToString_ReturnsTypeName()
    {
        Assert.Equal("Yes", new Yes().ToString());
    }

    // ====== No ======

    [Fact]
    public void No_Equals_ReturnsTrue()
    {
        Assert.True(new No() == new No());
        Assert.False(new No() != new No());
    }

    [Fact]
    public void No_ToString_ReturnsTypeName()
    {
        Assert.Equal("No", new No().ToString());
    }

    // ====== Maybe ======

    [Fact]
    public void Maybe_ToString_ReturnsTypeName()
    {
        Assert.Equal("Maybe", new Maybe().ToString());
    }

    // ====== True / False ======

    [Fact]
    public void True_ToString_ReturnsTypeName()
    {
        Assert.Equal("True", new True().ToString());
    }

    [Fact]
    public void False_ToString_ReturnsTypeName()
    {
        Assert.Equal("False", new False().ToString());
    }

    // ====== Unknown ======

    [Fact]
    public void Unknown_ToString_ReturnsTypeName()
    {
        Assert.Equal("Unknown", new Unknown().ToString());
    }

    // ====== Collection Markers ======

    [Fact]
    public void All_ToString_ReturnsTypeName()
    {
        Assert.Equal("All", new All().ToString());
    }

    [Fact]
    public void Some_ToString_ReturnsTypeName()
    {
        Assert.Equal("Some", new Some().ToString());
    }

    [Fact]
    public void None_Equals_ReturnsTrue()
    {
        Assert.True(new None() == new None());
    }

    [Fact]
    public void None_ToString_ReturnsTypeName()
    {
        Assert.Equal("None", new None().ToString());
    }

    [Fact]
    public void Empty_ToString_ReturnsTypeName()
    {
        Assert.Equal("Empty", new Empty().ToString());
    }

    // ====== State Markers ======

    [Fact]
    public void Pending_ToString_ReturnsTypeName()
    {
        Assert.Equal("Pending", new Pending().ToString());
    }

    [Fact]
    public void Cancelled_ToString_ReturnsTypeName()
    {
        Assert.Equal("Cancelled", new Cancelled().ToString());
    }

    [Fact]
    public void Timeout_ToString_ReturnsTypeName()
    {
        Assert.Equal("Timeout", new Timeout().ToString());
    }

    [Fact]
    public void Skipped_ToString_ReturnsTypeName()
    {
        Assert.Equal("Skipped", new Skipped().ToString());
    }

    [Fact]
    public void Invalid_ToString_ReturnsTypeName()
    {
        Assert.Equal("Invalid", new Invalid().ToString());
    }

    [Fact]
    public void Disabled_ToString_ReturnsTypeName()
    {
        Assert.Equal("Disabled", new Disabled().ToString());
    }

    [Fact]
    public void Expired_ToString_ReturnsTypeName()
    {
        Assert.Equal("Expired", new Expired().ToString());
    }

    [Fact]
    public void RateLimited_ToString_ReturnsTypeName()
    {
        Assert.Equal("RateLimited", new RateLimited().ToString());
    }

    // ====== HTTP Markers ======

    [Fact]
    public void NotFound_Equals_ReturnsTrue()
    {
        Assert.True(new NotFound() == new NotFound());
    }

    [Fact]
    public void NotFound_ToString_ReturnsTypeName()
    {
        Assert.Equal("NotFound", new NotFound().ToString());
    }

    [Fact]
    public void Forbidden_ToString_ReturnsTypeName()
    {
        Assert.Equal("Forbidden", new Forbidden().ToString());
    }

    [Fact]
    public void Unauthorized_ToString_ReturnsTypeName()
    {
        Assert.Equal("Unauthorized", new Unauthorized().ToString());
    }

    [Fact]
    public void Conflict_ToString_ReturnsTypeName()
    {
        Assert.Equal("Conflict", new Conflict().ToString());
    }

    [Fact]
    public void BadRequest_ToString_ReturnsTypeName()
    {
        Assert.Equal("BadRequest", new BadRequest().ToString());
    }

    [Fact]
    public void Accepted_ToString_ReturnsTypeName()
    {
        Assert.Equal("Accepted", new Accepted().ToString());
    }

    [Fact]
    public void NoContent_ToString_ReturnsTypeName()
    {
        Assert.Equal("NoContent", new NoContent().ToString());
    }

    // ====== CRUD Markers ======

    [Fact]
    public void Created_ToString_ReturnsTypeName()
    {
        Assert.Equal("Created", new Created().ToString());
    }

    [Fact]
    public void Updated_ToString_ReturnsTypeName()
    {
        Assert.Equal("Updated", new Updated().ToString());
    }

    [Fact]
    public void Deleted_ToString_ReturnsTypeName()
    {
        Assert.Equal("Deleted", new Deleted().ToString());
    }

    [Fact]
    public void Unchanged_ToString_ReturnsTypeName()
    {
        Assert.Equal("Unchanged", new Unchanged().ToString());
    }

    // ====== Result Markers ======

    [Fact]
    public void Success_Equals_ReturnsTrue()
    {
        Assert.True(new Success() == new Success());
    }

    [Fact]
    public void Success_ToString_ReturnsTypeName()
    {
        Assert.Equal("Success", new Success().ToString());
    }

    [Fact]
    public void Error_Equals_ReturnsTrue()
    {
        Assert.True(new Error() == new Error());
    }

    [Fact]
    public void Error_ToString_ReturnsTypeName()
    {
        Assert.Equal("Error", new Error().ToString());
    }

    // ====== Integration with Unio<> ======

    [Fact]
    public void Markers_WorkWithUnio_Success()
    {
        Unio<string, NotFound> result = "hello";
        Assert.True(result.IsT0);
        Assert.Equal("hello", result.AsT0);
    }

    [Fact]
    public void Markers_WorkWithUnio_NotFound()
    {
        Unio<string, NotFound> result = new NotFound();
        Assert.True(result.IsT1);
    }

    [Fact]
    public void Markers_WorkWithUnio_3Way ()
    {
        Unio<int, NotFound, Forbidden> result = new Forbidden();
        Assert.True(result.IsT2);

        string message = result.Match(
            v => string.Create(CultureInfo.InvariantCulture, $"Value: {v}"),
            _ => "Not found",
            _ => "Forbidden");
        Assert.Equal("Forbidden", message);
    }
}

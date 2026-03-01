// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Http;
using Unio.AspNetCore.MinimalApi;
using Unio.Types;

namespace Unio.AspNetCore.UnitTests.MinimalApi;

/// <summary>
/// Unit tests for <see cref="UnioResultExtensions"/> verifying that known marker types
/// are mapped to the correct HTTP status codes.
/// </summary>
public class UnioResultExtensionsTests
{
    [Fact]
    public void ToHttpResult_WithNotFoundMarker_Returns404()
    {
        Unio<string, NotFound> union = new NotFound();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithSuccessValue_Returns200()
    {
        Unio<string, NotFound> union = "hello";

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithBadRequestMarker_Returns400()
    {
        Unio<string, BadRequest> union = new BadRequest();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithConflictMarker_Returns409()
    {
        Unio<string, Conflict> union = new Conflict();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithNoContentMarker_Returns204()
    {
        Unio<string, NoContent> union = new NoContent();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithAcceptedMarker_Returns202()
    {
        Unio<string, Accepted> union = new Accepted();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status202Accepted, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithForbiddenMarker_Returns403()
    {
        Unio<string, Forbidden> union = new Forbidden();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithUnauthorizedMarker_Returns401()
    {
        Unio<string, Unauthorized> union = new Unauthorized();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithCreatedMarker_Returns201()
    {
        Unio<string, Created> union = new Created();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status201Created, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_3Arity_WithSuccessValue_Returns200()
    {
        Unio<string, NotFound, Forbidden> union = "data";

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_3Arity_WithNotFound_Returns404()
    {
        Unio<string, NotFound, Forbidden> union = new NotFound();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, statusResult.StatusCode);
    }

    [Fact]
    public void ToHttpResult_3Arity_WithForbidden_Returns403()
    {
        Unio<string, NotFound, Forbidden> union = new Forbidden();

        IResult result = union.ToHttpResult();

        IStatusCodeHttpResult statusResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status403Forbidden, statusResult.StatusCode);
    }
}

// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
    public async Task ToHttpResult_WithJsonContext_WritesSourceGeneratedJson()
    {
        Unio<ResponseBody, NotFound> union = new ResponseBody(42);
        using ServiceProvider services = new ServiceCollection().AddLogging().BuildServiceProvider();
        DefaultHttpContext context = new() { RequestServices = services };
        using MemoryStream stream = new();
        context.Response.Body = stream;

        await union.ToHttpResult(ResponseJsonContext.Default).ExecuteAsync(context);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Equal("""{"value":42}""", System.Text.Encoding.UTF8.GetString(stream.ToArray()));
    }

    [Fact]
    public void ToHttpResult_WithJsonContext_DoesNotNeedMarkerMetadata()
    {
        Unio<ResponseBody, NotFound> union = new NotFound();

        IResult result = union.ToHttpResult(ResponseJsonContext.Default);

        Assert.Equal(404, Assert.IsAssignableFrom<IStatusCodeHttpResult>(result).StatusCode);
    }

    [Fact]
    public void ToHttpResult_WithJsonContext_RejectsMissingMetadata()
    {
        Unio<DateTime, NotFound> union = DateTime.UnixEpoch;

        Assert.Throws<NotSupportedException>(() => union.ToHttpResult(ResponseJsonContext.Default));
    }

    [Fact]
    public void ToHttpResult_WithJsonContext_RejectsNullContext()
    {
        Unio<ResponseBody, NotFound> union = new NotFound();

        Assert.Throws<ArgumentNullException>(() => union.ToHttpResult(null!));
    }

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

    [Fact]
    public void ToHttpResult_WithNullValueAndJsonContext_ReturnsEmpty200()
    {
        Unio<string?, NotFound> union = (string?)null;

        IResult result = union.ToHttpResult(ResponseJsonContext.Default);

        Assert.Equal(StatusCodes.Status200OK, Assert.IsAssignableFrom<IStatusCodeHttpResult>(result).StatusCode);
    }

    [Fact]
    public void ToHttpResult_Arities3Through9_BothOverloads_MapCorrectly()
    {
        Unio<ResponseBody, NotFound, Forbidden> u3 = new ResponseBody(3);
        Unio<ResponseBody, NotFound, Forbidden, BadRequest> u4 = new BadRequest();
        Unio<ResponseBody, NotFound, Forbidden, BadRequest, Conflict> u5 = new Conflict();
        Unio<ResponseBody, NotFound, Forbidden, BadRequest, Conflict, Unauthorized> u6 = new Unauthorized();
        Unio<ResponseBody, NotFound, Forbidden, BadRequest, Conflict, Unauthorized, Created> u7 = new Created();
        Unio<ResponseBody, NotFound, Forbidden, BadRequest, Conflict, Unauthorized, Created, Accepted> u8 = new Accepted();
        Unio<ResponseBody, NotFound, Forbidden, BadRequest, Conflict, Unauthorized, Created, Accepted, NoContent> u9 = new NoContent();

        Assert.Equal(StatusCodes.Status200OK, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u3.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u4.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u4.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status409Conflict, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u5.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status409Conflict, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u5.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status401Unauthorized, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u6.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status401Unauthorized, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u6.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status201Created, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u7.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status201Created, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u7.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status202Accepted, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u8.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status202Accepted, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u8.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
        Assert.Equal(StatusCodes.Status204NoContent, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u9.ToHttpResult()).StatusCode);
        Assert.Equal(StatusCodes.Status204NoContent, Assert.IsAssignableFrom<IStatusCodeHttpResult>(u9.ToHttpResult(ResponseJsonContext.Default)).StatusCode);
    }
}

internal sealed record ResponseBody(int Value);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(ResponseBody))]
internal partial class ResponseJsonContext : JsonSerializerContext;

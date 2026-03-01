// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Http;
using Unio.Types;

namespace Unio.AspNetCore.MinimalApi;

/// <summary>
/// Extension methods for converting <c>Unio&lt;...&gt;</c> discriminated unions
/// to <see cref="IResult"/> for ASP.NET Core Minimal APIs.
/// Known marker types from <c>Unio.Types</c> are automatically mapped to HTTP status codes;
/// all other types are wrapped in <see cref="Results.Ok{TValue}(TValue)"/>.
/// </summary>
public static class UnioResultExtensions
{
    /// <summary>Converts a 2-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1>(this Unio<T0, T1> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 3-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2>(this Unio<T0, T1, T2> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 4-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3>(this Unio<T0, T1, T2, T3> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 5-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3, T4>(this Unio<T0, T1, T2, T3, T4> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 6-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3, T4, T5>(this Unio<T0, T1, T2, T3, T4, T5> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 7-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3, T4, T5, T6>(this Unio<T0, T1, T2, T3, T4, T5, T6> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts an 8-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3, T4, T5, T6, T7>(this Unio<T0, T1, T2, T3, T4, T5, T6, T7> union)
        => MapValueToResult(union.Value);

    /// <summary>Converts a 9-type union to an <see cref="IResult"/> using convention-based HTTP status mapping.</summary>
    public static IResult ToHttpResult<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this Unio<T0, T1, T2, T3, T4, T5, T6, T7, T8> union)
        => MapValueToResult(union.Value);

    /// <summary>
    /// Maps a union value to an <see cref="IResult"/> based on its runtime type.
    /// Sentinel marker types from <c>Unio.Types</c> are mapped to their conventional HTTP status codes.
    /// All other types produce <c>200 OK</c> with the value as the response body.
    /// </summary>
    internal static IResult MapValueToResult(object value) => value switch
    {
        // 4xx Client Errors
        BadRequest      => Results.BadRequest(),
        Unauthorized    => Results.StatusCode(StatusCodes.Status401Unauthorized),
        Forbidden       => Results.StatusCode(StatusCodes.Status403Forbidden),
        NotFound        => Results.NotFound(),
        Conflict        => Results.Conflict(),

        // 2xx Success (non-OK)
        Created         => Results.StatusCode(StatusCodes.Status201Created),
        Accepted        => Results.Accepted(),
        NoContent       => Results.NoContent(),

        // Default: wrap in 200 OK
        _               => Results.Ok(value)
    };
}

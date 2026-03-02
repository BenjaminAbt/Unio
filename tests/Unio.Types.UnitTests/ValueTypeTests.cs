// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Unio.Types;

namespace Unio.Types.UnitTests;

/// <summary>
/// Unit tests for value-carrying result types in <see cref="Unio.Types"/>:
/// <see cref="Success{T}"/>, <see cref="Error{T}"/>, <see cref="Result{T}"/>,
/// <see cref="NotFound{T}"/>, <see cref="Created{T}"/>, <see cref="Updated{T}"/>,
/// <see cref="ValidationError"/> and <see cref="ValidationError{T}"/>.
/// Also verifies integration with <c>Unio&lt;...&gt;</c>.
/// </summary>
public class ValueTypeTests
{
    // ====== Success<T> ======

    [Fact]
    public void SuccessT_StoresValue()
    {
        Success<int> s = new Success<int>(42);
        Assert.Equal(42, s.Value);
    }

    [Fact]
    public void SuccessT_ImplicitConversion()
    {
        Success<string> s = "hello";
        Assert.Equal("hello", s.Value);
    }

    [Fact]
    public void SuccessT_Equals_SameValue()
    {
        Success<int> a = 42;
        Success<int> b = 42;
        Assert.True(a.Equals(b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void SuccessT_NotEquals_DifferentValue()
    {
        Success<int> a = 1;
        Success<int> b = 2;
        Assert.False(a.Equals(b));
        Assert.True(a != b);
    }

    [Fact]
    public void SuccessT_GetHashCode_ConsistentWithEquals()
    {
        Success<int> a = 42;
        Success<int> b = 42;
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void SuccessT_ToString_IncludesValue()
    {
        Success<int> s = 42;
        Assert.Equal("Success(42)", s.ToString());
    }

    [Fact]
    public void SuccessT_Equals_Object()
    {
        Success<int> s = 42;
        Assert.True(s.Equals((object)new Success<int>(42)));
        Assert.False(s.Equals((object?)null));
        Assert.False(s.Equals("not Success"));
    }

    // ====== Error<T> ======

    [Fact]
    public void ErrorT_StoresValue()
    {
        Error<string> e = new Error<string>("fail");
        Assert.Equal("fail", e.Value);
    }

    [Fact]
    public void ErrorT_ImplicitConversion()
    {
        Error<string> e = "oops";
        Assert.Equal("oops", e.Value);
    }

    [Fact]
    public void ErrorT_Equals_SameValue()
    {
        Error<string> a = "fail";
        Error<string> b = "fail";
        Assert.True(a == b);
    }

    [Fact]
    public void ErrorT_NotEquals_DifferentValue()
    {
        Error<string> a = "fail";
        Error<string> b = "other";
        Assert.True(a != b);
    }

    [Fact]
    public void ErrorT_ToString_IncludesValue()
    {
        Error<string> e = "fail";
        Assert.Equal("Error(fail)", e.ToString());
    }

    // ====== Result<T> ======

    [Fact]
    public void ResultT_StoresValue()
    {
        Result<int> r = 99;
        Assert.Equal(99, r.Value);
    }

    [Fact]
    public void ResultT_Equals_SameValue()
    {
        Result<int> a = 99;
        Result<int> b = 99;
        Assert.True(a == b);
    }

    [Fact]
    public void ResultT_ToString_IncludesValue()
    {
        Result<int> r = 99;
        Assert.Equal("Result(99)", r.ToString());
    }

    // ====== NotFound<T> ======

    [Fact]
    public void NotFoundT_StoresValue()
    {
        NotFound<int> nf = new NotFound<int>(42);
        Assert.Equal(42, nf.Value);
    }

    [Fact]
    public void NotFoundT_ImplicitConversion()
    {
        NotFound<int> nf = 42;
        Assert.Equal(42, nf.Value);
    }

    [Fact]
    public void NotFoundT_Equals()
    {
        NotFound<int> a = 42;
        NotFound<int> b = 42;
        Assert.True(a == b);
    }

    [Fact]
    public void NotFoundT_ToString_IncludesValue()
    {
        NotFound<int> nf = 42;
        Assert.Equal("NotFound(42)", nf.ToString());
    }

    // ====== Created<T> ======

    [Fact]
    public void CreatedT_StoresValue()
    {
        Created<int> c = 1;
        Assert.Equal(1, c.Value);
    }

    [Fact]
    public void CreatedT_ToString_IncludesValue()
    {
        Created<int> c = 1;
        Assert.Equal("Created(1)", c.ToString());
    }

    // ====== Updated<T> ======

    [Fact]
    public void UpdatedT_StoresValue()
    {
        Updated<string> u = "new";
        Assert.Equal("new", u.Value);
    }

    [Fact]
    public void UpdatedT_ToString_IncludesValue()
    {
        Updated<string> u = "new";
        Assert.Equal("Updated(new)", u.ToString());
    }

    // ====== ValidationError ======

    [Fact]
    public void ValidationError_StoresMessage()
    {
        ValidationError ve = new ValidationError("Name is required");
        Assert.Equal("Name is required", ve.Message);
    }

    [Fact]
    public void ValidationError_ImplicitFromString()
    {
        ValidationError ve = "invalid email";
        Assert.Equal("invalid email", ve.Message);
    }

    [Fact]
    public void ValidationError_Equals_SameMessage()
    {
        ValidationError a = "msg";
        ValidationError b = "msg";
        Assert.True(a == b);
    }

    [Fact]
    public void ValidationError_NotEquals_DifferentMessage()
    {
        ValidationError a = "msg1";
        ValidationError b = "msg2";
        Assert.True(a != b);
    }

    [Fact]
    public void ValidationError_ToString_IncludesMessage()
    {
        ValidationError ve = "bad input";
        Assert.Equal("ValidationError(bad input)", ve.ToString());
    }

    [Fact]
    public void ValidationError_GetHashCode_ConsistentWithEquals()
    {
        ValidationError a = "msg";
        ValidationError b = "msg";
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    // ====== ValidationError<T> ======

    [Fact]
    public void ValidationErrorT_StoresValue()
    {
        ValidationError<string[]> ve = new ValidationError<string[]>(["err1", "err2"]);
        Assert.Equal(2, ve.Value.Length);
    }

    [Fact]
    public void ValidationErrorT_ToString_IncludesValue()
    {
        ValidationError<int> ve = 404;
        Assert.Equal("ValidationError(404)", ve.ToString());
    }

    // ====== Integration with Unio<> ======

    [Fact]
    public void ValueTypes_WorkWithUnio_Success()
    {
        Unio<Success<int>, Error<string>> result = new Success<int>(42);
        Assert.True(result.IsT0);
        Assert.Equal(42, result.AsT0.Value);
    }

    [Fact]
    public void ValueTypes_WorkWithUnio_Error()
    {
        Unio<Success<int>, Error<string>> result = new Error<string>("fail");
        Assert.True(result.IsT1);
        Assert.Equal("fail", result.AsT1.Value);
    }

    [Fact]
    public void ValueTypes_WorkWithUnio_ValidationError()
    {
        Unio<int, ValidationError> result = new ValidationError("Name required");
        Assert.True(result.IsT1);
        Assert.Equal("Name required", result.AsT1.Message);
    }

    [Fact]
    public void ValueTypes_WorkWithUnio_Match()
    {
        Unio<Created<int>, NotFound, ValidationError> result = new Created<int>(7);

        string message = result.Match(
            c => string.Create(CultureInfo.InvariantCulture, $"Created: {c.Value}"),
            _ => "Not found",
            e => $"Error: {e.Message}");

        Assert.Equal("Created: 7", message);
    }
}

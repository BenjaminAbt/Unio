# Unio.AspNetCore

ASP.NET Core integration for Unio discriminated unions.

Targets .NET 9, .NET 10 and .NET 11 RC1. Supports unions with 2–9 branches.

## Native AOT

The package enables `IsAotCompatible`. For response bodies, pass a source-generated JSON context:

```csharp
using System.Text.Json.Serialization;
using Unio;
using Unio.AspNetCore.MinimalApi;
using Unio.Types;

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();
app.MapGet("/users/{id:int}", (int id) =>
{
    Unio<User, NotFound> result = id == 1 ? new User(1, "Ada") : new NotFound();
    return result.ToHttpResult(ApiJsonContext.Default);
});
app.Run();

public sealed record User(int Id, string Name);

[JsonSerializable(typeof(User))]
internal partial class ApiJsonContext : JsonSerializerContext;
```

Enable `<PublishAot>true</PublishAot>` in the application project and publish for a concrete runtime identifier. The context must contain every possible response body type, including concrete derived types. Marker responses and null values do not require JSON metadata; null produces an empty 200 response. Missing metadata throws `NotSupportedException` rather than falling back to reflection.

Alternatively, register the context through `ConfigureHttpJsonOptions` and use the existing parameterless `ToHttpResult()` overload. AOT applications must provide JSON metadata through one of these paths.

`VerifyReferenceAotCompatibility` is disabled only for this extension because `Microsoft.AspNetCore.App` references framework assemblies that lack AOT metadata. AOT/trim analyzers remain enabled. The [runnable example](https://github.com/BenjaminAbt/Unio/tree/main/samples/Unio.AspNetCore.NativeAot) validates native JSON execution in CI.

## Minimal API Integration

Automatically maps `Unio<...>` types containing known marker types to HTTP status codes:

```csharp
using Unio.AspNetCore.MinimalApi;

app.MapGet("/users/{id}", (int id, IUserService svc) =>
{
    Unio<User, NotFound, Forbidden> result = svc.GetUser(id);
    return result.ToHttpResult(); // User→200, NotFound→404, Forbidden→403
});
```

### Convention Mapping

| Marker Type    | HTTP Status |
|----------------|-------------|
| `BadRequest`   | 400         |
| `Unauthorized` | 401         |
| `Forbidden`    | 403         |
| `NotFound`     | 404         |
| `Conflict`     | 409         |
| `Created`      | 201         |
| `Accepted`     | 202         |
| `NoContent`    | 204         |
| *(other)*      | 200 OK      |

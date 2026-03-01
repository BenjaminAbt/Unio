# Unio.AspNetCore

ASP.NET Core integration for Unio discriminated unions.

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

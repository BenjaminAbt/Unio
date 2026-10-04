// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Unio;
using Unio.AspNetCore.MinimalApi;
using Unio.Types;

namespace Unio.AspNetCore.NativeAot;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        if (args.Contains("--smoke"))
        {
            if (RuntimeFeature.IsDynamicCodeSupported)
            {
                throw new InvalidOperationException("Run the published native executable.");
            }

            await RunSmokeTestAsync().ConfigureAwait(false);
            return;
        }

        WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonContext.Default));
        WebApplication app = builder.Build();
        app.MapGet("/users/{id:int}", (int id) => FindUser(id).ToHttpResult(ApiJsonContext.Default));
        await app.RunAsync().ConfigureAwait(false);
    }

    private static Unio<User, NotFound> FindUser(int id)
        => id == 1 ? new User(1, "Ada") : new NotFound();

    private static async Task RunSmokeTestAsync()
    {
        ServiceCollection services = new();
        services.AddLogging();
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, ApiJsonContext.Default));
        using ServiceProvider provider = services.BuildServiceProvider();

        await CheckResponseAsync(FindUser(1).ToHttpResult(ApiJsonContext.Default), provider, 200, """{"id":1,"name":"Ada"}""").ConfigureAwait(false);
        await CheckResponseAsync(FindUser(1).ToHttpResult(), provider, 200, """{"id":1,"name":"Ada"}""").ConfigureAwait(false);
        await CheckResponseAsync(FindUser(0).ToHttpResult(ApiJsonContext.Default), provider, 404, "").ConfigureAwait(false);
        Unio<User?, NotFound> empty = (User?)null;
        await CheckResponseAsync(empty.ToHttpResult(ApiJsonContext.Default), provider, 200, "").ConfigureAwait(false);
        Console.WriteLine("NativeAOT HTTP smoke tests passed: explicit metadata, configured metadata, marker and null.");
    }

    private static async Task CheckResponseAsync(IResult result, IServiceProvider services, int status, string body)
    {
        DefaultHttpContext context = new() { RequestServices = services };
        using MemoryStream stream = new();
        context.Response.Body = stream;
        await result.ExecuteAsync(context).ConfigureAwait(false);
        stream.Position = 0;
        using StreamReader reader = new(stream);
        string actualBody = await reader.ReadToEndAsync(context.RequestAborted).ConfigureAwait(false);
        if (context.Response.StatusCode != status || !string.Equals(actualBody, body, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Unexpected HTTP response: {context.Response.StatusCode} {actualBody}");
        }
    }
}

internal sealed record User(int Id, string Name);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(User))]
internal partial class ApiJsonContext : JsonSerializerContext;

using Microsoft.AspNetCore.Mvc;
using Seiyo.Services;

namespace Seiyo.Endpoints;

public static class GetResourceByTextEndpoint
{
    private const string Route = "/v1/resources";

    public static void MapRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(Route, Handle)
            .WithOpenApi();
    }

    private static async Task<IResult> Handle([FromQuery] string q, [FromServices] IResourceSearcher searcher,
        CancellationToken cancellationToken = default)
    {
        var result = await searcher.FromTextAsync(q, cancellationToken);
        return Results.Ok(result);
    }
}
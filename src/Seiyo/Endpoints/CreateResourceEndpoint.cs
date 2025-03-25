using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Seiyo.Messages;
using Seiyo.Models;

namespace Seiyo.Endpoints;

public class CreateResourceEndpoint
{
    private const string Route = "/v1/resources";

    public static void MapRoute(IEndpointRouteBuilder app)
    {
        app.MapPost(Route, Handle)
            .WithOpenApi();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateResourceRequest request,
        [FromServices] IMongoDatabase database,
        [FromServices] IBus bus,
        CancellationToken cancellationToken = default)
    {
        var resource = new Resource
        {
            Id = ObjectId.GenerateNewId(),
            DisplayName = request.DisplayName,
            Description = request.Description,
            Type = request.Type,
            BusinessObject = new BusinessObject
            {
                Name = request.BusinessObject.Name,
                DisplayName = request.BusinessObject.DisplayName,
                Description = request.BusinessObject.Description,
                Areas = request.BusinessObject.Areas,
                Properties = GetProperties(request.BusinessObject.Properties),
                Parameters = request.BusinessObject.Parameters.Select(x =>
                    new BusinessObjectParameter
                    {
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        Description = x.Description,
                        Type = x.Type
                    }).ToList()
            }
        };

        var message = new ResourceCreated(resource);
        await bus.Publish(message, cancellationToken);

        return Results.Ok();
    }

    private static List<BusinessObjectProperty> GetProperties(
        IEnumerable<CreateResourceRequestBusinessObjectProperty> businessObjectProperties)
    {
        return businessObjectProperties
            .Select(item => new BusinessObjectProperty
            {
                Name = item.Name,
                DisplayName = item.DisplayName,
                Description = item.Description,
                Type = item.Type,
                Properties = GetProperties(item.Properties)
            })
            .ToList();
    }
}

public record CreateResourceRequest
{
    public string DisplayName { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Type { get; init; } = null!;
    public CreateResourceRequestBusinessObject BusinessObject { get; set; } = null!;
}

public record CreateResourceRequestBusinessObject
{
    public string DisplayName { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Name { get; init; } = null!;
    public IEnumerable<string> Areas { get; init; } = [];
    public IEnumerable<CreateResourceRequestBusinessObjectParameter> Parameters { get; init; } = [];
    public IEnumerable<CreateResourceRequestBusinessObjectProperty> Properties { get; init; } = [];
}

public class CreateResourceRequestBusinessObjectParameter
{
    public string Name { get; init; } = null!;

    public string DisplayName { get; init; } = null!;

    public string Type { get; init; } = null!;

    public string Description { get; init; } = string.Empty;
}

public class CreateResourceRequestBusinessObjectProperty
{
    public string Name { get; init; } = null!;

    public string DisplayName { get; init; } = null!;

    public string Description { get; init; } = string.Empty;

    public string Type { get; init; } = null!;

    public IEnumerable<CreateResourceRequestBusinessObjectProperty> Properties { get; init; } = [];
}
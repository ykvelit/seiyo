using System.Text.Json;
using MassTransit;
using MongoDB.Driver;
using Seiyo.Messages;
using Seiyo.Models;
using Seiyo.Services;

namespace Seiyo.Consumers;

public class GenerateEmbeddingWhenResourceCreated(IMongoDatabase database, IEmbeddingGenerator generator)
    : IConsumer<ResourceCreated>
{
    public async Task Consume(ConsumeContext<ResourceCreated> context)
    {
        var resource = context.Message.Resource;
        var json = JsonSerializer.Serialize(resource);
        resource.Embedding = await generator.FromTextAsync(json, context.CancellationToken);

        var resources = database.GetCollection<Resource>("resources");
        await resources.InsertOneAsync(resource, null, context.CancellationToken);
    }
}
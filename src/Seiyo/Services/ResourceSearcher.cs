using MongoDB.Driver;
using Seiyo.Models;

namespace Seiyo.Services;

public class ResourceSearcher(IEmbeddingGenerator generator, IMongoDatabase database) : IResourceSearcher
{
    public async Task<IEnumerable<Resource>> FromTextAsync(string text, CancellationToken cancellationToken = default)
    {
        var embedding = await generator.FromTextAsync(text, cancellationToken);

        var vectorOptions = new VectorSearchOptions<Resource>
        {
            IndexName = "vector_index",
            NumberOfCandidates = 150
        };

        var resources = database.GetCollection<Resource>("resources");

        var projection = Builders<Resource>.Projection
            .Include(x => x.Id)
            .Include(x => x.DisplayName)
            .Include(x => x.Description)
            .Include(x => x.Type);

        var movies = await resources.Aggregate()
            .VectorSearch(movie => movie.Embedding, embedding, 150, vectorOptions)
            .Project<Resource>(projection)
            .ToListAsync(cancellationToken);

        return movies;
    }
}
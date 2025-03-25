namespace Seiyo.Services;

public interface IEmbeddingGenerator
{
    Task<double[]> FromTextAsync(string text, CancellationToken cancellationToken = default);
}
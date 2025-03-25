using Seiyo.Models;

namespace Seiyo.Services;

public interface IResourceSearcher
{
    Task<IEnumerable<Resource>> FromTextAsync(string text, CancellationToken cancellationToken = default);
}
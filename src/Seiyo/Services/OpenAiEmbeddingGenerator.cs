using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Seiyo.Settings;

namespace Seiyo.Services;

public class OpenAiEmbeddingGenerator(HttpClient client, IOptions<OpenAiSettings> options) : IEmbeddingGenerator
{
    public async Task<double[]> FromTextAsync(string text, CancellationToken cancellationToken = default)
    {
        var body = new Dictionary<string, object>
        {
            { "model", options.Value.EmbeddingModel },
            { "input", text }
        };

        var requestBody = JsonSerializer.Serialize(body);
        var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/v1/embeddings", requestContent, cancellationToken);

        if (!response.IsSuccessStatusCode) throw new Exception();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(responseBody)!;
        return embeddingResponse.Data[0].Embedding;
    }
}

public record EmbeddingResponse
{
    [JsonPropertyName("object")] public string Object { get; init; } = null!;
    [JsonPropertyName("data")] public EmbeddingDataResponse[] Data { get; init; } = [];
    [JsonPropertyName("model")] public string Model { get; init; } = null!;
    [JsonPropertyName("usage")] public EmbeddingUsageResponse EmbeddingUsageResponse { get; init; } = null!;
}

public record EmbeddingDataResponse
{
    [JsonPropertyName("object")] public string Object { get; init; } = null!;
    [JsonPropertyName("index")] public int Index { get; init; }
    [JsonPropertyName("embedding")] public double[] Embedding { get; init; } = null!;
}

public record EmbeddingUsageResponse
{
    [JsonPropertyName("prompt_tokens")] public int PromptTokens { get; init; }
    [JsonPropertyName("total_tokens")] public int TotalTokens { get; init; }
}
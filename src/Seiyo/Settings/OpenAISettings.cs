using System.ComponentModel.DataAnnotations;

namespace Seiyo.Settings;

public record OpenAiSettings
{
    public const string Section = "OpenAI";

    [Required] public string Endpoint { get; set; } = null!;

    [Required] public string Key { get; set; } = null!;

    [Required] public string EmbeddingModel { get; set; } = null!;
    public int Timeout { get; set; } = 30;
}
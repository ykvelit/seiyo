using System.Text.Json.Serialization;

namespace Seiyo.Models;

public class BusinessObject
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = null!;

    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("areas")] public IEnumerable<string> Areas { get; set; } = [];

    [JsonPropertyName("properties")] public IEnumerable<BusinessObjectProperty> Properties { get; set; } = [];

    [JsonPropertyName("parameters")] public IEnumerable<BusinessObjectParameter> Parameters { get; set; } = [];
}

public class BusinessObjectParameter
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = null!;

    [JsonPropertyName("type")] public string Type { get; set; } = null!;

    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
}

public class BusinessObjectProperty
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = null!;

    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("type")] public string Type { get; set; } = null!;

    [JsonPropertyName("properties")] public IEnumerable<BusinessObjectProperty> Properties { get; set; } = [];
}
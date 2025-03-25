namespace Seiyo.Settings;

public record MongoDbSettings
{
    public const string Section = "MongoDB";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
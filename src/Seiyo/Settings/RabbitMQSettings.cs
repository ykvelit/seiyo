namespace Seiyo.Settings;

public class RabbitMqSettings
{
    public const string Section = "RabbitMQ";
    public string ConnectionString { get; set; } = null!;
}
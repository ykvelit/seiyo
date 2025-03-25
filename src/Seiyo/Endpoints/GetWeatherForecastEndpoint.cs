namespace Seiyo.Endpoints;

public static class GetWeatherForecastEndpoint
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public static void MapRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", Handle)
            .WithName("GetWeatherForecast")
            .WithOpenApi();
    }

    private static IResult Handle()
    {
        var forecast = Enumerable.Range(1, 5)
            .Select(index => new WeatherForecastResponse(DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]))
            .ToArray();

        return Results.Ok(forecast);
    }
}

public record WeatherForecastResponse(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
using System.Reflection;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Seiyo.Messages;
using Seiyo.Services;
using Seiyo.Settings;

namespace Seiyo;

public static class DependencyInjection
{
    public static void AddRootModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceModule(configuration);
        services.AddMongoDbModule(configuration);
        services.AddMassTransitModule(configuration);
    }

    private static void AddServiceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<OpenAiSettings>()
            .Bind(configuration.GetSection(OpenAiSettings.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<IEmbeddingGenerator, OpenAiEmbeddingGenerator>(httpClient =>
        {
            var settings = new OpenAiSettings();
            configuration.GetSection(OpenAiSettings.Section).Bind(settings);

            httpClient.BaseAddress = new Uri(settings.Endpoint);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.Key}");
            httpClient.Timeout = TimeSpan.FromSeconds(settings.Timeout);
        });

        services.AddScoped<IResourceSearcher, ResourceSearcher>();
    }

    private static void AddMongoDbModule(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new MongoDbSettings();
        configuration.GetSection(MongoDbSettings.Section).Bind(settings);

        services.AddTransient<IMongoClient>(x => new MongoClient(settings.ConnectionString));
        services.AddTransient(x => x.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));
    }

    private static void AddMassTransitModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumers(Assembly.GetExecutingAssembly());

            configurator.UsingRabbitMq((context, cfg) =>
            {
                var settings = new RabbitMqSettings();
                configuration.GetSection(RabbitMqSettings.Section).Bind(settings);

                cfg.Host(settings.ConnectionString);
                cfg.ConfigureEndpoints(context);

                // Retentativas fazendo novas entregas com delay
                cfg.UseDelayedRedelivery(x =>
                    x.Exponential(
                        10,
                        TimeSpan.FromMinutes(1),
                        TimeSpan.FromDays(7),
                        TimeSpan.FromMinutes(21))
                );

                // Retentativas com delay ainda em memória
                cfg.UseMessageRetry(x =>
                    x.Intervals(
                        TimeSpan.FromSeconds(7),
                        TimeSpan.FromSeconds(31)
                    )
                );
            });
        });
    }
}
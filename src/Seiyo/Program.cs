using Seiyo;
using Seiyo.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRootModule(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

GetWeatherForecastEndpoint.MapRoute(app);
GetResourceByTextEndpoint.MapRoute(app);
CreateResourceEndpoint.MapRoute(app);

await app.RunAsync();
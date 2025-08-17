using WeatherForecast.Application;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/api/weather", (IWeatherForecastService weatherForecastService, string city) =>
{
    return weatherForecastService.GetWeatherByCityAsync(city);
})
//.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/api/weather", (CreateWeatherForecastDto weatherForecast, IWeatherForecastService weatherForecastService) =>
{
    return weatherForecastService.AddWeatherForecastAsync(weatherForecast);
})
.WithOpenApi();

app.Run();


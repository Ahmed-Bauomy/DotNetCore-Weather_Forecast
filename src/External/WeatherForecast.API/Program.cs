using Microsoft.EntityFrameworkCore;
using WeatherForecast.Application;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Infrastructure;
using WeatherForecast.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

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

app.MapPost("/api/weather", (IWeatherForecastService weatherForecastService, CreateWeatherForecastDto createWeatherForecastDto) =>
{
    return weatherForecastService.AddWeatherForecastAsync(createWeatherForecastDto);
})
//.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/api/auth/register", (UserDto userDto, IAuthService authService) =>
{
    return authService.RegisterAsync(userDto);
})
.WithOpenApi();

app.MapPost("/api/auth/login", (UserDto userDto, IAuthService authService) =>
{
    return authService.LoginAsync(userDto);
})
.WithOpenApi();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // This applies any pending migrations
}


app.Run();


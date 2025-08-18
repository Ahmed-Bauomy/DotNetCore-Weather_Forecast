using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer,
        ValidAudience = jwtSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/weather", (IWeatherForecastService weatherForecastService, string city) =>
{
    return weatherForecastService.GetWeatherByCityAsync(city);
})
.WithOpenApi()
.RequireAuthorization();

app.MapPost("/api/weather", (IWeatherForecastService weatherForecastService, CreateWeatherForecastDto createWeatherForecastDto) =>
{
    return weatherForecastService.AddWeatherForecastAsync(createWeatherForecastDto);
})
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

app.MapGet("/api/User", (string email, IAuthService authService) =>
{
    return authService.GetByEmailAsync(email);
})
.WithOpenApi();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // This applies any pending migrations
    // seed data
    var seed = scope.ServiceProvider.GetRequiredService<SeedData>();
    await seed.SetUpWeatherForecastData();
}




app.Run();

public partial class Program { } // needed for WebApplicationFactory


using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Behaviours;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Services;

namespace WeatherForecast.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>  cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IValidationBehaviour<>), typeof(ValidationBehaviour<>));
            services.AddSingleton<ICacheService, MemoryCacheService>();
            return services;
        }
    }
}

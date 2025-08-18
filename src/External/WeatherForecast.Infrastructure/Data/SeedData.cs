using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Domain.Contracts;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Infrastructure.Data
{
    public class SeedData
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private static readonly string[] ConditionPool =
        {
            "Sunny", "Partly Cloudy", "Cloudy", "Overcast",
            "Light Rain", "Rain", "Thunderstorm",
            "Drizzle", "Fog", "Windy"
        };
        private List<string> cities = ["Cairo", "London", "Paris", "New York", "Tokyo"];

        public SeedData(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public async Task SetUpWeatherForecastData()
        {
            var weatherForecasts = new List<WeatherFC>();
            foreach (var city in cities)
            {
                weatherForecasts.Add(Generate(city));
            }
            foreach (var weather in weatherForecasts)
            {
                var existingWeather = await _weatherForecastRepository.GetWeatherByCityAsync(weather.City);
                if (existingWeather == null) await _weatherForecastRepository.AddAsync(weather);
            }
        }
        /// <summary>
        /// Generate a list of mock forecasts starting today.
        /// </summary>
        private WeatherFC Generate(string city, int? seed = null)
        {
            var rng = seed.HasValue ? new Random(seed.Value) : Random.Shared;
            double baseTemp = 22 + 8 * Math.Sin(DateTime.UtcNow.DayOfYear * Math.PI / 180.0);
            double temp = Math.Round(baseTemp + rng.NextDouble() * 10 - 5, 1);

            return new WeatherFC
            {
                Date = DateTime.UtcNow,
                TemperatureCelsius = temp,
                Conditions = ConditionPool[rng.Next(ConditionPool.Length)],
                HumidityPercentage = Math.Round(40 + rng.NextDouble() * 60, 0), // 40–100%
                WindSpeedKmh = Math.Round(rng.NextDouble() * 40, 1),            // 0–40 km/h
                City = city
            };
        }

        
    }
}

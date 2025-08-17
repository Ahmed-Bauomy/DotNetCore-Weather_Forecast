using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Domain.Contracts
{
    public interface IWeatherForecastRepository : IAsyncRepository<WeatherFC>
    {
        Task<WeatherFC> GetWeatherByCityAsync(string city);
    }
}

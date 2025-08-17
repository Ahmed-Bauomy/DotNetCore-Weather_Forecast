using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Dtos;

namespace WeatherForecast.Application.Contracts
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherByCityAsync(string city);
    }
}

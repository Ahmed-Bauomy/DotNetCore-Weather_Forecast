using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Domain.Contracts;

namespace WeatherForecast.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly IMapper _mapper;

        public WeatherService(IWeatherForecastRepository weatherForecastRepository, IMapper mapper)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _mapper = mapper;
        }

        public async Task<WeatherDto> GetWeatherByCityAsync(string city)
        {
            var weather = await _weatherForecastRepository.GetWeatherByCityAsync(city);
            return _mapper.Map<WeatherDto>(weather);
        }
    }
}

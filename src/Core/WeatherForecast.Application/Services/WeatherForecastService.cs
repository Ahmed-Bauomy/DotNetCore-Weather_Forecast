using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Domain.Contracts;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Application.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly IMapper _mapper;
        private readonly IValidationBehaviour<CreateWeatherForecastDto> _createWeatherForecastValidator;
        private readonly ICacheService _cacheService;

        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository, 
                                      IMapper mapper,
                                      IValidationBehaviour<CreateWeatherForecastDto> createWeatherForecastValidator,
                                      ICacheService cacheService)
        {
            _weatherForecastRepository = weatherForecastRepository;
            _mapper = mapper;
            _createWeatherForecastValidator = createWeatherForecastValidator;
            _cacheService = cacheService;
        }

        public async Task<WeatherForecastDto> GetWeatherByCityAsync(string city)
        {
            // TODO: can be modified to use decorator design pattern
            var key = $"WeatherForecast_{city}";
            var cashedData = _cacheService.Get<WeatherForecastDto>(key);
            if (cashedData != null) return cashedData;
            var weather = await _weatherForecastRepository.GetWeatherByCityAsync(city);
            if(weather != null) _cacheService.Set(key, weather,TimeSpan.FromMinutes(60));
            return _mapper.Map<WeatherForecastDto>(weather);
        }

        public async Task<WeatherForecastDto> AddWeatherForecastAsync(CreateWeatherForecastDto weatherForecastDto)
        {
            await _createWeatherForecastValidator.Validate(weatherForecastDto);
            var weatherForecast = _mapper.Map<WeatherFC>(weatherForecastDto);
            var result = await _weatherForecastRepository.AddAsync(weatherForecast);
            return _mapper.Map<WeatherForecastDto>(result);
        }
    }
}

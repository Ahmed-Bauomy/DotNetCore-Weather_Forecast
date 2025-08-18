using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Behaviours;
using WeatherForecast.Application.Contracts;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Application.Mapping;
using WeatherForecast.Application.Services;
using WeatherForecast.Application.Validators;
using WeatherForecast.Domain.Contracts;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.UnitTest
{
    public class WeatherForecastServiceTests
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly Mock<IWeatherForecastRepository> _weatherRepositoryMock;
        private readonly ValidationBehaviour<CreateWeatherForecastDto> _validationBehaviour;
        private readonly IMapper _mapper;
        private readonly Mock<ICacheService> _memoryCacheService;
        public WeatherForecastServiceTests()
        {
            _weatherRepositoryMock = new Mock<IWeatherForecastRepository>();
            var loggerFactory = LoggerFactory.Create(c =>
            {
                c.AddConsole();
                c.SetMinimumLevel(LogLevel.Information);
            });
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            },loggerFactory);
            _mapper = new Mapper(config);
            _validationBehaviour = new ValidationBehaviour<CreateWeatherForecastDto>([new CreateWeatherForecastValidator()]);
            _memoryCacheService = new Mock<ICacheService>();
            _weatherForecastService = new WeatherForecastService(_weatherRepositoryMock.Object, _mapper, _validationBehaviour,_memoryCacheService.Object);
        }

        private void SetupGetWeatherForecastByCity(string city,WeatherFC weatherForecast)
        {
            _weatherRepositoryMock.Setup(r => r.GetWeatherByCityAsync(city)).ReturnsAsync(weatherForecast);
            _memoryCacheService.Setup(t => t.Get<WeatherForecastDto>(city)).Returns((WeatherForecastDto)null);
        }
        [Fact]
        public async Task Should_return_WeatherForecast_ByCity()
        {
            //arrange
            var city = "Cairo";
            var weatherForecast = new WeatherFC()
            {
                City = city,
                Date = DateTime.Now,
                TemperatureCelsius = 25,
                Conditions = "test",
                HumidityPercentage = 40,
                WindSpeedKmh = 30
            };
            SetupGetWeatherForecastByCity(city,weatherForecast);
            //act
            var result = await _weatherForecastService.GetWeatherByCityAsync(city);

            result.City.Should().Be(city);
        }
    }
}

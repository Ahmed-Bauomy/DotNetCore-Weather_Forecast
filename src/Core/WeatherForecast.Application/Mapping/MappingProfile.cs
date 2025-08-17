using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WeatherFC, WeatherForecastDto>().ReverseMap();
            CreateMap<WeatherFC, CreateWeatherForecastDto>().ReverseMap();
        }
    }
}

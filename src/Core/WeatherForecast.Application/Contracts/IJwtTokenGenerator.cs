using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Dtos;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Application.Contracts
{
    public interface IJwtTokenGenerator
    {
        TokenResult GenerateToken(User user);
    }
}

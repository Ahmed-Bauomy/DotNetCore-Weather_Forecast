using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Dtos;

namespace WeatherForecast.Application.Contracts
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDto userDto);
        Task<TokenResult> LoginAsync(UserDto userDto);
    }
}

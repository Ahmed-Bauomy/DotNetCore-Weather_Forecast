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
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IValidationBehaviour<UserDto> _registerUserValidator;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, 
                           IJwtTokenGenerator jwtTokenGenerator, 
                           IValidationBehaviour<UserDto> registerUserValidator,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _registerUserValidator = registerUserValidator;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(UserDto userDto)
        {
            await _registerUserValidator.Validate(userDto);
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Username = userDto.Email,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            return _mapper.Map<UserDto>(await _userRepository.AddAsync(user));
        }

        public async Task<TokenResult> LoginAsync(UserDto userDto)
        {
            var user = await _userRepository.GetByEmailAsync(userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }
    }
}

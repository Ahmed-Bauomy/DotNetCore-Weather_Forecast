using AutoMapper;
using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly ValidationBehaviour<UserDto> _validationBehaviour;
        private readonly IMapper _mapper;
        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var jwtSettings = new JwtSettings
            {
                Key = "Better_have_it_somewhere_safer!!",
                ValidIssuer = "localhost",
                ValidAudience = "localhost",
                TokenExpiresInMinutes = 10
            };

            var options = Options.Create(jwtSettings);
            _jwtGenerator = new JwtTokenGenerator(options);
            _validationBehaviour = new ValidationBehaviour<UserDto>([new RegisterUserValidator()]);
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            },loggerFactory);
            var mapper = new Mapper(config);
            _mapper = mapper;
            _authService = new AuthService(_userRepositoryMock.Object, _jwtGenerator,_validationBehaviour,mapper);
        }

        private void SetupRegister(UserDto user)
        {
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync((User)null);
            var createdUser = _mapper.Map<User>(user);
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        }

        [Fact]
        public async void RegisterAsync_Should_Register_New_User_And_Return_Created_User()
        {
            // arrange
            var user = new UserDto() { Email = "test@email.com", Password = "P@ssword123" };
            SetupRegister(user);

            // act
            var result = await _authService.RegisterAsync(user);

            // assert
            result.Email.Should().Be(user.Email);
        }

        private void SetupLogin(User user)
        {
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
        }
        [Fact]
        public async Task LoginAsync_Should_Return_Token_When_Valid_Credentials()
        {
            var user = new User() { Username = "test@email.com", Email = "test@email.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssword123") };
            var userDto = new UserDto() { Email = "test@email.com", Password = "P@ssword123" };
            // arrange
            SetupLogin(user);
            // act
            var result = await _authService.LoginAsync(userDto);

            // assert
            result.AccessToken.Should().NotBeNull();
        }
    }
}

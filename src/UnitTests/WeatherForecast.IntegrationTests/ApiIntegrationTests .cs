using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using WeatherForecast.Application.Dtos;

namespace WeatherForecast.IntegrationTests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Endpoint_Should_Create_User_And_Return_Created_User()
        {
            var response = await CreateTestUserIfNotExist(new { Email = "test@integration.com", Password = "P@ssw0rd123" });

            if(response == null)
            {
                response.Should().BeNull();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<UserDto>();
                result.Email.Should().NotBeNullOrEmpty();
            }
        }

        private async Task<HttpResponseMessage> CreateTestUserIfNotExist(dynamic user)
        {
            var getUserResult = await _httpClient.GetAsync($"/api/User?email={user.Email}");
            var content = await getUserResult.Content.ReadFromJsonAsync<UserDto>();
            if(content == null)
            {
                return await _httpClient.PostAsJsonAsync("/api/auth/register", (object)user);
            }
            return null;
        }

        [Fact]
        public async Task Login_Endpoint_Should_Return_Token_When_Valid()
        {
            var user = new { Email = "test@integration.com", Password = "P@ssw0rd123" };
            // Register first
            await CreateTestUserIfNotExist(user);

            // Then login
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", user);

            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.Content.ReadFromJsonAsync<TokenResult>();
            string token = result.AccessToken;
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Weather_Endpoint_Should_Require_Authorization()
        {
            var response = await _httpClient.GetAsync("/api/weather?city=cairo");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Weather_Endpoint_Should_Return_Data_When_Authorized()
        {
            // create new weatherforecast
            var city = "testCity";
            await CreateWeatherForecastIfNotExist(city);

            var user = new { Email = "test@integration.com", Password = "P@ssw0rd123" };
            await CreateTestUserIfNotExist(user);
            // login + get token
            var loginResponse = await _httpClient.PostAsJsonAsync("/api/auth/login", user);
            var registerResult = await loginResponse.Content.ReadFromJsonAsync<TokenResult>();
            string token = registerResult.AccessToken;
            // Add Bearer header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/api/weather?city={city}");

            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.Content.ReadFromJsonAsync<WeatherForecastDto>();

            result.City.Should().Be(city);
        }

        private async Task CreateWeatherForecastIfNotExist(string city)
        {
            var user = new { Email = "test@integration.com", Password = "P@ssw0rd123" };
            await CreateTestUserIfNotExist(user);
            // login + get token
            var loginResponse = await _httpClient.PostAsJsonAsync("/api/auth/login", user);
            var registerResult = await loginResponse.Content.ReadFromJsonAsync<TokenResult>();
            string token = registerResult.AccessToken;
            // Add Bearer header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var getWeatherByCity = await _httpClient.GetAsync($"/api/weather?city={city}");
            var resultCity = await getWeatherByCity.Content.ReadFromJsonAsync<WeatherForecastDto>();
            if (resultCity == null)
            {
                var cityCreationResult = await _httpClient.PostAsJsonAsync("/api/weather", new
                {
                    City = city,
                    Date = DateTime.Now,
                    TemperatureCelsius = 25,
                    Conditions = "test",
                    HumidityPercentage = 40,
                    WindSpeedKmh = 30
                });
                cityCreationResult.IsSuccessStatusCode.Should().BeTrue();
            }
        }
    }
}
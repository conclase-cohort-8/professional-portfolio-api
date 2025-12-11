using Microsoft.Extensions.Configuration;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.DTOs;
using System.Net.Http.Json;

namespace ProfessionalPortfolio.Application.Services
{
    public class WeatherforecastService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public WeatherforecastService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? 
                throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ApiResult<WeatherDto?>> GetForecast(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return new ApiResult<WeatherDto?>(null);
            }

            var client = _httpClientFactory.CreateClient("WeatherForecastService");
            var httpResponse = await client.GetAsync($"/v1/current.json?key={_apiKey}&q={location}");

            if(httpResponse == null || !httpResponse.IsSuccessStatusCode)
            {
                return new ApiResult<WeatherDto?>(null);
            }

            var forecast = await httpResponse.Content.ReadFromJsonAsync<WeatherDto>();
            return new ApiResult<WeatherDto?>(forecast);
        }
    }
}

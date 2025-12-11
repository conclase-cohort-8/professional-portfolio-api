using ProfessionalPortfolio.Application.DTOs;
using Refit;

namespace ProfessionalPortfolio.Application.Common.Interfaces
{
    public interface IWeatherForecast
    {
        [Get("/v1/current.json")]
        Task<ApiResponse<WeatherDto?>> Get(string key, string q);
    }
}

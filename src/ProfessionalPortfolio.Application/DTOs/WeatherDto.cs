using System.Text.Json.Serialization;

namespace ProfessionalPortfolio.Application.DTOs
{
    public class WeatherDto
    {
        public WeatherLocationDto? Location { get; set; }
        public CurrentWeatherDto? Current { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace ProfessionalPortfolio.Application.DTOs
{
    public class CurrentWeatherDto
    {
        [JsonPropertyName("temp_c")]
        public double TempC { get; set; }
        [JsonPropertyName("temp_f")]
        public double TempF { get; set; }
        public WeatherConditionDto? Condition { get; set; }
        [JsonPropertyName("feelslike_c")]
        public double FeelsLikeC { get; set; }
        [JsonPropertyName("feelslike_f")]
        public double FeelsLikeF { get; set; }
        [JsonPropertyName("wind_kph")]
        public double WindKph { get; set; }
        [JsonPropertyName("wind_mph")]
        public double WindMph { get; set; }
    }
}

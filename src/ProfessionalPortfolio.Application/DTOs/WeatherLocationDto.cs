using System.Text.Json.Serialization;

namespace ProfessionalPortfolio.Application.DTOs
{
    public class WeatherLocationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
        [JsonPropertyName("tz_id")]
        public string TimeZone { get; set; } = string.Empty;
    }
}

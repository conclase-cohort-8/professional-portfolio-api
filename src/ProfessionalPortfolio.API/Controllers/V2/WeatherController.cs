using Microsoft.AspNetCore.Mvc;
using ProfessionalPortfolio.Application.Common;
using ProfessionalPortfolio.Application.Common.Interfaces;
using ProfessionalPortfolio.Application.DTOs;
using ProfessionalPortfolio.Application.Services;

namespace ProfessionalPortfolio.API.Controllers.V2
{
    [Route("api/v{version:apiversion}/weather")]
    [ApiVersion("2.0")]
    [ApiController]
    public class WeatherController : ApiControllerBase
    {
        private readonly WeatherforecastService _service;
        private readonly IWeatherForecast _weatherForecast;
        private readonly string _apiKey;

        public WeatherController(WeatherforecastService service, 
            IWeatherForecast weatherForecast, IConfiguration configuration)
        {
            _service = service;
            _weatherForecast = weatherForecast;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? 
                throw new ArgumentNullException("WeatherApi:ApiKey");
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string location)
        {
            var response = await _service.GetForecast(location);
            return FromResponse(response);
        }

        [HttpGet("refit")]
        public async Task<IActionResult> GetRefit([FromQuery] string location)
        {
            var refitResponse = await _weatherForecast.Get(_apiKey, location);
            var response = refitResponse != null && refitResponse.IsSuccessStatusCode ?
                refitResponse.Content : null;

            return FromResponse(new ApiResult<WeatherDto?>(response));
        }
    }
}

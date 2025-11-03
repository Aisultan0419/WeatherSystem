using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherApplication;
using WeatherApplication.DTO;
using WeatherApplication.Enums;

namespace WeatherSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherDisplay : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherFacade _facade;
        private readonly OpenWeatherOptions _options;

        public WeatherDisplay(WeatherFacade facade, IOptions<OpenWeatherOptions> options, IHttpClientFactory httpClientFactory)
        {
            _facade = facade;
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<WeatherGetDTO>> GetWeather([FromQuery] string city = "Almaty", [FromQuery] StrategyType strategy = StrategyType.RealTime)
        {
            if (string.IsNullOrEmpty(_options?.ApiKey))
            {
                return StatusCode(500, "Missing OpenWeather API key in configuration.");
            }

            var client = _httpClientFactory.CreateClient();
            var data = await _facade.GetWeatherAsync(city, strategy, client, _options.ApiKey);
            if (data == null) return StatusCode(502, "Failed to fetch from upstream API");
            return Ok(data);
        }
    }
}

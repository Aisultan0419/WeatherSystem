using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
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
        private readonly WeatherService _weatherService;
        private readonly OpenWeatherOptions _options;
        public WeatherDisplay(WeatherService weatherService, IOptions<OpenWeatherOptions> options, IHttpClientFactory httpClientFactory)
        {
            _weatherService = weatherService;
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
            var ctx = new WeatherContext
            {
                City = city,
                HttpClient = client,
                ApiKey = _options.ApiKey
            };

            var data = await _weatherService.GetByStrategyAsync(strategy, ctx);
            if (data == null) return StatusCode(502, "Failed to fetch from upstream API");
            return Ok(data);
        }
    }
}

using Microsoft.Extensions.Logging;
namespace WeatherApplication.DTO
{
    public class WeatherContext
    {
        public string City { get; init; } = "Almaty";
        public HttpClient HttpClient { get; init; } = default!;
        public string? ApiKey { get; init; }
    }
}

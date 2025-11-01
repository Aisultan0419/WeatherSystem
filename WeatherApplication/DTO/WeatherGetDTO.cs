
namespace WeatherApplication.DTO
{
    public class WeatherGetDTO
    {
        public string City { get; set; } = default!;
        public double TemperatureC { get; set; }
        public int Humidity { get; set; }
        public string? Summary { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

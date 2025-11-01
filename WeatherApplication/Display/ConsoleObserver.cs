
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Display
{
    public class ConsoleObserver : IWeatherObserver
    {
        private readonly string _name;
        private readonly string _city;
        public ConsoleObserver(string name, string city) { _name = name; _city = city; }
        public void Update(WeatherGetDTO weather)
        {
            if (!string.Equals(weather.City, _city, StringComparison.OrdinalIgnoreCase)) return;
            Console.WriteLine($"[{_name}] {weather.City}: {weather.TemperatureC}C {weather.Summary} at {weather.Timestamp}");
        }
    }


}

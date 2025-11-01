using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Display
{
    public class FileObserver : IWeatherObserver
    {
        private readonly string _city;
        private readonly string _fileName;

        public FileObserver(string city)
        {
            _city = city;
            _fileName = $"{city}_{DateTime.UtcNow:yyyyMMddHHmmss}.log";
        }

        public void Update(WeatherGetDTO weather)
        {
            if (!string.Equals(weather.City, _city, StringComparison.OrdinalIgnoreCase)) return;
            File.AppendAllText(_fileName, $"{DateTime.UtcNow:u} {weather.City} {weather.TemperatureC}C {weather.Summary}\n");
        }
    }


}

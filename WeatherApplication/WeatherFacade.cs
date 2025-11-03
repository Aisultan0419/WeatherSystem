using WeatherApplication.Display;
using WeatherApplication.DTO;
using WeatherApplication.Enums;
using WeatherApplication.Interfaces;

namespace WeatherApplication
{
    public class WeatherFacade
    {
        private readonly WeatherService _weatherService;
        private readonly WeatherStation _station;
        private readonly IObserverFactory _observerFactory;

        public WeatherFacade(WeatherService weatherService,
                             WeatherStation station,
                             IObserverFactory observerFactory)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _station = station ?? throw new ArgumentNullException(nameof(station));
            _observerFactory = observerFactory ?? throw new ArgumentNullException(nameof(observerFactory));
        }
        public async Task<WeatherGetDTO?> GetWeatherAsync(string city, StrategyType strategy,
                                                         HttpClient httpClient, string? apiKey)
        {
            if (string.IsNullOrWhiteSpace(city)) city = "Almaty";
            var ctx = new WeatherContext
            {
                City = city,
                HttpClient = httpClient,
                ApiKey = apiKey
            };

            return await _weatherService.GetByStrategyAsync(strategy, ctx);
        }
        public Guid Subscribe(SubscribeDto subscriber, WeatherGetDTO? initialWeather = null)
        {
            var observer = _observerFactory.Create(subscriber);
            var id = _station.Subscribe(observer);
            if (initialWeather != null)
            {
                _station.Notify(initialWeather);
            }
            return id;
        }

        public bool Unsubscribe(Guid id) => _station.Unsubscribe(id);
    }
}

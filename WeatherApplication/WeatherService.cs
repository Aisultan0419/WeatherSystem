using WeatherApplication.Enums;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication
{
    public class WeatherService
    {
        private readonly IWeatherFactory _factory;
        public WeatherService(IWeatherFactory factory)
        {
            _factory = factory;
        }
        public async Task<WeatherGetDTO?> GetByStrategyAsync(StrategyType type, WeatherContext ctx)
        {
            var strategy = _factory.Create(type);
            var data = await strategy.UpdateWeatherAsync(ctx);
            return data;
        }
    }
}

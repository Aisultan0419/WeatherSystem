using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Enums;
using WeatherApplication.Interfaces;
using WeatherApplication.Strategies;

namespace WeatherApplication.Factories
{
    public class WeatherFactory : IWeatherFactory
    {
        private readonly IServiceProvider _sp;
        public WeatherFactory(IServiceProvider sp)
        {
            _sp = sp;
        }
        public IUpdateStrategy Create(StrategyType type)
        {
            return type switch
            {
                StrategyType.RealTime => _sp.GetRequiredService<RealTimeStrategy>(),
                StrategyType.Batch => _sp.GetRequiredService<BatchStrategy>(),
                StrategyType.Manual => _sp.GetRequiredService<ManualStrategy>(),
                _ => throw new NotSupportedException()
            };
        }
    }
}

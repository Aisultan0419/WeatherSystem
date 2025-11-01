using WeatherApplication.DTO;
namespace WeatherApplication.Interfaces
{
    public interface IObserverFactory
    {
        IWeatherObserver Create(SubscribeDto dto);
    }
}

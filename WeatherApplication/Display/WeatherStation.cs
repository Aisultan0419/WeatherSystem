using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Display
{
    public class WeatherStation
    {
        private readonly Dictionary<Guid, IWeatherObserver> _observers = new();

        public Guid Subscribe(IWeatherObserver observer)
        {
            var id = Guid.NewGuid();
            _observers[id] = observer;
            return id;
        }

        public bool Unsubscribe(Guid id) => _observers.Remove(id);

        public void Notify(WeatherGetDTO weather)
        {
            foreach (var obs in _observers.Values.ToList())
            {
                try { obs.Update(weather); }
                catch {  }
            }
        }
    }


}

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Display
{
    public class WebSocketObserver : IWeatherObserver
    {
        private readonly IHubContext<WeatherHub> _hub;
        private readonly string _connectionId;
        private readonly string _city;
        public WebSocketObserver(IHubContext<WeatherHub> hub, string connectionId, string city)
        {
            _hub = hub; _connectionId = connectionId; _city = city;
        }
        public void Update(WeatherGetDTO weather)
        {
            if (!string.Equals(weather.City, _city, StringComparison.OrdinalIgnoreCase)) return;
            _hub.Clients.Client(_connectionId).SendAsync("WeatherUpdated", weather);
        }
    }

}

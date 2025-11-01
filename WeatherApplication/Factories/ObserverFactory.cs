using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Display;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Factories
{
    public class ObserverFactory : IObserverFactory
    {
        private readonly IHubContext<WeatherHub> _hub;

        public ObserverFactory(IHubContext<WeatherHub> hub)
        {
            _hub = hub;
        }

        public IWeatherObserver Create(SubscribeDto dto)
        {
            var method = dto.Method?.ToLowerInvariant();
            switch (method)
            {
                case "console":
                    return new ConsoleObserver(dto.Name, dto.City);
                case "file":
                    return new FileObserver(dto.City);
                case "web":
                    if (string.IsNullOrEmpty(dto.ConnectionId))
                        throw new ArgumentException("ConnectionId required for web subscription");
                    return new WebSocketObserver(_hub, dto.ConnectionId, dto.City);
                default:
                    throw new NotSupportedException($"Unknown method {dto.Method}");
            }
        }
    }
}

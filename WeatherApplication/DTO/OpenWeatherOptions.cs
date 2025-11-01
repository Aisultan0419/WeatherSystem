using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.DTO
{
    public class OpenWeatherOptions
    {
        public string ApiKey { get; set; } = null!;
        public string BaseUrl { get; set; } = "https://api.openweathermap.org/data/2.5/weather";
    }

}

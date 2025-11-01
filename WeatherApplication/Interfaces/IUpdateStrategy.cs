using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.DTO;

namespace WeatherApplication.Interfaces
{
    public interface IUpdateStrategy
    {
        Task<WeatherGetDTO?> UpdateWeatherAsync(WeatherContext ctx);
    }
}

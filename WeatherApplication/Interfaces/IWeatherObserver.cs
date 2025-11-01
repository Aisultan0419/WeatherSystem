using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.DTO;

namespace WeatherApplication.Interfaces
{
    public interface IWeatherObserver
    {
        void Update(WeatherGetDTO wgD);
    }
}

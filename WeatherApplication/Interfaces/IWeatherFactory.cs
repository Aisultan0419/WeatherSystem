using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Enums;

namespace WeatherApplication.Interfaces
{
    public interface IWeatherFactory
    {
        IUpdateStrategy Create(StrategyType type);
    }
}

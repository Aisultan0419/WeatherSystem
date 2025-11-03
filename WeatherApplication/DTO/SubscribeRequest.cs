using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.DTO
{
    public class SubscribeRequest
    {
        public SubscribeDto Subscriber { get; set; } = new();
        public WeatherGetDTO Weather { get; set; } = new();
    }

}

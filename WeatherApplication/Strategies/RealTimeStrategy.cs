using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

namespace WeatherApplication.Strategies
{
    public class RealTimeStrategy : IUpdateStrategy
    {
        private readonly string _openWeatherUrl = "https://api.openweathermap.org/data/2.5/weather";

        public async Task<WeatherGetDTO?> UpdateWeatherAsync(WeatherContext ctx)
        {
            if (ctx.HttpClient == null) throw new ArgumentNullException(nameof(ctx.HttpClient));
            if (string.IsNullOrEmpty(ctx.ApiKey)) throw new ArgumentNullException(nameof(ctx.ApiKey));

            var url = $"{_openWeatherUrl}?q={Uri.EscapeDataString(ctx.City)}&units=metric&appid={ctx.ApiKey}";
            var res = await ctx.HttpClient.GetAsync(url);
            if (!res.IsSuccessStatusCode)
            {
                return null;
            }

            using var stream = await res.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var root = doc.RootElement;
            var temp = root.GetProperty("main").GetProperty("temp").GetDouble();
            var humidity = root.GetProperty("main").GetProperty("humidity").GetInt32();
            var summary = root.GetProperty("weather")[0].GetProperty("description").GetString();

            return new WeatherGetDTO
            {
                City = ctx.City,
                TemperatureC = temp,
                Humidity = humidity,
                Summary = summary,
                Timestamp = DateTime.UtcNow
            };
        }
    }

}

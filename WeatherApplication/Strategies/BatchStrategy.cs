using System.Text.Json;
using WeatherApplication.DTO;
using WeatherApplication.Interfaces;

public class BatchStrategy : IUpdateStrategy
{
    private readonly WeatherCache _cache;
    private readonly string _openWeatherUrl = "https://api.openweathermap.org/data/2.5/weather";

    public BatchStrategy(WeatherCache cache)
    {
        _cache = cache;
    }

    public async Task<WeatherGetDTO?> UpdateWeatherAsync(WeatherContext ctx)
    {
        if (ctx.HttpClient == null) throw new ArgumentNullException(nameof(ctx.HttpClient));
        if (string.IsNullOrEmpty(ctx.ApiKey)) throw new ArgumentNullException(nameof(ctx.ApiKey));
        var city = ctx.City?.Trim() ?? "";

        if (_cache.TryGet(city, out var cached))
        {
            if ((DateTime.UtcNow - cached!.Timestamp).TotalMinutes < 30)
                return cached;
        }

        var url = $"{_openWeatherUrl}?q={Uri.EscapeDataString(city)}&units=metric&appid={ctx.ApiKey}";
        var res = await ctx.HttpClient.GetAsync(url);
        if (!res.IsSuccessStatusCode) return null;

        using var stream = await res.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        var temp = root.GetProperty("main").GetProperty("temp").GetDouble();
        var humidity = root.GetProperty("main").GetProperty("humidity").GetInt32();
        var summary = root.GetProperty("weather")[0].GetProperty("description").GetString();

        var result = new WeatherGetDTO
        {
            City = city,
            TemperatureC = temp,
            Humidity = humidity,
            Summary = summary,
            Timestamp = DateTime.UtcNow
        };

        _cache.Set(city, result);
        return result;
    }
}

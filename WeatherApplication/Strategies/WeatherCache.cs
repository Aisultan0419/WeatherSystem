using System.Collections.Concurrent;
using WeatherApplication.DTO;

public class WeatherCache
{
    private readonly ConcurrentDictionary<string, WeatherGetDTO> _cache = new();

    public bool TryGet(string city, out WeatherGetDTO? dto) =>
        _cache.TryGetValue(Normalize(city), out dto);

    public void Set(string city, WeatherGetDTO dto) =>
        _cache[Normalize(city)] = dto;

    private static string Normalize(string city) => city?.Trim().ToLowerInvariant() ?? string.Empty;
}

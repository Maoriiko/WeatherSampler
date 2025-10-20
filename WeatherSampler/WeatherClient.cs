using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherSampler;
using WeatherSampler.Models;


public class WeatherClient
{
    private readonly IHttpClientFactory _http;
    private readonly OpenWeatherSettings _settings;

    public WeatherClient(IHttpClientFactory http, IOptions<OpenWeatherSettings> options)
    {
        _http = http;
        _settings = options.Value;
    }

    public async Task<(OpenWeatherResponse?, string)> GetCurrentWeatherAsync()
    {
        var client = _http.CreateClient();
        var url = $"{_settings.ApiUrl}?q={Uri.EscapeDataString(_settings.City)}&appid={_settings.ApiKey}&units=metric";
        using var resp = await client.GetAsync(url);
        var raw = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) return (null, raw);
        var doc = JsonSerializer.Deserialize<OpenWeatherResponse>(raw, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return (doc, raw);
    }
}

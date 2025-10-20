namespace WeatherSampler.Models;
public record WeatherMain(decimal temp, int humidity);
public record Wind(decimal speed);
public record OpenWeatherResponse(WeatherMain main, Wind wind, string name);

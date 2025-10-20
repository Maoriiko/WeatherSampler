namespace WeatherSampler.Models;
public record WeatherMessage
{
    public required string City { get; init; }
    public decimal Temperature { get; init; }
    public int Humidity { get; init; }
    public decimal WindSpeed { get; init; }
    public long TimestampMillis { get; init; }
    public required string TimestampIso { get; init; }
    public required string RawJson { get; init; }
}

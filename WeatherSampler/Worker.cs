using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WeatherSampler;

public class Worker : BackgroundService
{
    private readonly WeatherClient _weatherClient;
    private readonly RabbitMqPublisher _publisher;
    private readonly SamplingSettings _sampling;

    public Worker(WeatherClient weatherClient, RabbitMqPublisher publisher, IOptions<SamplingSettings> sampling)
    {
        _weatherClient = weatherClient;
        _publisher = publisher;
        _sampling = sampling.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var timestampMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var timestampIso = DateTimeOffset.UtcNow.ToString("o");
            var (resp, raw) = await _weatherClient.GetCurrentWeatherAsync();
            if (resp is not null)
            {
                var msg = new SimpleWeatherMessage
                {
                    City = resp.name,
                    Temperature = resp.main.temp
                };
                try
                {
                    if (msg.Temperature > 24 || msg.Temperature < 0)
                    {
                        await _publisher.ConnectAsync();
                        await _publisher.PublishAsync(msg);
                        Console.WriteLine($"{DateTimeOffset.UtcNow:o} Published weather {msg.City} {msg.Temperature}°C");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Publish failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"{DateTimeOffset.UtcNow:o} Weather API failed: {raw}");
            }
            var delay = TimeSpan.FromMinutes(Math.Max(1, _sampling.IntervalMinutes));
            await Task.Delay(delay, stoppingToken);
        }
    }
}

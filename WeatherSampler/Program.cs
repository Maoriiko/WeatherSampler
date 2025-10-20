using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherSampler;


var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json");
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        services.Configure<OpenWeatherSettings>(context.Configuration.GetSection("OpenWeather"));
        services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMq"));
        services.Configure<SamplingSettings>(context.Configuration.GetSection("Sampling"));
        services.AddSingleton<WeatherClient>();
        services.AddSingleton<RabbitMqPublisher>();
        services.AddHostedService<Worker>();
    });

await builder.RunConsoleAsync();


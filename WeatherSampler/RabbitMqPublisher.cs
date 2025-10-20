using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;
using WeatherSampler;
using WeatherSampler.Models;


public class RabbitMqPublisher : IAsyncDisposable, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Connects asynchronously to RabbitMQ and declares the queue.
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            AutomaticRecoveryEnabled = true
        };

        _connection = await factory.CreateConnectionAsync("WeatherSamplerPublisher", cancellationToken)
                                   .ConfigureAwait(false);

        _channel = await _connection.CreateChannelAsync();


        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    /// <summary>
    /// Publishes a message asynchronously.
    /// </summary>
    public async Task PublishAsync(object message)
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMQ connection is not established. Call ConnectAsync first.");

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        // יוצרים properties ידנית
        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json"
        };

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: _settings.QueueName,
            mandatory: false,
            basicProperties: props,
            body: body
        );

        Console.WriteLine($"[RabbitMQ] Published: {json}");
        Console.WriteLine($"RabbitMQ: {_settings.HostName}:{_settings.Port}, Queue: {_settings.QueueName}");

    }

    public void Dispose()
    {
        try
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            _channel?.Dispose();
            _connection?.Dispose();
        }
        catch (BrokerUnreachableException) { }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            _channel?.Dispose();
            _connection?.Dispose();
            await Task.CompletedTask;
        }
        catch (BrokerUnreachableException) { }
    }
}

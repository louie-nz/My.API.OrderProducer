using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
namespace My.API.OrderProducer.Kafka;
public class OrderConsumerService : BackgroundService
{
    private readonly KafkaSettings _settings;
    private readonly ILogger<OrderConsumerService> _logger;
    public OrderConsumerService(IOptions<KafkaSettings> settings, ILogger<OrderConsumerService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = "order-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_settings.Topic);
            _logger.LogInformation("Kafka consumer started, topic: {Topic}", _settings.Topic);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        _logger.LogInformation("Kafka consumed: {Message}", result.Message.Value);
                        // TODO: 在这里处理订单事件（写数据库、调用下游 API 等）
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Kafka consume error");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopping...");
            }
            finally
            {
                consumer.Close();
            }
        }, stoppingToken);
    }
}


using Confluent.Kafka;
using Microsoft.Extensions.Options;
namespace My.API.OrderProducer.Kafka;
public class MessageProducer : IMessageProducer
{
    private readonly KafkaSettings _settings;
    private readonly ILogger<MessageProducer> _logger;
    public MessageProducer(IOptions<KafkaSettings> settings, ILogger<MessageProducer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }
    public async Task ProduceAsync(string topic, string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _settings.BootstrapServers
        };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var dr = await producer.ProduceAsync(
                string.IsNullOrWhiteSpace(topic) ? _settings.Topic : topic,
                new Message<Null, string> { Value = message });
            _logger.LogInformation("Kafka produced to {Topic}, offset {Offset}", dr.Topic, dr.Offset);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error producing Kafka message");
            throw;
        }
    }
}


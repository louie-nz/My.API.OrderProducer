namespace My.API.OrderProducer.Kafka;
public interface IMessageProducer
{
    Task ProduceAsync(string topic, string message);
}

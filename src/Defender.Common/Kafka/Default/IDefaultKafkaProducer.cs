namespace Defender.Common.Kafka.Default;

public interface IDefaultKafkaProducer<in TValue>
{
    Task ProduceAsync(
        string topic, 
        TValue value,
        CancellationToken cancellationToken);

    void Flush();
}

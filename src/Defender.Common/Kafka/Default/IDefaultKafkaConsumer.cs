namespace Defender.Common.Kafka.Default;

public interface IDefaultKafkaConsumer<out TValue>
{
    public Task StartConsuming(
        string topic,
        Func<TValue,Task> handleMessage,
        CancellationToken cancellationToken);
}

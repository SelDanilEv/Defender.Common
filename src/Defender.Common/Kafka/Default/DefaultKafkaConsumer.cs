using Confluent.Kafka;
using Defender.Common.Configuration.Options.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Defender.Common.Kafka.Default;

public class DefaultKafkaConsumer<TValue> : IDefaultKafkaConsumer<TValue>, IDisposable
{
    private readonly IConsumer<Ignore, TValue> _consumer;
    private readonly ILogger<DefaultKafkaConsumer<TValue>> _logger;

    public DefaultKafkaConsumer(
        IOptions<KafkaOptions> kafkaOptions, 
        ILogger<DefaultKafkaConsumer<TValue>> logger,
        IDeserializer<TValue> valueSerializer)
    {
        if (string.IsNullOrWhiteSpace(kafkaOptions?.Value?.BootstrapServers))
        {
            throw new ArgumentException("BootstrapServers must be provided in KafkaOptions.", nameof(kafkaOptions));
        }

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
            GroupId = kafkaOptions.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<Ignore, TValue>(config)
            .SetValueDeserializer(valueSerializer)
            .SetErrorHandler((_, error) => OnError(error))
            .Build();
    }

    public async Task StartConsuming(
        string topic,
        Func<TValue, Task> handleMessage,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            throw new ArgumentException("Topic name cannot be null or empty.", nameof(topic));
        }

        if (handleMessage == null)
        {
            throw new ArgumentNullException(nameof(handleMessage), "Message handler cannot be null.");
        }

        _consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed to topic: {Topic}", topic);

        await Task.Run(async () =>
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);
                        
                        _logger.LogInformation("New message to consume {ConsumeResult}.", consumeResult);

                        if (consumeResult is not null && consumeResult.Message.Value is not null)
                            await handleMessage(consumeResult.Message.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Error consuming message from topic {Topic}.", topic);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer operation canceled.");
            }
            finally
            {
                _consumer.Close();
                _logger.LogInformation("Kafka consumer closed.");
            }
        }, cancellationToken);
    }

    private void OnError(Error error)
    {
        _logger.LogError("Kafka Consumer Error: {Reason}", error.Reason);
    }

    public void Dispose()
    {
        _consumer.Dispose();
        _logger.LogInformation("Kafka consumer disposed.");
    }
}

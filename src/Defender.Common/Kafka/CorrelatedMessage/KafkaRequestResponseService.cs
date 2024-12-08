using Confluent.Kafka;
using Defender.Common.Configuration.Options.Kafka;
using Defender.Common.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Defender.Common.Kafka.CorrelatedMessage;

public class KafkaRequestResponseService(
    IOptions<KafkaOptions> kafkaOptions) : IKafkaRequestResponseService
{
    public async Task<TResponse> SendAsync<TRequest, TResponse>(
        string requestTopic,
        string responseTopic,
        CorrelatedKafkaRequest<TRequest> correlatedKafkaRequest,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        var adminConfig = new AdminClientConfig
        {
            BootstrapServers = kafkaOptions.Value.BootstrapServers,
        };

        using var producer = new ProducerBuilder<string, string>
            (producerConfig).Build();
        using var consumer = new ConsumerBuilder<string, string>
            (consumerConfig).Build();
        using var admin = new AdminClientBuilder
            (adminConfig).Build();

        var metadata = admin.GetMetadata(responseTopic, TimeSpan.FromSeconds(10));
        var partitions = metadata.Topics
            .First(t => t.Topic == responseTopic)
            .Partitions
            .Select(p => new TopicPartition(responseTopic, p.PartitionId));

        consumer.Assign(partitions);

        var message = new Message<string, string>
        {
            Key = correlatedKafkaRequest.CorrelationId,
            Value = JsonSerializer.Serialize(correlatedKafkaRequest)
        };

        await producer.ProduceAsync(requestTopic, message, cancellationToken);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cts.Token);
                if (consumeResult == null) continue;

                try
                {
                    var response = JsonSerializer.Deserialize<CorrelatedKafkaResponse<TResponse>>(consumeResult.Message.Value);
                    if (consumeResult.Message.Key == correlatedKafkaRequest.CorrelationId)
                    {
                        return response!.GetResult;
                    }
                }
                catch (JsonException)
                {
                    continue;
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            throw new ServiceException(
                Errors.ErrorCode
                    .CM_NoResponseForTheCorrelatedKafkaRequest, ex);
        }
        catch (Exception ex)
        {
            throw new ServiceException(
                Errors.ErrorCode
                    .CM_KafkaIssue, ex);
        }

        throw new ServiceException(
            Errors.ErrorCode
                .CM_NoResponseForTheCorrelatedKafkaRequest,
                    new TimeoutException("Request timed out."));
    }
}



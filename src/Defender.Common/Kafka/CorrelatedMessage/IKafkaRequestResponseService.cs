namespace Defender.Common.Kafka.CorrelatedMessage;

public interface IKafkaRequestResponseService
{
    Task<TResponse> SendAsync<TRequest, TResponse>(
        string requestTopic,
        string responseTopic,
        CorrelatedKafkaRequest<TRequest> correlatedKafkaRequest,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}


namespace Defender.Common.Kafka.CorrelatedMessage;

public class CorrelatedKafkaRequest : CorrelatedKafkaRequest<string>
{
    public static CorrelatedKafkaRequest CreateDefault
        => new() { Message = String.Empty };
}

public class CorrelatedKafkaRequest<T>
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    public required T Message { get; set; }

    public CorrelatedKafkaResponse<RType> CreateResponse<RType>(RType response)
    {
        return new CorrelatedKafkaResponse<RType>
        {
            CorrelationId = CorrelationId,
            Message = response
        };
    }
}

public class CorrelatedKafkaResponse<T>
{
    public string? CorrelationId { get; set; }
    public required T Message { get; set; }

    public T GetResult => Message;
}

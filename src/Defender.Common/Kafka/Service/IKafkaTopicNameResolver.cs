namespace Defender.Common.Kafka.Service;

public interface IKafkaTopicNameResolver
{
    string ResolveTopicName(string topicName);
}

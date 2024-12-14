using Defender.Common.Extension;
using Microsoft.Extensions.Hosting;

namespace Defender.Common.Kafka.Service;

internal class KafkaTopicNameResolver(
        IHostEnvironment hostEnvironment)
    : IKafkaTopicNameResolver
{
    public string ResolveTopicName(string topicName)
    {
        var env = hostEnvironment.GetAppEnvironment();
        return $"{env}_{topicName}";
    }
}

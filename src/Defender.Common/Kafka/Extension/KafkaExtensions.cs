using Confluent.Kafka;
using Defender.Common.Configuration.Options.Kafka;
using Defender.Common.Kafka.CorrelatedMessage;
using Defender.Common.Kafka.Default;
using Defender.Common.Kafka.Service;
using Defender.Common.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Kafka.Extension;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services,
        Action<KafkaOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton(typeof(ISerializer<>), typeof(JsonSerializer<>));
        services.AddSingleton(typeof(IDeserializer<>), typeof(JsonSerializer<>));
        
        services.AddSingleton<IKafkaTopicNameResolver, KafkaTopicNameResolver>();
        
        services.AddTransient<IKafkaRequestResponseService, KafkaRequestResponseService>();
        services.AddTransient(typeof(IDefaultKafkaProducer<>),typeof(DefaultKafkaProducer<>));
        services.AddTransient(typeof(IDefaultKafkaConsumer<>),typeof(DefaultKafkaConsumer<>));

        return services;
    }
}

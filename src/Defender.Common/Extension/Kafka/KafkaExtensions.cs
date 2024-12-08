using Defender.Common.Kafka.CorrelatedMessage;
using Defender.Common.Kafka.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Extension.Kafka;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafkaRequestResponseService(
        this IServiceCollection services)
    {
        services.AddTransient<IKafkaRequestResponseService, KafkaRequestResponseService>();

        return services;
    }

    public static IServiceCollection AddDefaultKafkaProducer(
        this IServiceCollection services)
    {
        services.AddTransient(typeof(IDefaultKafkaProducer<>),typeof(DefaultKafkaProducer<>));

        return services;
    }

    public static IServiceCollection AddDefaultKafkaConsumer(
        this IServiceCollection services)
    {
        services.AddTransient(typeof(IDefaultKafkaConsumer<>),typeof(DefaultKafkaConsumer<>));

        return services;
    }
}

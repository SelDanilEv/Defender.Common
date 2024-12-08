using Confluent.Kafka;

using Defender.Common.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Extensions;

public static class SerializationExtensions
{
    public static IServiceCollection AddJsonSerializer(
        this IServiceCollection services)
    {
        services.AddSingleton(typeof(ISerializer<>), typeof(JsonSerializer<>));
        services.AddSingleton(typeof(IDeserializer<>), typeof(JsonSerializer<>));

        return services;
    }
}

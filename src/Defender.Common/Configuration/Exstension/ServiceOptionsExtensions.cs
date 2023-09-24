using Defender.Common.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Configuration.Exstension;

public static class ServiceOptionsExtensions
{
    public static IServiceCollection AddCommonOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

        return services;
    }
}
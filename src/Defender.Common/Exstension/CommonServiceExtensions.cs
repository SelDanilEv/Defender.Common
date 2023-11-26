using Defender.Common.Behaviours;
using Defender.Common.Configuration.Options;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.Common.Repositories.Secrets;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Exstension;

public static class CommonServiceExtensions
{
    public static IServiceCollection AddCommonOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

        services.PostConfigure<MongoDbOptions>(async options =>
        {
            options.ConnectionString = string.Format(
                options.ConnectionString,
                await SecretsHelper.GetSecretAsync(Secret.MongoDBPassword));
        });

        return services;
    }

    public static IServiceCollection AddCommonPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }

    public static IServiceCollection AddSecretAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IMongoSecretAccessor, SecretsRepository>();

        var mongoSecretAccesor = services
            .BuildServiceProvider()
            .GetRequiredService<IMongoSecretAccessor>();

        if (mongoSecretAccesor != null)
        {
            SecretsHelper.Initialize(mongoSecretAccesor);
        }

        return services;
    }

    public static IServiceCollection AddSecretManager(this IServiceCollection services)
    {
        services.AddSingleton<IMongoSecretService, SecretsRepository>();

        return services;
    }
}
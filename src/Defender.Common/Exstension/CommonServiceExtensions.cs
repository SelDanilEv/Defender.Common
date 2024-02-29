using Defender.Common.Accessors;
using Defender.Common.Behaviours;
using Defender.Common.Configuration.Options;
using Defender.Common.DB.Repositories.Account;
using Defender.Common.DB.Repositories.Secrets;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.Common.Service;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Exstension;

public static class CommonServiceExtensions
{
    /// <summary>
    /// Configure Accessors, Options, Secrets, Services, Pipelines
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonAccessors();
        services.AddCommonOptions(configuration);
        services.AddSecretAccessor();
        services.AddAccountInfoAccessor();
        services.AddCommonServices();
        services.AddCommonPipelines();

        return services;
    }

    private static IServiceCollection AddCommonOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));

        services.PostConfigure<MongoDbOptions>(options =>
        {
            options.ConnectionString = SecretsHelper
            .GetSecretAsync(Secret.MongoDBConnectionString)
            .GetAwaiter()
            .GetResult();
        });

        return services;
    }

    private static IServiceCollection AddCommonPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }

    private static IServiceCollection AddCommonAccessors(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentAccountAccessor, CurrentAccountAccessor>();
        services.AddSingleton<IAuthenticationHeaderAccessor, AuthenticationHeaderAccessor>();

        return services;
    }

    private static IServiceCollection AddSecretAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IMongoSecretAccessor, ROSecretRepository>();

        var mongoSecretAccesor = services
            .BuildServiceProvider()
            .GetRequiredService<IMongoSecretAccessor>();

        if (mongoSecretAccesor != null)
        {
            SecretsHelper.Initialize(mongoSecretAccesor);
        }

        return services;
    }

    private static IServiceCollection AddAccountInfoAccessor(this IServiceCollection services)
    {
        services.AddSingleton<IAccountAccessor, ROAccountInfoRepository>();

        return services;
    }

    private static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationCheckingService, AuthorizationCheckingService>();

        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Identity;

public static class ConfigureIdentityClient
{
    public static void RegisterIdentityClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(nameof(IdentityServiceClient), configureClient);
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Identity;

    public static class ConfigureIdentityClient
{
    public static void RegisterIdentityAsServiceClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IIdentityAsServiceClient, IdentityAsServiceClient>(nameof(IdentityAsServiceClient), configureClient);
    }

    public static void RegisterIdentityClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IIdentityClient, IdentityAsServiceClient>(nameof(IdentityAsServiceClient), configureClient);
    }

}

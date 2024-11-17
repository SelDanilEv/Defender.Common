using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Portal;

public static class ConfigurePortalApiClient
{
    public static void RegisterPortalClient(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IPortalApiClient, PortalApiClient>(
            nameof(PortalApiClient),
            configureClient);
    }

}

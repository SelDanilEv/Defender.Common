using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Wallet;

public static class ConfigureWalletClient
{
    public static void RegisterWalletClient(
        this IServiceCollection services, 
        Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IWalletServiceClient, WalletServiceClient>(nameof(WalletServiceClient), configureClient);
    }

}

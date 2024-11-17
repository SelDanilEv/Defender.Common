using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.RiskGames;

public static class ConfigureRiskGamesClient
{
    public static void RegisterRiskGamesClient(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IRiskGamesServiceClient, RiskGamesServiceClient>(
            nameof(RiskGamesServiceClient), configureClient);
    }

}

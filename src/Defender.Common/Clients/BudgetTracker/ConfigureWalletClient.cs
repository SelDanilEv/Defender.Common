using Defender.Common.Clients.BudgetTracker;
using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.BudgetTracker;

public static class ConfigureBudgetTrackerClient
{
    public static void RegisterBudgetTrackerClient(
        this IServiceCollection services, 
        Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IBudgetTrackerServiceClient, BudgetTrackerServiceClient>(
            nameof(BudgetTrackerServiceClient), configureClient);
    }

}

using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.UserManagement;

public static class ConfigureUserManagementClient
{
    public static void RegisterUserManagementClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<IUserManagementServiceClient, UserManagementServiceClient>(nameof(UserManagementServiceClient), configureClient);
    }

}

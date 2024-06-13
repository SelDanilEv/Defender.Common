using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Notification;

public static class ConfigureNotificationClient
{
    public static void RegisterNotificationClient(
        this IServiceCollection services, 
        Action<IServiceProvider, HttpClient> configureClient)
    {
        services.AddHttpClient<INotificationServiceClient, NotificationServiceClient>(
            nameof(NotificationServiceClient), configureClient);
    }
}

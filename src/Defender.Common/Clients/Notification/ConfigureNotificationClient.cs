using Microsoft.Extensions.DependencyInjection;

namespace Defender.Common.Clients.Notification
{
    public static class ConfigureNotificationClient
    {
        public static void RegisterNotificationAsServiceClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
        {
            services.AddHttpClient<INotificationAsServiceClient, NotificationAsServiceClient>(nameof(NotificationAsServiceClient), configureClient);
        }

        public static void RegisterNotificationClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient)
        {
            services.AddHttpClient<INotificationClient, NotificationAsServiceClient>(nameof(NotificationAsServiceClient), configureClient);
        }

    }
}

using Microsoft.Extensions.Hosting;

namespace Defender.Common.Exstension;

public static class CommonHostEnvironmentExtensions
{
    public static bool IsLocalOrDevelopment(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("Development") || hostEnvironment.IsEnvironment("Local");
    }
}
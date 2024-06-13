using Defender.Common.Enums;
using Microsoft.Extensions.Hosting;

namespace Defender.Common.Extension;

public static class CommonHostEnvironmentExtensions
{
    public static bool IsLocalOrDevelopment(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("Development")
            || hostEnvironment.IsEnvironment("Local")
            || hostEnvironment.IsEnvironment("DockerLocal")
            || hostEnvironment.IsEnvironment("DockerDev");
    }

    public static AppEnvironment GetAppEnvironment(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.EnvironmentName switch
        {
            "Production" or "DockerProd" => AppEnvironment.prod,
            "Development" or "DockerDev" => AppEnvironment.dev,
            "Local" or "DockerLocal" => AppEnvironment.local,
            _ => AppEnvironment.local,
        };
    }
}
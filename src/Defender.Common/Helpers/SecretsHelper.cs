using Defender.Common.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Defender.Common.Helpers;

public static class SecretsHelper
{
    private const string EnvironmentVariablePrefix = "Defender_App_";

    private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

    public static string GetSecret(Secret envVariable)
    {
        return GetSecretFromEnvVariables(envVariable.ToString());
    }

    public static string GetSecret(string envVariable)
    {
        return GetSecretFromEnvVariables(envVariable);
    }

    private static string GetSecretFromEnvVariables(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        key = MapEnvVariableToKey(key);

        if (cache.TryGetValue(key, out var cachedValue))
        {
            return (string)cachedValue;
        }

        var value =
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

        if (value == null)
        {
            return string.Empty;
        }

        cache.Set(key, value, DateTime.UtcNow.AddMinutes(5));

        return value;
    }

    private static string MapEnvVariableToKey(string envVariable) =>
        EnvironmentVariablePrefix + envVariable;

    private static string MapEnvVariableToKey(Secret envVariable) =>
        MapEnvVariableToKey(envVariable.ToString());
}

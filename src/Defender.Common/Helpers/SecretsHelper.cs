using Defender.Common.Enums;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Defender.Common.Helpers;

public static class SecretsHelper
{
    private const string EnvironmentVariablePrefix = "Defender_App_";
    private const int CacheExpirationMinutes = 5;

    private static readonly MemoryCache cache = new(new MemoryCacheOptions());

    private static IMongoSecretAccessor? _mongoSecretAccessor;

    public static void Initialize(IMongoSecretAccessor mongoSecretAccessor)
    {
        _mongoSecretAccessor = mongoSecretAccessor;
    }


    #region Sync
    public static string GetSecretSync(Secret envVariable, bool useMongo = false)
    {
        return GetSecretSync(envVariable.ToString(), useMongo);
    }

    public static string GetSecretSync(string envVariable, bool useMongo = false)
    {
        return GetSecretAsync(envVariable, useMongo).GetAwaiter().GetResult();
    }
    #endregion

    public static async Task<string> GetSecretAsync(Secret envVariable)
    {
        return await GetSecretAsync(envVariable.ToString());
    }

    public static async Task<string> GetSecretAsync(string key, bool useMongoSecrets = true)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        if (cache.TryGetValue(key, out var cachedValue))
        {
            return (string)(cachedValue ?? String.Empty);
        }

        var secret = GetSecretFromEnvVariables(key);

        if (string.IsNullOrWhiteSpace(secret) && useMongoSecrets)
        {
            secret = await GetSecretFromMongoSecrets(key);
        }

        if (!string.IsNullOrWhiteSpace(secret))
        {
            cache.Set(key, secret, DateTime.UtcNow.AddMinutes(CacheExpirationMinutes));
        }

        return secret;
    }

    private static async Task<string> GetSecretFromMongoSecrets(string key)
    {
        if (string.IsNullOrWhiteSpace(key) || _mongoSecretAccessor == null)
        {
            return string.Empty;
        }

        var secret = await _mongoSecretAccessor.GetSecretValueByNameAsync(key);

        return secret;
    }

    private static string GetSecretFromEnvVariables(string key)
    {
        var envVariableKey = MapEnvVariableToKey(key);

        var value =
            Environment.GetEnvironmentVariable(envVariableKey, EnvironmentVariableTarget.Process) ??
            Environment.GetEnvironmentVariable(envVariableKey, EnvironmentVariableTarget.User) ??
            Environment.GetEnvironmentVariable(envVariableKey, EnvironmentVariableTarget.Machine);

        if (value == null)
        {
            return string.Empty;
        }

        cache.Set(key, value, DateTime.UtcNow.AddMinutes(CacheExpirationMinutes));

        return value;
    }

    private static string MapEnvVariableToKey(string envVariable) =>
        EnvironmentVariablePrefix + envVariable;
}

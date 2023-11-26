using Defender.Common.Enums;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Defender.Common.Helpers;

public static class SecretsHelper
{
    private const string EnvironmentVariablePrefix = "Defender_App_";
    private const int CacheExpirationMintures = 5;

    private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

    private static IMongoSecretAccessor _mongoSecretAccessor;

    public static void Initialize(IMongoSecretAccessor mongoSecretAccessor)
    {
        _mongoSecretAccessor = mongoSecretAccessor;
    }

    public static async Task<string> GetSecretAsync(Secret envVariable)
    {
        return await GetSecretAsync(envVariable.ToString());
    }

    public static async Task<string> GetSecretAsync(string key)
    {
        var secret = GetSecretFromEnvVariables(key);

        if (string.IsNullOrWhiteSpace(secret))
        {
            secret = await GetSecretFromMongoSecrets(key);
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

        if (!string.IsNullOrWhiteSpace(secret))
        {
            cache.Set(key, secret, DateTime.UtcNow.AddMinutes(CacheExpirationMintures));
        }

        return secret;
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


    #region Sync - no mongo secrets
    [SuppressMessage("Usage", "Use this method only in contructors (only env variables secrets)")]
    public static string GetSecretSync(Secret envVariable)
    {
        return GetSecretFromEnvVariables(envVariable.ToString());
    }

    public static string GetSecretSync(string envVariable)
    {
        var secret = GetSecretFromEnvVariables(envVariable);

        return secret;
    }
    #endregion
}

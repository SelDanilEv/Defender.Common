using Defender.Common.Enums;

namespace Defender.Common.Helpers;

public static class SecretsHelper
{
    private const string EnvironmentVariablePrefix = "Defender_App_";

    private static readonly Dictionary<string, string> _environmentVariables =
        new Dictionary<string, string>();

    public static string GetSecret(Secret envVariable)
    {
        return GetSecret(MapEnvVariableToKey(envVariable));
    }

    public static string GetSecret(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        if (_environmentVariables.ContainsKey(key))
        {
            return _environmentVariables[key];
        }

        var value =
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ??
            Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

        if (value == null)
        {
            return string.Empty;
        }

        _environmentVariables.Add(key, value);

        return value;
    }

    private static string MapEnvVariableToKey(Secret envVariable) =>
        EnvironmentVariablePrefix + envVariable.ToString();
}

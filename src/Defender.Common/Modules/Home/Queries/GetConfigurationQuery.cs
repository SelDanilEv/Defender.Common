using System.Collections;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using MediatR;

namespace Defender.Common.Modules.Home.Queries;

public record GetConfigurationQuery : IRequest<Dictionary<string, string>>
{
    public ConfigurationLevel Level { get; set; } = ConfigurationLevel.All;
};

public class GetConfigurationQueryHandler : IRequestHandler<GetConfigurationQuery, Dictionary<string, string>>
{
    public async Task<Dictionary<string, string>> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, string>();

        switch (request.Level)
        {
            case ConfigurationLevel.All:
                var allProcessEnvVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
                foreach (DictionaryEntry envVariable in allProcessEnvVariables)
                {
                    var (key, value) = (envVariable.Key.ToString(), envVariable.Value?.ToString() ?? String.Empty);
                    if (key != null && value != null)
                        result.TryAdd(key, value);
                }

                var allUserEnvVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
                foreach (DictionaryEntry envVariable in allUserEnvVariables)
                {
                    var (key, value) = (envVariable.Key.ToString(), envVariable.Value?.ToString() ?? String.Empty);
                    if (key != null && value != null)
                        result.TryAdd(key, value);
                }

                var allMachineEnvVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
                foreach (DictionaryEntry envVariable in allMachineEnvVariables)
                {
                    var (key, value) = (envVariable.Key.ToString(), envVariable.Value?.ToString() ?? String.Empty);
                    if (key != null && value != null)
                        result.TryAdd(key, value);
                }
                break;
            case ConfigurationLevel.Admin:
                foreach (Secret secret
                    in (Secret[])Enum.GetValues(typeof(Secret)))
                {
                    result.Add(secret.ToString(), await SecretsHelper.GetSecretAsync(secret));
                }
                break;
        }

        return result;
    }

}

using Defender.Common.Entities.Secrets;

namespace Defender.Common.Interfaces;

public interface IMongoSecretAccessor
{
    Task<string> GetSecretValueByNameAsync(string secretName);
    Task<MongoSecret> GetSecretByNameAsync(string secretName);
}

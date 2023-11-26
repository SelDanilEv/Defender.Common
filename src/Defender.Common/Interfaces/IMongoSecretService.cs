using Defender.Common.Entities.Secrets;

namespace Defender.Common.Interfaces;

public interface IMongoSecretService : IMongoSecretAccessor
{
    public Task<MongoSecret> CreateOrUpdateSecretAsync(string secretName, string value);

}

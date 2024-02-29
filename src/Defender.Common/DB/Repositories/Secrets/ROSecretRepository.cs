using Defender.Common.Configuration.Options;
using Defender.Common.Contst;
using Defender.Common.DB.Model;
using Defender.Common.DB.Repositories;
using Defender.Common.Entities.Secrets;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Defender.Common.DB.Repositories.Secrets;

internal class ROSecretRepository : BaseMongoRepository<MongoSecret>, IMongoSecretAccessor
{
    public ROSecretRepository(IOptions<MongoDbOptions> mongoOption)
        : base(new MongoDbOptions(
            ConstValues.SecretManagementServiceMongoDBName, mongoOption?.Value!))
    {
    }

    public async Task<string> GetSecretValueByNameAsync(string secretName)
    {
        var secret = await GetSecretByNameAsync(secretName);

        if (secret == null)
        {
            return string.Empty;
        }

        return await CryptographyHelper.DecryptStringAsync(secret?.Value!, secretName);
    }

    public async Task<MongoSecret> GetSecretByNameAsync(string secretName)
    {
        var findModelRequest = FindModelRequest<MongoSecret>.Init(x => x.SecretName, secretName);

        return await GetItemAsync(findModelRequest);
    }
}

using Defender.Common.Configuration.Options;
using Defender.Common.Consts;
using Defender.Common.DB.Model;
using Defender.Common.Entities.Secrets;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Defender.Common.DB.Repositories.Secrets;

internal class ROSecretRepository(
    IOptions<MongoDbOptions> mongoOption)
    : BaseMongoRepository<MongoSecret>(
        new MongoDbOptions(
            ConstValues.SecretManagementServiceMongoDBName, 
            mongoOption?.Value!)), 
    IMongoSecretAccessor
{
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

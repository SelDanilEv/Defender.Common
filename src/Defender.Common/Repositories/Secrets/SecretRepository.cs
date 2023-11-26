using Defender.Common.Configuration.Options;
using Defender.Common.Contst;
using Defender.Common.Entities.Secrets;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.Common.Models;
using Microsoft.Extensions.Options;

namespace Defender.Common.Repositories.Secrets;

internal class SecretRepository : MongoRepository<MongoSecret>, IMongoSecretAccessor
{
    public SecretRepository(IOptions<MongoDbOptions> mongoOption)
        : base(new MongoDbOptions(ConstValues.SecretManagementServiceMongoDBName, mongoOption?.Value))
    {
    }

    public async Task<MongoSecret> CreateOrUpdateSecretAsync(string secretName, string value)
    {
        value = await CryptographyHelper.EncryptStringAsync(value, secretName);

        var existingSecret = await GetSecretByNameAsync(secretName);

        if (existingSecret == null)
        {
            var secret = MongoSecret.FromSecretName(secretName, value);

            return await AddItemAsync(secret);
        }

        var updateRequest = UpdateModelRequest<MongoSecret>
            .Init(existingSecret)
            .UpdateField(x=> x.Value, value);

        return await UpdateItemAsync(updateRequest);
    }

    public async Task<string> GetSecretValueByNameAsync(string secretName)
    {
        var secret = await GetSecretByNameAsync(secretName);

        if (secret == null)
        {
            return String.Empty;
        }

        return await CryptographyHelper.DecryptStringAsync(secret?.Value, secretName);
    }

    public async Task<MongoSecret> GetSecretByNameAsync(string secretName)
    {
        var findModelRequest = FindModelRequest<MongoSecret>.Init(x => x.SecretName, secretName);

        return await GetItemAsync(findModelRequest);
    }
}

﻿using Defender.Common.Configuration.Options;
using Defender.Common.Contst;
using Defender.Common.Entities.Secrets;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.Common.Models;
using Microsoft.Extensions.Options;

namespace Defender.Common.Repositories.Secrets;

public class SecretsRepository : MongoRepository<MongoSecret>, IMongoSecretAccessor, IMongoSecretService
{
    public SecretsRepository(IOptions<MongoDbOptions> mongoOption)
        : base(new MongoDbOptions(ConstValues.SecretManagementServiceMongoDBName, mongoOption?.Value))
    {
    }

    public async Task<MongoSecret> CreateOrUpdateSecretAsync(string secretName, string value)
    {
        value = await CryptographyHelper.EncryptString(value, secretName);

        var existingSecret = await GetMongoSecretByName(secretName);

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

    public async Task<string> GetSecretValueAsync(string secretName)
    {
        var secret = await GetMongoSecretByName(secretName);

        if (secret == null)
        {
            return String.Empty;
        }

        return await CryptographyHelper.DecryptString(secret?.Value, secretName);
    }

    private async Task<MongoSecret> GetMongoSecretByName(string secretName)
    {
        var findModelRequest = FindModelRequest<MongoSecret>.Init(x => x.SecretName, secretName);

        return await GetItemAsync(findModelRequest);
    }
}

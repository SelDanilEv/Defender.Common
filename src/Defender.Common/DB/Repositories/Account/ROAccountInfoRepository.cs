using Defender.Common.Configuration.Options;
using Defender.Common.Consts;
using Defender.Common.Entities.AccountInfo;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Defender.Common.DB.Repositories.Account;

internal class ROAccountInfoRepository(
    IOptions<MongoDbOptions> mongoOption)
    : BaseMongoRepository<BaseAccountInfo>(
        new MongoDbOptions(
            ConstValues.IdentityServiceMongoDBName,
            mongoOption?.Value!),
            typeof(BaseAccountInfo).Name.Replace("Base", "")),
    IAccountAccessor
{

    public async Task<BaseAccountInfo> GetAccountInfoById(Guid accountId)
    {
        return await GetItemAsync(accountId);
    }
}

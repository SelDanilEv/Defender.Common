using Defender.Common.Configuration.Options;
using Defender.Common.Contst;
using Defender.Common.Entities.AccountInfo;
using Defender.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Defender.Common.DB.Repositories.Account;

internal class ROAccountInfoRepository : BaseMongoRepository<BaseAccountInfo>, IAccountAccessor
{
    public ROAccountInfoRepository(IOptions<MongoDbOptions> mongoOption)
        : base(new MongoDbOptions(
            ConstValues.IdentityServiceMongoDBName, mongoOption?.Value!))
    {
    }

    public async Task<BaseAccountInfo> GetAccountInfoById(Guid accountId)
    {
        return await GetItemAsync(accountId);
    }
}

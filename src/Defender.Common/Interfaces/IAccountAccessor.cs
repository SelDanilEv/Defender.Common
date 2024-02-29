using Defender.Common.Entities.AccountInfo;

namespace Defender.Common.Interfaces;

public interface IAccountAccessor
{
    Task<BaseAccountInfo> GetAccountInfoById(Guid accountId);
}

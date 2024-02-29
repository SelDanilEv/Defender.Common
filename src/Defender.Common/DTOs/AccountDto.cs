using Defender.Common.Entities.AccountInfo;

namespace Defender.Common.DTOs;

public record AccountDto : BaseAccountInfo
{
    public bool IsPhoneVerified { get; set; }

    public bool IsEmailVerified { get; set; }

    public bool IsBlocked { get; set; }
}

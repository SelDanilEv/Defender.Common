using Defender.Common.DTOs;

namespace Defender.Common.Interfaces;

public interface IAccountAccessor
{
    AccountDto? AccountInfo { get; }
    string Token { get; }
}

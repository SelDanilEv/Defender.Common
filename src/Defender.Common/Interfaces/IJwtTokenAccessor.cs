using Defender.Common.DTOs;

namespace Defender.Common.Interfaces;

public interface IJwtTokenAccessor
{
    AccountDto? JwtInfo { get; }
    string Token { get; }
}

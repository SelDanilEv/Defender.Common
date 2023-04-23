using Defender.Common.Entities;

namespace Defender.Common.Interfaces;

public interface IJwtTokenAccessor
{
    AccountInfo? JwtInfo { get; }
    string Token { get; }
}

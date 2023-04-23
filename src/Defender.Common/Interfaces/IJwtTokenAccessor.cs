using Defender.Common.Entities.User;

namespace Defender.Common.Interfaces;

public interface IJwtTokenAccessor
{
    AccountInfo? JwtInfo { get; }
    string Token { get; }
}

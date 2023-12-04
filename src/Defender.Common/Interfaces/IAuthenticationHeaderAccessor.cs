using Defender.Common.Wrapper.Internal;
using System.Net.Http.Headers;

namespace Defender.Common.Interfaces;

public interface IAuthenticationHeaderAccessor
{
    Task<AuthenticationHeaderValue> GetAuthenticationHeader(AuthorizationType authorizationType);
}

using System.Net.Http.Headers;

namespace Defender.Common.Clients.Identity;

internal partial class IdentityServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}

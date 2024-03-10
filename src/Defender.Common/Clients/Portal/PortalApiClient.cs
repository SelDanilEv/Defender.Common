using System.Net.Http.Headers;

namespace Defender.Common.Clients.Portal;

internal partial class PortalApiClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
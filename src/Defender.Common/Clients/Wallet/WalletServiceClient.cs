using System.Net.Http.Headers;

namespace Defender.Common.Clients.Wallet;

internal partial class WalletServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
using System.Net.Http.Headers;

namespace Defender.Common.Clients.RiskGames;

internal partial class RiskGamesServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
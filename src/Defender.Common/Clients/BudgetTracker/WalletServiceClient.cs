using System.Net.Http.Headers;

namespace Defender.Common.Clients.BudgetTracker;

internal partial class BudgetTrackerServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
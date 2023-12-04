using System.Net.Http.Headers;

namespace Defender.Common.Clients.Notification;

internal partial class NotificationServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
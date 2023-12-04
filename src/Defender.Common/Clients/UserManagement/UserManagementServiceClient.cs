using System.Net.Http.Headers;

namespace Defender.Common.Clients.UserManagement;

internal partial class UserManagementServiceClient
{
    public void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader)
    {
        _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
    }
}
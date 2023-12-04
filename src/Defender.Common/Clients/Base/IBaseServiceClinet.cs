using System.Net.Http.Headers;

namespace Defender.Common.Clients.Base
{
    public partial interface IBaseServiceClient
    {
        void SetAuthorizationHeader(AuthenticationHeaderValue authenticationHeader);
    }
}
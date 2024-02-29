using Defender.Common.Clients.Base;
using Defender.Common.Interfaces;

namespace Defender.Common.Wrapper.Internal
{
    public abstract class BaseInternalSwaggerWrapper : BaseSwaggerWrapper
    {
        private readonly IBaseServiceClient _client;
        private readonly IAuthenticationHeaderAccessor _authenticationHeaderAccessor;

        public BaseInternalSwaggerWrapper(
            IBaseServiceClient client,
            IAuthenticationHeaderAccessor authenticationHeaderAccessor
            )
        {
            _client = client;
            _authenticationHeaderAccessor = authenticationHeaderAccessor;
        }

        protected async Task<Result> ExecuteSafelyAsync<Result>(
            Func<Task<Result>> action,
            AuthorizationType authorizationType = 
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            return await base.ExecuteSafelyAsync(action);
        }

        protected async Task ExecuteSafelyAsync(
            Func<Task> action,
            AuthorizationType authorizationType = 
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            await base.ExecuteSafelyAsync(action);
        }

        private async Task SetAuthorizationHeaderAsync(
            AuthorizationType authorizationType)
        {
            if (authorizationType == AuthorizationType.WithoutAuthorization) 
                return;

            var headerValue = await _authenticationHeaderAccessor
                .GetAuthenticationHeader(authorizationType);

            _client.SetAuthorizationHeader(headerValue);
        }
    }
}

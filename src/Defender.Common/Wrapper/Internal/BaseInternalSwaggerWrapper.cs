using Defender.Common.Clients.Base;
using Defender.Common.Interfaces;

namespace Defender.Common.Wrapper.Internal
{
    public abstract class BaseInternalSwaggerWrapper(
        IBaseServiceClient client,
        IAuthenticationHeaderAccessor authenticationHeaderAccessor
            ) : BaseSwaggerWrapper
    {
        protected async Task<Result> ExecuteSafelyAsync<Result>(
            Func<Task<Result>> action,
            AuthorizationType authorizationType =
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            return await base.ExecuteSafelyAsync(action);
        }

        protected async Task<Result> ExecuteUnsafelyAsync<Result>(
            Func<Task<Result>> action,
            AuthorizationType authorizationType =
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            return await base.ExecuteUnsafelyAsync(action);
        }

        protected async Task ExecuteSafelyAsync(
            Func<Task> action,
            AuthorizationType authorizationType =
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            await base.ExecuteSafelyAsync(action);
        }

        protected async Task ExecuteUnsafelyAsync(
            Func<Task> action,
            AuthorizationType authorizationType =
                AuthorizationType.WithoutAuthorization)
        {
            await SetAuthorizationHeaderAsync(authorizationType);

            await base.ExecuteUnsafelyAsync(action);
        }

        private async Task SetAuthorizationHeaderAsync(
            AuthorizationType authorizationType)
        {
            if (authorizationType == AuthorizationType.WithoutAuthorization)
                return;

            var headerValue = await authenticationHeaderAccessor
                .GetAuthenticationHeader(authorizationType);

            client.SetAuthorizationHeader(headerValue);
        }
    }
}

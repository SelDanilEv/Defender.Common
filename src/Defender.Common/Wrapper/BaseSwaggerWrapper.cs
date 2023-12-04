using Defender.Common.Errors;
using Defender.Common.Exceptions;

namespace Defender.Common.Wrapper
{
    public abstract class BaseSwaggerWrapper
    {
        protected virtual async Task<Result> ExecuteSafelyAsync<Result>(Func<Task<Result>> action)
        {
            try
            {
                return await action();
            }
            catch (ApiException ex)
            {
                throw ex.ToServiceException();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ErrorCode.UnhandledError, ex);
            }
        }

        protected virtual async Task ExecuteSafelyAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (ApiException ex)
            {
                throw ex.ToServiceException();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ErrorCode.UnhandledError, ex);
            }
        }
    }
}

using Defender.Common.Errors;
using Defender.Common.Exceptions;

namespace Defender.Common.Wrapper
{
    public class BaseSwaggerWrapper
    {
        protected Result ExecuteSafely<Result>(Func<Result> action)
        {
            try
            {
                return action();
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

        protected async Task<Result> ExecuteSafelyAsync<Result>(Func<Task<Result>> action)
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
    }
}

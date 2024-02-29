using Defender.Common.Errors;

namespace Defender.Common.Interfaces;

public interface IAuthorizationCheckingService
{
    Task<T> RunWithAuthAsync<T>(
        Guid targetAccountId,
        Func<Task<T>> action,
        ErrorCode changingSuperAdminError = ErrorCode.CM_ForbiddenAccess,
        ErrorCode changingAdminError = ErrorCode.CM_ForbiddenAccess
        );
}

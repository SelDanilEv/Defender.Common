using Defender.Common.Errors;

namespace Defender.Common.Interfaces;

public interface IAuthorizationCheckingService
{
    Task<T> ExecuteWithAuthCheckAsync<T>(
        Guid targetAccountId,
        Func<Task<T>> action,
        bool requireSuperAdmin = false,
        ErrorCode errorCode = ErrorCode.CM_ForbiddenAccess);

    Task<T> ExecuteBasedOnUserRoleAsync<T>(
        Func<Task<T>>? superAdminAction = null,
        Func<Task<T>>? adminAction = null,
        Func<Task<T>>? userAction = null);
}

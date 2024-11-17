using Defender.Common.Enums;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Interfaces;

namespace Defender.Common.Service;

internal class AuthorizationCheckingService(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccountAccessor accountAccessor)
    : IAuthorizationCheckingService
{

    public async Task<T> ExecuteWithAuthCheckAsync<T>(
        Guid targetAccountId,
        Func<Task<T>> action,
        bool requireSuperAdmin = false,
        ErrorCode errorCode = ErrorCode.CM_ForbiddenAccess)
    {
        var targetAccount = await accountAccessor.GetAccountInfoById(targetAccountId);

        if (targetAccount == null)
        {
            throw new ServiceException(ErrorCode.CM_NotFound);
        }

        var currentUserId = currentAccountAccessor.GetAccountId();
        var currentUserRole = currentAccountAccessor.GetHighestRole();

        if (targetAccount.Id == currentUserId
            || currentUserRole == Role.SuperAdmin
            || currentUserRole == Role.Admin
                && !(requireSuperAdmin || targetAccount.IsAdmin))
        {
            return await action();
        }

        throw new ForbiddenAccessException(errorCode);
    }

    public async Task<T> ExecuteBasedOnUserRoleAsync<T>(
        Func<Task<T>>? superAdminAction = null,
        Func<Task<T>>? adminAction = null,
        Func<Task<T>>? userAction = null)
    {
        var currentUserRole = currentAccountAccessor.GetHighestRole();

        switch (currentUserRole)
        {
            case Role.SuperAdmin:
                if (superAdminAction != null)
                {
                    return await superAdminAction();
                }
                break;
            case Role.Admin:
                if (adminAction != null)
                {
                    return await adminAction();
                }
                break;
            case Role.User:
                if (userAction != null)
                {
                    return await userAction();
                }
                break;
        }

        throw new ServiceException(ErrorCode.CM_ForbiddenAccess);
    }

}

using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;

namespace Defender.Common.Service;

internal class AuthorizationCheckingService : IAuthorizationCheckingService
{
    private readonly ICurrentAccountAccessor _currentAccountAccessor;
    private readonly IAccountAccessor _accountAccessor;

    public AuthorizationCheckingService(
        ICurrentAccountAccessor currentAccountAccessor,
        IAccountAccessor accountAccessor)
    {
        _currentAccountAccessor = currentAccountAccessor;
        _accountAccessor = accountAccessor;
    }

    public async Task<T> RunWithAuthAsync<T>(
        Guid targetAccountId,
        Func<Task<T>> action,
        ErrorCode changingSuperAdminError = ErrorCode.CM_ForbiddenAccess,
        ErrorCode changingAdminError = ErrorCode.CM_ForbiddenAccess
        )
    {
        var targetAccount = await _accountAccessor
            .GetAccountInfoById(targetAccountId);

        if (targetAccount == null)
        {
            throw new ServiceException(ErrorCode.CM_NotFound);
        }

        var currentUserId = _currentAccountAccessor.GetAccountId();
        var currentUserRoles = _currentAccountAccessor.GetRoles();

        if (targetAccount.Id != currentUserId)
        {
            if (RolesHelper.IsSuperAdmin(currentUserRoles) && targetAccount.IsSuperAdmin)
            {
                throw new ForbiddenAccessException(changingSuperAdminError);
            }
            else if (RolesHelper.IsAdmin(currentUserRoles) && targetAccount.IsAdmin)
            {
                throw new ForbiddenAccessException(changingAdminError);
            }
            else
            {
                throw new ForbiddenAccessException();
            }
        }

        return await action();
    }


}

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

        var currentUserRoles = _currentAccountAccessor.GetRoles();

        if (RolesHelper.IsSuperAdmin(currentUserRoles))
        {
            if (targetAccount.IsSuperAdmin)
            {
                throw new ForbiddenAccessException(changingSuperAdminError);
            }
        }
        else if (RolesHelper.IsAdmin(currentUserRoles))
        {
            if (targetAccount.IsAdmin)
            {
                throw new ForbiddenAccessException(changingAdminError);
            }
        }
        else
        {
            var currentUserId = _currentAccountAccessor
                .GetAccountId();

            if (targetAccount.Id != currentUserId)
            {
                throw new ForbiddenAccessException();
            }
        }

        return await action();
    }


}

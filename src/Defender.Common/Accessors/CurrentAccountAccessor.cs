using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Defender.Common.Interfaces;
using Defender.Common.Exceptions;
using Defender.Common.Errors;
using Defender.Common.Helpers;

namespace Defender.Common.Accessors;

public class CurrentAccountAccessor(IHttpContextAccessor httpContextAccessor)
    : ICurrentAccountAccessor
{
    public Guid GetAccountId()
    {
        var currentUserClaims = httpContextAccessor.HttpContext?.User.Claims;

        if (currentUserClaims == null || !Guid.TryParse(
                currentUserClaims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                .Value,
            out var userId))
        {
            throw new ServiceException(ErrorCode.CM_InvalidUserJWT);
        }

        return userId;
    }

    public List<string> GetRoles()
    {
        var currentUserClaims = httpContextAccessor.HttpContext?.User.Claims;

        if(currentUserClaims == null)
        {
            return new List<string>();
        }

        return currentUserClaims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
    }

    public string GetHighestRole()
    {
        return RolesHelper.GetHighestRole(GetRoles());
    }

    public string? Token
    {
        get
        {
            return httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization;
        }
    }
}

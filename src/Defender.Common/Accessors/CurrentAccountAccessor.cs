using System.Security.Claims;
using Defender.Common.Enums;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using CustomClaimTypes = Defender.Common.Consts.ClaimTypes;

namespace Defender.Common.Accessors;

public class CurrentAccountAccessor(IHttpContextAccessor httpContextAccessor)
    : ICurrentAccountAccessor
{
    public Guid GetAccountId()
    {
        var currentUserClaims = httpContextAccessor.HttpContext?.User.Claims;

        if (
            currentUserClaims == null
            || !Guid.TryParse(
                currentUserClaims.FirstOrDefault(x => x.Type == CustomClaimTypes.NameIdentifier)?.Value,
                out var userId
            )
        )
        {
            throw new ServiceException(ErrorCode.CM_InvalidUserJWT);
        }

        return userId;
    }

    public List<string> GetRoles()
    {
        var currentUserClaims = httpContextAccessor.HttpContext?.User.Claims;

        if (currentUserClaims == null)
        {
            return [];
        }

        return currentUserClaims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();
    }

    public Role GetHighestRole()
    {
        return RolesHelper.GetHighestRole(GetRoles());
    }

    public bool HasRole(Role role)
    {
        return RolesHelper.HasRole(GetRoles(), role);
    }

    public string? Token
    {
        get { return httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization; }
    }
}

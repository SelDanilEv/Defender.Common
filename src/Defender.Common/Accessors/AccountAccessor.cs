using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Defender.Common.Interfaces;
using Defender.Common.DTOs;

namespace Defender.Common.Accessors;

public class AccountAccessor : IAccountAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AccountDto? AccountInfo
    {
        get
        {
            var currentUserClaims = _httpContextAccessor.HttpContext?.User.Claims;

            var account = new AccountDto();

#pragma warning disable CS8604 // Possible null reference argument.
            if (Guid.TryParse(
                currentUserClaims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var guid))
            {
                account.Id = guid;
            }
#pragma warning restore CS8604 // Possible null reference argument.

            account.Roles = currentUserClaims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return account;
        }
    }

    public string Token
    {
        get
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}

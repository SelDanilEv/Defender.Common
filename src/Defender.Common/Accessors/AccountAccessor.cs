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

            if (Guid.TryParse(
                currentUserClaims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var guid))
            {
                account.Id = guid;
            }

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
            return _httpContextAccessor?.HttpContext?.Request?.Headers?.Authorization;
        }
    }
}

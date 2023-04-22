using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Defender.Common.Entities.User;
using Defender.Common.Interfaces;

namespace Defender.Common.Accessors;

public class JwtTokenAccessor : IJwtTokenAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public JwtInfo? JwtInfo
    {
        get
        {
            var currentUserClaims = _httpContextAccessor.HttpContext?.User.Claims;

            var jwt = new JwtInfo();

            if (Guid.TryParse(
                currentUserClaims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                out var guid))
            {
                jwt.Id = guid;
            }

            jwt.Roles = currentUserClaims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return jwt;
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

using Microsoft.AspNetCore.Authorization;

namespace Defender.Common.Attributes;

public class AuthAttribute : AuthorizeAttribute
{
    public AuthAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}


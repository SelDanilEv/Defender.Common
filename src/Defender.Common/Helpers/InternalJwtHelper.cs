﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Defender.Common.Consts;
using Defender.Common.Enums;
using Microsoft.IdentityModel.Tokens;
using ClaimTypes = Defender.Common.Consts.ClaimTypes;

namespace Defender.Common.Helpers;

public static class InternalJwtHelper
{
    public static async Task<string> GenerateInternalJWTAsync(string issuer, int expiresMinutes = 1)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
        };

        claims.AddRange(Roles.Any.Split(',').Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(await SecretsHelper.GetSecretAsync(Secret.JwtSecret))
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            null,
            claims,
            expires: DateTime.Now.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

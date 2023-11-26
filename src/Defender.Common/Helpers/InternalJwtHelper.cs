using Defender.Common.Enums;
using Defender.Common.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Defender.Common.Helpers;

public static class InternalJwtHelper
{
    public static async Task<string> GenerateInternalJWT(string issuer)
    {
        var claims = new List<Claim>{
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        Guid.Empty.ToString())};

        foreach (var role in Roles.Any.Split(','))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            await SecretsHelper.GetSecretAsync(Secret.JwtSecret)));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer,
          null,
          claims,
          expires: DateTime.Now.AddMinutes(1),
          signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

﻿using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using Defender.Common.Wrapper.Internal;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Defender.Common.Accessors;

public class AuthenticationHeaderAccessor : IAuthenticationHeaderAccessor
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentAccountAccessor _accountAccessor;

    public AuthenticationHeaderAccessor(
        IConfiguration configuration,
        ICurrentAccountAccessor accountAccessor
        )
    {
        _configuration = configuration;
        _accountAccessor = accountAccessor;
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeader(AuthorizationType authorizationType)
    {
        if (authorizationType == AuthorizationType.WithoutAuthorization) return DefaultAuthenticationHeader;

        var schemaAndToken = _accountAccessor.Token?.Split(' ');

#pragma warning disable CS8604 // Possible null reference argument.
        var headerValue = authorizationType switch
        {
            AuthorizationType.Service => new AuthenticationHeaderValue(
                "Bearer",
                await InternalJwtHelper.GenerateInternalJWTAsync(_configuration["JwtTokenIssuer"])),
            AuthorizationType.User => schemaAndToken?.Length == 2
                ? new AuthenticationHeaderValue(schemaAndToken[0], schemaAndToken[1])
                : DefaultAuthenticationHeader,
            _ => DefaultAuthenticationHeader
        };
#pragma warning restore CS8604 // Possible null reference argument.

        return headerValue;
    }

    private AuthenticationHeaderValue DefaultAuthenticationHeader => new AuthenticationHeaderValue("Bearer");
}

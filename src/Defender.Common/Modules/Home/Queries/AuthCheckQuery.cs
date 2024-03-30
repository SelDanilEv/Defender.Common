using Defender.Common.DTOs;
using Defender.Common.Helpers;
using Defender.Common.Interfaces;
using MediatR;

namespace Defender.Common.Modules.Home.Queries;

public record AuthCheckQuery : IRequest<AuthCheckDto>;

public class AuthCheckQueryHandler(
    ICurrentAccountAccessor currentAccountAccessor
    ) : IRequestHandler<AuthCheckQuery, AuthCheckDto>
{
    public Task<AuthCheckDto> Handle(AuthCheckQuery request, CancellationToken cancellationToken)
    {
        var userId = currentAccountAccessor.GetAccountId();
        var userRoles = currentAccountAccessor.GetRoles();

        return Task.FromResult(new AuthCheckDto(userId, RolesHelper.GetHighestRole(userRoles)));
    }

}

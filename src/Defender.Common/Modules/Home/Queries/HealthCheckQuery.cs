using Defender.Common.DTOs;
using MediatR;

namespace Defender.Common.Modules.Home.Queries;

public record HealthCheckQuery : IRequest<HealthCheckDto>;

public class HealthCheckQueryHandler(
    ) : IRequestHandler<HealthCheckQuery, HealthCheckDto>
{
    public Task<HealthCheckDto> Handle(
        HealthCheckQuery request, 
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new HealthCheckDto("Health"));
    }

}

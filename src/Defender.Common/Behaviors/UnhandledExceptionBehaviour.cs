using MediatR;
using Microsoft.Extensions.Logging;

namespace Defender.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(
        ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, $"Unhandled Exception for Request {requestName}");

            throw;
        }
    }
}

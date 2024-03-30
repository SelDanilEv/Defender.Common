using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Defender.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(
        ILogger<LoggingBehaviour<TRequest, TResponse>> logger) : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Sending a request {typeof(TRequest).Name}");

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        logger.LogInformation(
            $"Response {typeof(TResponse).Name} has been got. Elapsed time = {stopwatch.ElapsedMilliseconds} ms");

        return response;
    }
}

using Defender.Common.Errors;
using FluentValidation;
using MediatR;

namespace Defender.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                var errorMessage = failures.Count > 0
                    ? failures.FirstOrDefault()?.ErrorMessage
                        ?? ErrorCodeHelper.GetErrorCode(ErrorCode.Unknown)
                    : ErrorCodeHelper.GetErrorCode(ErrorCode.Unknown);

                throw new Exceptions.ValidationException(errorMessage, failures);
            }
        }
        return await next();
    }
}

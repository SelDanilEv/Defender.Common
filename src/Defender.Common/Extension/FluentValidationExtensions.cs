using Defender.Common.Errors;
using FluentValidation;

namespace Defender.Common.Extension;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithMessage<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        ErrorCode code = ErrorCode.Unknown)
    {
        return rule.WithMessage(ErrorCodeHelper.GetErrorCode(code));
    }
}
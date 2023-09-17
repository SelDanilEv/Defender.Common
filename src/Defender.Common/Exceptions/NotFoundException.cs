using Defender.Common.Errors;

namespace Defender.Common.Exceptions;

public class NotFoundException : ServiceException
{
    public NotFoundException()
        : base(ErrorCode.UnhandledError)
    {
    }

    public NotFoundException(ErrorCode errorCode) :
    base(ErrorCodeHelper.GetErrorCode(errorCode))
    {
    }
}

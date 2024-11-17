using Defender.Common.Errors;

namespace Defender.Common.Exceptions;


public class ForbiddenAccessException : ServiceException
{
    public ForbiddenAccessException() :
        base(ErrorCodeHelper.GetErrorCode(ErrorCode.CM_ForbiddenAccess))
    {
    }

    public ForbiddenAccessException(ErrorCode errorCode) :
        base(ErrorCodeHelper.GetErrorCode(errorCode))
    {
    }
}

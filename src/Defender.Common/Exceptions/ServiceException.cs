using Defender.Common.Errors;
using System.Runtime.Serialization;

namespace Defender.Common.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(string? message) : base(message)
        {
        }

        public ServiceException(ErrorCode message) : base(ErrorCodeHelper.GetErrorCode(message))
        {
        }

        public ServiceException(ErrorCode message, Exception? innerException) : base(ErrorCodeHelper.GetErrorCode(message), innerException)
        {
        }

        public ServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

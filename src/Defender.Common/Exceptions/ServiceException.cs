using System.Runtime.Serialization;

namespace Defender.Common.Exceptions
{
    public class ServiceException : Exception
    {
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public ServiceException()
        {
        }

        public ServiceException(Dictionary<string, string> properties)
        {
            Properties = properties;
        }

        public ServiceException(string? message) : base(message)
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

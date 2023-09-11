namespace Defender.Common.Errors
{
    public class ErrorCodeHelper
    {
        public static string GetErrorCode(ErrorCode status) =>
                status.ToString(); 
        
        public static string GetErrorCode(string status) =>
         GetErrorCode(ToErrorCode(status));

        public static ErrorCode ToErrorCode(string status) =>
            Enum.IsDefined(typeof(ErrorCode), status) ?
            (ErrorCode)Enum.Parse(typeof(ErrorCode), status, false) :
            ErrorCode.UnhandledError;
    }
}

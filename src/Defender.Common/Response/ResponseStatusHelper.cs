namespace Defender.Common.Response
{
    public class ResponseStatusHelper
    {
        public static string ToString(ResponseStatus status) =>
            status.ToString();
        public static ResponseStatus ToStatus(string status) =>
            Enum.IsDefined(typeof(ResponseStatus), status) ?
            (ResponseStatus)Enum.Parse(typeof(ResponseStatus), status, false) :
            ResponseStatus.Unknown;
    }
}

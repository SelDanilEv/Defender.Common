using Defender.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Defender.Common.Exceptions;

public partial class ApiException(
    string message, 
    int statusCode, 
    string response, 
    IReadOnlyDictionary<string, IEnumerable<string>> headers,
    Exception innerException) 
    : ServiceException(
        message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
{
    public int StatusCode { get; private set; } = statusCode;

    public string? Response { get; private set; } = response;

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; } = headers;

    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }

    public ServiceException ToServiceException()
    {
        if (string.IsNullOrEmpty(Response))
        {
            return new ServiceException(ErrorCode.Unknown);
        }

        var details = Newtonsoft.Json.JsonConvert.DeserializeObject<ProblemDetails>(Response);

        if (details == null || string.IsNullOrEmpty(details?.Detail))
        {
            return new ServiceException(ErrorCode.Unknown);
        }

        return new ServiceException(ErrorCodeHelper.GetErrorCode(details.Detail));
    }
}

public partial class ApiException<TResult>(
    string message, 
    int statusCode,
    string response, 
    IReadOnlyDictionary<string, IEnumerable<string>> headers, 
    TResult result, Exception innerException) 
    : ApiException(message, statusCode, response, headers, innerException)
{
    public TResult Result { get; private set; } = result;
}

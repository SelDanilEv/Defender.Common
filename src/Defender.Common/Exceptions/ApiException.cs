using Defender.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Defender.Common.Exceptions;

public partial class ApiException : ServiceException
{
    public int StatusCode { get; private set; }

    public string Response { get; private set; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
    {
        StatusCode = statusCode;
#pragma warning disable CS8601 // Possible null reference assignment.
        Response = response;
#pragma warning restore CS8601 // Possible null reference assignment.
        Headers = headers;
    }

    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }

    public ServiceException ToServiceException()
    {
        var details = Newtonsoft.Json.JsonConvert.DeserializeObject<ProblemDetails>(Response);
#pragma warning disable CS8604 // Possible null reference argument.
        return new ServiceException(ErrorCodeHelper.GetErrorCode(details?.Detail));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}

public partial class ApiException<TResult> : ApiException
{
    public TResult Result { get; private set; }

    public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, System.Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}

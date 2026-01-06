namespace MojiWeather.Sdk.Exceptions;

/// <summary>
/// API请求异常
/// </summary>
public class ApiException : MojiWeatherException
{
    /// <summary>
    /// HTTP状态码
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// API返回的错误码
    /// </summary>
    public int? ErrorCode { get; }

    public ApiException(string message, int? statusCode = null, int? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public ApiException(string message, Exception innerException, int? statusCode = null, int? errorCode = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

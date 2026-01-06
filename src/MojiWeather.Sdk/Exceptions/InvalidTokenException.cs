namespace MojiWeather.Sdk.Exceptions;

/// <summary>
/// 无效Token异常
/// </summary>
public class InvalidTokenException : MojiWeatherException
{
    public InvalidTokenException()
        : base("The API token is invalid or has expired.") { }

    public InvalidTokenException(string message) : base(message) { }

    public InvalidTokenException(string message, Exception innerException)
        : base(message, innerException) { }
}

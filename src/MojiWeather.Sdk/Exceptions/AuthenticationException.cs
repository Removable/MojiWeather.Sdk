namespace MojiWeather.Sdk.Exceptions;

/// <summary>
/// 认证异常
/// </summary>
public class AuthenticationException : MojiWeatherException
{
    public AuthenticationException()
        : base("Authentication failed. Please check your AppCode.") { }

    public AuthenticationException(string message) : base(message) { }

    public AuthenticationException(string message, Exception innerException)
        : base(message, innerException) { }
}

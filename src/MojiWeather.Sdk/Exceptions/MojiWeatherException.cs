namespace MojiWeather.Sdk.Exceptions;

/// <summary>
/// 墨迹天气SDK基础异常
/// </summary>
public class MojiWeatherException : Exception
{
    public MojiWeatherException() { }

    public MojiWeatherException(string message) : base(message) { }

    public MojiWeatherException(string message, Exception innerException)
        : base(message, innerException) { }
}

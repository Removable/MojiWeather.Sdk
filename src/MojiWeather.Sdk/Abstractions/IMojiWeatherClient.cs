namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 墨迹天气客户端接口
/// </summary>
public interface IMojiWeatherClient
{
    /// <summary>
    /// 天气实况服务
    /// </summary>
    IWeatherService Weather { get; }

    /// <summary>
    /// 天气预报服务
    /// </summary>
    IForecastService Forecast { get; }

    /// <summary>
    /// 空气质量服务
    /// </summary>
    IAirQualityService AirQuality { get; }

    /// <summary>
    /// 天气预警服务
    /// </summary>
    IAlertService Alert { get; }

    /// <summary>
    /// 生活指数服务
    /// </summary>
    ILiveIndexService LiveIndex { get; }

    /// <summary>
    /// 限行数据服务
    /// </summary>
    ITrafficService Traffic { get; }
}

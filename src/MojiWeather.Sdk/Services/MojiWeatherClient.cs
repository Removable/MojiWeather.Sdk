using MojiWeather.Sdk.Abstractions;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 墨迹天气客户端实现
/// </summary>
public sealed class MojiWeatherClient(
    IWeatherService weatherService,
    IForecastService forecastService,
    IAirQualityService airQualityService,
    IAlertService alertService,
    ILiveIndexService liveIndexService,
    ITrafficService trafficService) : IMojiWeatherClient
{
    /// <inheritdoc />
    public IWeatherService Weather { get; } = weatherService;

    /// <inheritdoc />
    public IForecastService Forecast { get; } = forecastService;

    /// <inheritdoc />
    public IAirQualityService AirQuality { get; } = airQualityService;

    /// <inheritdoc />
    public IAlertService Alert { get; } = alertService;

    /// <inheritdoc />
    public ILiveIndexService LiveIndex { get; } = liveIndexService;

    /// <inheritdoc />
    public ITrafficService Traffic { get; } = trafficService;
}

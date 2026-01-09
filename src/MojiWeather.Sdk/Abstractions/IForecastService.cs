using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 天气预报服务接口
/// </summary>
public interface IForecastService
{
    /// <summary>
    /// 获取3天精简天气预报
    /// </summary>
    Task<ApiResponse<BriefDailyForecastData>> GetBriefForecast3DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取6天天精简气预报
    /// </summary>
    Task<ApiResponse<BriefDailyForecastData>> GetBriefForecast6DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取15天天气预报
    /// </summary>
    Task<ApiResponse<DailyForecastData>> GetForecast15DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取24小时天气预报
    /// </summary>
    Task<ApiResponse<HourlyForecastData>> GetForecast24HoursAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取短时预报
    /// </summary>
    Task<ApiResponse<ShortForecastData>> GetShortForecastAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

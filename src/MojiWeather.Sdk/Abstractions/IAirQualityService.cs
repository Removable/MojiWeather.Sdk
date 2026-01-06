using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 空气质量服务接口
/// </summary>
public interface IAirQualityService
{
    /// <summary>
    /// 获取精简AQI
    /// </summary>
    Task<ApiResponse<BriefAqiData>> GetBriefAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取详细AQI
    /// </summary>
    Task<ApiResponse<DetailedAqiData>> GetDetailedAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取5天AQI预报
    /// </summary>
    Task<ApiResponse<AqiForecastData>> GetAqiForecast5DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

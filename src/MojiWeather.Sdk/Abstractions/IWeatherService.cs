using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 天气实况服务接口
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// 获取精简天气实况
    /// </summary>
    Task<ApiResponse<BriefConditionData>> GetBriefConditionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取详细天气实况
    /// </summary>
    Task<ApiResponse<DetailedConditionData>> GetDetailedConditionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

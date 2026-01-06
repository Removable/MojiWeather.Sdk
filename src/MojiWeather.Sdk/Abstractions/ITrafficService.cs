using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Traffic;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 限行数据服务接口
/// </summary>
public interface ITrafficService
{
    /// <summary>
    /// 获取限行数据
    /// </summary>
    Task<ApiResponse<TrafficRestrictionData>> GetRestrictionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

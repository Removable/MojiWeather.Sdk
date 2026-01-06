using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 生活指数服务接口
/// </summary>
public interface ILiveIndexService
{
    /// <summary>
    /// 获取生活指数
    /// </summary>
    Task<ApiResponse<LiveIndexData>> GetLiveIndexAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

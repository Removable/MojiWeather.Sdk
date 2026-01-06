using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Http;

/// <summary>
/// 墨迹天气HTTP客户端接口
/// </summary>
public interface IMojiHttpClient
{
    /// <summary>
    /// 发送API请求
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    /// <param name="endpoint">端点信息</param>
    /// <param name="location">位置查询</param>
    /// <param name="additionalParameters">附加参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task<ApiResponse<T>> SendAsync<T>(
        EndpointInfo endpoint,
        LocationQuery location,
        IReadOnlyDictionary<string, string>? additionalParameters = null,
        CancellationToken cancellationToken = default) where T : class;
}

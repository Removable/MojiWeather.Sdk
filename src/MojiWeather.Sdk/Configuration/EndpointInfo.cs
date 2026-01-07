namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// API端点信息
/// </summary>
/// <param name="Name">端点名称</param>
/// <param name="Token">API Token</param>
/// <param name="BaseUrl">API基础URL</param>
/// <param name="Path">API路径</param>
/// <param name="MinimumTier">所需订阅级别</param>
public sealed record EndpointInfo(
    string Name,
    string Token,
    string BaseUrl,
    string Path,
    SubscriptionTier MinimumTier)
{
    /// <summary>
    /// 检查指定订阅级别是否有权访问此端点
    /// </summary>
    public bool IsAccessibleWith(SubscriptionTier tier)
        => tier == MinimumTier;
}

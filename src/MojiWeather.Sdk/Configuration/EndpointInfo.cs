namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// API端点信息
/// </summary>
/// <param name="Name">端点名称</param>
/// <param name="Token">API Token</param>
/// <param name="Path">API路径</param>
/// <param name="MinimumTier">最低订阅级别</param>
public sealed record EndpointInfo(
    string Name,
    string Token,
    string Path,
    SubscriptionTier MinimumTier)
{
    /// <summary>
    /// 检查指定订阅级别是否有权访问此端点
    /// </summary>
    public bool IsAccessibleWith(SubscriptionTier tier)
        => tier >= MinimumTier || (MinimumTier == SubscriptionTier.Basic && tier == SubscriptionTier.Professional);
}

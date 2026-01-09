using MojiWeather.Sdk.Configuration;

namespace MojiWeather.Sdk.Exceptions;

/// <summary>
/// 当前订阅级别不支持请求的接口时抛出的异常
/// </summary>
public sealed class SubscriptionTierNotSupportedException : MojiWeatherException
{
    /// <summary>
    /// 接口名称
    /// </summary>
    public string EndpointName { get; }

    /// <summary>
    /// 用户当前的订阅级别
    /// </summary>
    public SubscriptionTier CurrentTier { get; }

    /// <summary>
    /// 接口支持的订阅级别列表
    /// </summary>
    public IReadOnlyList<SubscriptionTier> RequiredTiers { get; }

    /// <summary>
    /// 查询类型（经纬度或CityID）
    /// </summary>
    public string QueryType { get; }

    public SubscriptionTierNotSupportedException(
        string endpointName,
        string queryType,
        SubscriptionTier currentTier,
        IEnumerable<SubscriptionTier> requiredTiers)
        : base($"接口 '{endpointName}'（{queryType}查询）需要订阅级别: {string.Join(", ", requiredTiers)}，当前配置的订阅级别为: {currentTier}")
    {
        EndpointName = endpointName;
        QueryType = queryType;
        CurrentTier = currentTier;
        RequiredTiers = requiredTiers.ToList().AsReadOnly();
    }
}

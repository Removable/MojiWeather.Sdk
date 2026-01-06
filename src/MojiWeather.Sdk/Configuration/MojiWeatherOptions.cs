namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// 墨迹天气SDK配置选项
/// </summary>
public sealed class MojiWeatherOptions
{
    /// <summary>
    /// 配置节名称
    /// </summary>
    public const string SectionName = "MojiWeather";

    /// <summary>
    /// 阿里云API市场的AppCode
    /// </summary>
    public required string AppCode { get; set; }

    /// <summary>
    /// 订阅级别
    /// </summary>
    public SubscriptionTier Tier { get; set; } = SubscriptionTier.Trial;

    /// <summary>
    /// 默认查询类型
    /// </summary>
    public LocationQueryType QueryType { get; set; } = LocationQueryType.Coordinates;

    /// <summary>
    /// 是否使用HTTPS
    /// </summary>
    public bool UseHttps { get; set; } = true;

    /// <summary>
    /// 请求超时时间
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 重试配置
    /// </summary>
    public RetryOptions Retry { get; set; } = new();

    /// <summary>
    /// API基础URL
    /// </summary>
    internal string BaseUrl => UseHttps
        ? "https://aliv18.data.moji.com"
        : "http://aliv18.data.moji.com";
}

/// <summary>
/// 重试配置选项
/// </summary>
public sealed class RetryOptions
{
    /// <summary>
    /// 最大重试次数
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// 初始延迟时间
    /// </summary>
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// 延迟时间乘数
    /// </summary>
    public double BackoffMultiplier { get; set; } = 2.0;
}

/// <summary>
/// 订阅级别
/// </summary>
public enum SubscriptionTier
{
    /// <summary>
    /// 试用版
    /// </summary>
    Trial = 0,

    /// <summary>
    /// PM2.5版
    /// </summary>
    Pm25 = 1,

    /// <summary>
    /// 专业版
    /// </summary>
    Professional = 2,

    /// <summary>
    /// 基础版
    /// </summary>
    Basic = 3
}

/// <summary>
/// 位置查询类型
/// </summary>
public enum LocationQueryType
{
    /// <summary>
    /// 经纬度查询
    /// </summary>
    Coordinates,

    /// <summary>
    /// 城市ID查询
    /// </summary>
    CityId
}

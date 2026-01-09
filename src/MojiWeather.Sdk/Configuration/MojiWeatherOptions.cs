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
    /// 端点Token配置（可选覆盖默认值）
    /// </summary>
    /// <remarks>
    /// Token是API端点标识符，默认值来自官方API文档。
    /// 可通过配置文件覆盖以支持测试或API更新场景。
    /// </remarks>
    public EndpointTokens Tokens { get; set; } = new();
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
    /// 基础版
    /// </summary>
    Basic = 2,

    /// <summary>
    /// 专业版
    /// </summary>
    Professional = 3
}

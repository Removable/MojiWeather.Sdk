using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 精简实况数据
/// </summary>
public sealed record BriefConditionData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 天气实况
    /// </summary>
    [JsonPropertyName("condition")]
    public BriefCondition? Condition { get; init; }
}

/// <summary>
/// 精简天气实况
/// </summary>
public sealed record BriefCondition
{
    /// <summary>
    /// 实时天气现象
    /// </summary>
    [JsonPropertyName("condition")]
    public string? Condition { get; init; }

    /// <summary>
    /// 湿度 (%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public string? Humidity { get; init; }

    /// <summary>
    /// 天气icon
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    /// <summary>
    /// 温度 (摄氏度)
    /// </summary>
    [JsonPropertyName("temp")]
    public string? Temperature { get; init; }

    /// <summary>
    /// 发布时间 (秒)
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 能见度 (m)
    /// </summary>
    [JsonPropertyName("vis")]
    public string? Visibility { get; init; }

    /// <summary>
    /// 风向角度 (度)
    /// </summary>
    [JsonPropertyName("windDegrees")]
    public string? WindDegrees { get; init; }

    /// <summary>
    /// 风向
    /// </summary>
    [JsonPropertyName("windDir")]
    public string? WindDirection { get; init; }

    /// <summary>
    /// 风级
    /// </summary>
    [JsonPropertyName("windLevel")]
    public string? WindLevel { get; init; }
}

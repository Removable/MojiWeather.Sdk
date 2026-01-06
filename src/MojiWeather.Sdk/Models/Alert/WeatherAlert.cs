using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Alert;

/// <summary>
/// 天气预警数据
/// </summary>
public sealed record WeatherAlertData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 预警列表
    /// </summary>
    [JsonPropertyName("alert")]
    public IReadOnlyList<WeatherAlert>? Alerts { get; init; }
}

/// <summary>
/// 天气预警
/// </summary>
public sealed record WeatherAlert
{
    /// <summary>
    /// 预警ID
    /// </summary>
    [JsonPropertyName("alertId")]
    public string? AlertId { get; init; }

    /// <summary>
    /// 预警标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// 预警类型
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// 预警等级
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; init; }

    /// <summary>
    /// 预警等级代码 (蓝/黄/橙/红)
    /// </summary>
    [JsonPropertyName("levelCode")]
    public string? LevelCode { get; init; }

    /// <summary>
    /// 预警内容
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [JsonPropertyName("pubTime")]
    public string? PublishTime { get; init; }

    /// <summary>
    /// 预警来源
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; init; }
}

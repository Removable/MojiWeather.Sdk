using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.AirQuality;

/// <summary>
/// 精简AQI数据
/// </summary>
public sealed record BriefAqiData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// AQI信息
    /// </summary>
    [JsonPropertyName("aqi")]
    public BriefAqi? Aqi { get; init; }
}

/// <summary>
/// 精简空气质量指数
/// </summary>
public sealed record BriefAqi
{
    /// <summary>
    /// AQI值
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; init; }

    /// <summary>
    /// AQI等级描述
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; init; }

    /// <summary>
    /// 主要污染物
    /// </summary>
    [JsonPropertyName("primaryPollutant")]
    public string? PrimaryPollutant { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("pubtime")]
    public string? PublishTime { get; init; }
}

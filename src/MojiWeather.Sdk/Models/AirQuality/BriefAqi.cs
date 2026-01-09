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
    public required string Value { get; init; }

    /// <summary>
    /// 城市名称
    /// </summary>
    [JsonPropertyName("cityName")]
    public required string CityName { get; init; }

    /// <summary>
    /// 更新时间（时间戳，单位毫秒）
    /// </summary>
    [JsonPropertyName("pubtime")]
    public required string PublishTimestamp { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("publishTime")]
    public DateTimeOffset PublishTime => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(PublishTimestamp));
}

using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.AirQuality;

/// <summary>
/// AQI预报数据
/// </summary>
public sealed record AqiForecastData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// AQI预报列表
    /// </summary>
    [JsonPropertyName("aqiForecast")]
    public IReadOnlyList<AqiForecast>? Forecasts { get; init; }
}

/// <summary>
/// AQI预报
/// </summary>
public sealed record AqiForecast
{
    /// <summary>
    /// 预测日期
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

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
    /// 最小AQI值
    /// </summary>
    [JsonPropertyName("min")]
    public int Min { get; init; }

    /// <summary>
    /// 最大AQI值
    /// </summary>
    [JsonPropertyName("max")]
    public int Max { get; init; }

    /// <summary>
    /// 主要污染物
    /// </summary>
    [JsonPropertyName("primaryPollutant")]
    public string? PrimaryPollutant { get; init; }
}

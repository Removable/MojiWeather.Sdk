using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.AirQuality;

/// <summary>
/// 详细AQI数据
/// </summary>
public sealed record DetailedAqiData
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
    public DetailedAqi? Aqi { get; init; }
}

/// <summary>
/// 详细空气质量指数
/// </summary>
public sealed record DetailedAqi
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
    /// PM2.5浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("pm25")]
    public int Pm25 { get; init; }

    /// <summary>
    /// PM10浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("pm10")]
    public int Pm10 { get; init; }

    /// <summary>
    /// CO浓度(mg/m³)
    /// </summary>
    [JsonPropertyName("co")]
    public double Co { get; init; }

    /// <summary>
    /// NO2浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("no2")]
    public int No2 { get; init; }

    /// <summary>
    /// SO2浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("so2")]
    public int So2 { get; init; }

    /// <summary>
    /// O3浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("o3")]
    public int O3 { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("pubtime")]
    public string? PublishTime { get; init; }

    /// <summary>
    /// 提示信息
    /// </summary>
    [JsonPropertyName("tips")]
    public string? Tips { get; init; }
}

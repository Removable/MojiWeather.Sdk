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
    /// 城市名称
    /// </summary>
    [JsonPropertyName("cityName")]
    public required string CityName { get; init; }

    /// <summary>
    /// 发布时间戳（UTC时间戳，单位毫秒）
    /// </summary>
    [JsonPropertyName("pubtime")]
    public required string PublishTimestamp { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTimeOffset PublishTime => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(PublishTimestamp));

    /// <summary>
    /// 空气质量指数值
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>
    /// 一氧化碳指数
    /// </summary>
    [JsonPropertyName("co")]
    public required string Co { get; init; }

    /// <summary>
    /// 二氧化氮指数
    /// </summary>
    [JsonPropertyName("no2")]
    public required string No2 { get; init; }

    /// <summary>
    /// 臭氧指数
    /// </summary>
    [JsonPropertyName("o3")]
    public required string O3 { get; init; }

    /// <summary>
    /// PM2.5指数
    /// </summary>
    [JsonPropertyName("pm25")]
    public required string Pm25 { get; init; }

    /// <summary>
    /// PM10指数
    /// </summary>
    [JsonPropertyName("pm10")]
    public required string Pm10 { get; init; }

    /// <summary>
    /// 全国排名
    /// </summary>
    [JsonPropertyName("rank")]
    public required string Rank { get; init; }

    /// <summary>
    /// 一氧化碳浓度(mg/m³)
    /// </summary>
    [JsonPropertyName("coC")]
    public required string CoConcentration { get; init; }

    /// <summary>
    /// 二氧化氮浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("no2C")]
    public required string No2Concentration { get; init; }

    /// <summary>
    /// 臭氧浓度(μg/m³)
    /// </summary>
    [JsonPropertyName(("o3C"))]
    public required string O3Concentration { get; init; }

    /// <summary>
    /// PM10浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("pm10C")]
    public required string Pm10Concentration { get; init; }

    /// <summary>
    /// 二氧化硫浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("so2C")]
    public required string So2Concentration { get; init; }

    /// <summary>
    /// PM2.5浓度(μg/m³)
    /// </summary>
    [JsonPropertyName("pm25C")]
    public required string Pm25Concentration { get; init; }

    /// <summary>
    /// 二氧化硫指数
    /// </summary>
    [JsonPropertyName("so2")]
    public required string So2 { get; init; }
}

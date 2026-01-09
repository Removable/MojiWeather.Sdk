using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 详细天气实况数据
/// </summary>
public sealed record DetailedConditionData
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
    public DetailedCondition? Condition { get; init; }
}

/// <summary>
/// 详细天气实况
/// </summary>
public sealed record DetailedCondition
{
    /// <summary>
    /// 实时天气现象
    /// </summary>
    [JsonPropertyName("condition")]
    public string? ConditionDescription { get; init; }

    /// <summary>
    /// 实时天气id
    /// </summary>
    [JsonPropertyName("conditionId")]
    public string? ConditionId { get; init; }

    /// <summary>
    /// 湿度(%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public string? Humidity { get; init; }

    /// <summary>
    /// 天气图标ID
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    /// <summary>
    /// 气压(百帕)
    /// </summary>
    [JsonPropertyName("pressure")]
    public string? Pressure { get; init; }

    /// <summary>
    /// 体感温度(摄氏度)
    /// </summary>
    [JsonPropertyName("realFeel")]
    public string? RealFeel { get; init; }

    /// <summary>
    /// 日出时间(yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("sunRise")]
    public string? SunRise { get; init; }

    /// <summary>
    /// 日落时间(yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("sunSet")]
    public string? SunSet { get; init; }

    /// <summary>
    /// 温度(摄氏度)
    /// </summary>
    [JsonPropertyName("temp")]
    public string? Temperature { get; init; }

    /// <summary>
    /// 一句话提示
    /// </summary>
    [JsonPropertyName("tips")]
    public string? Tips { get; init; }

    /// <summary>
    /// 发布时间(yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 紫外线强度
    /// </summary>
    [JsonPropertyName("uvi")]
    public string? UvIndex { get; init; }

    /// <summary>
    /// 能见度(m)
    /// </summary>
    [JsonPropertyName("vis")]
    public string? Visibility { get; init; }

    /// <summary>
    /// 风向角度(度)
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

    /// <summary>
    /// 风速(m/s)
    /// </summary>
    [JsonPropertyName("windSpeed")]
    public string? WindSpeed { get; init; }
}

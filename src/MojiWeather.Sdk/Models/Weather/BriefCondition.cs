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
    /// 温度(摄氏度)
    /// </summary>
    [JsonPropertyName("temp")]
    public int Temperature { get; init; }

    /// <summary>
    /// 湿度(%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

    /// <summary>
    /// 能见度(米)
    /// </summary>
    [JsonPropertyName("vis")]
    public int Visibility { get; init; }

    /// <summary>
    /// 风向角度(度)
    /// </summary>
    [JsonPropertyName("windDegrees")]
    public int WindDegrees { get; init; }

    /// <summary>
    /// 风级
    /// </summary>
    [JsonPropertyName("windLevel")]
    public int WindLevel { get; init; }

    /// <summary>
    /// 天气现象描述
    /// </summary>
    [JsonPropertyName("condition")]
    public string? ConditionDescription { get; init; }

    /// <summary>
    /// 天气图标ID
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 风向描述
    /// </summary>
    [JsonPropertyName("windDir")]
    public string? WindDirection { get; init; }

    /// <summary>
    /// 气压(hPa)
    /// </summary>
    [JsonPropertyName("pressure")]
    public int Pressure { get; init; }

    /// <summary>
    /// 体感温度
    /// </summary>
    [JsonPropertyName("realFeel")]
    public int RealFeel { get; init; }

    /// <summary>
    /// 提示信息
    /// </summary>
    [JsonPropertyName("tips")]
    public string? Tips { get; init; }
}

using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 日预报数据
/// </summary>
public sealed record BriefDailyForecastData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 预报列表
    /// </summary>
    [JsonPropertyName("forecast")]
    public IReadOnlyList<BriefDailyForecast>? Forecasts { get; init; }
}

/// <summary>
/// 日天气预报
/// </summary>
public sealed record BriefDailyForecast
{
    /// <summary>
    /// 白天天气现象
    /// </summary>
    [JsonPropertyName("conditionDay")]
    public string? ConditionDay { get; init; }

    /// <summary>
    /// 白天天气id
    /// </summary>
    [JsonPropertyName("conditionIdDay")]
    public string? ConditionIdDay { get; init; }

    /// <summary>
    /// 夜间天气id
    /// </summary>
    [JsonPropertyName("conditionIdNight")]
    public string? ConditionIdNight { get; init; }

    /// <summary>
    /// 夜间天气现象
    /// </summary>
    [JsonPropertyName("conditionNight")]
    public string? ConditionNight { get; init; }

    /// <summary>
    /// 相对湿度 (%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public string? Humidity { get; init; }

    /// <summary>
    /// 预报日期 (天)
    /// </summary>
    [JsonPropertyName("predictDate")]
    public string? PredictDate { get; init; }

    /// <summary>
    /// 白天温度 (摄氏度)
    /// </summary>
    [JsonPropertyName("tempDay")]
    public int TempDay { get; init; }

    /// <summary>
    /// 夜间温度 (摄氏度)
    /// </summary>
    [JsonPropertyName("tempNight")]
    public int TempNight { get; init; }

    /// <summary>
    /// 更新时间 (秒)
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 白天风向角度 (度)
    /// </summary>
    [JsonPropertyName("windDegreesDay")]
    public string? WindDegreesDay { get; init; }

    /// <summary>
    /// 夜间风向角度 (度)
    /// </summary>
    [JsonPropertyName("windDegreesNight")]
    public string? WindDegreesNight { get; init; }

    /// <summary>
    /// 白天风向
    /// </summary>
    [JsonPropertyName("windDirDay")]
    public string? WindDirDay { get; init; }

    /// <summary>
    /// 夜间风向
    /// </summary>
    [JsonPropertyName("windDirNight")]
    public string? WindDirNight { get; init; }

    /// <summary>
    /// 白天风级
    /// </summary>
    [JsonPropertyName("windLevelDay")]
    public string? WindLevelDay { get; init; }

    /// <summary>
    /// 白天风速 (米/秒)
    /// </summary>
    [JsonPropertyName("windSpeedDay")]
    public string? WindSpeedDay { get; init; }

    /// <summary>
    /// 夜间风级
    /// </summary>
    [JsonPropertyName("windLevelNight")]
    public string? WindLevelNight { get; init; }

    /// <summary>
    /// 夜间风速 (米/秒)
    /// </summary>
    [JsonPropertyName("windSpeedNight")]
    public string? WindSpeedNight { get; init; }

    /// <summary>
    /// 降水概率
    /// </summary>
    [JsonPropertyName("pop")]
    public string? Pop { get; init; }
}

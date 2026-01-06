using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 日预报数据
/// </summary>
public sealed record DailyForecastData
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
    public IReadOnlyList<DailyForecast>? Forecasts { get; init; }
}

/// <summary>
/// 日天气预报
/// </summary>
public sealed record DailyForecast
{
    /// <summary>
    /// 预测日期 (yyyyMMdd)
    /// </summary>
    [JsonPropertyName("predictDate")]
    public string? PredictDate { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 白天天气现象
    /// </summary>
    [JsonPropertyName("conditionDay")]
    public string? ConditionDay { get; init; }

    /// <summary>
    /// 夜间天气现象
    /// </summary>
    [JsonPropertyName("conditionNight")]
    public string? ConditionNight { get; init; }

    /// <summary>
    /// 白天天气图标
    /// </summary>
    [JsonPropertyName("conditionIdDay")]
    public string? ConditionIdDay { get; init; }

    /// <summary>
    /// 夜间天气图标
    /// </summary>
    [JsonPropertyName("conditionIdNight")]
    public string? ConditionIdNight { get; init; }

    /// <summary>
    /// 最高温度
    /// </summary>
    [JsonPropertyName("tempHigh")]
    public int TempHigh { get; init; }

    /// <summary>
    /// 最低温度
    /// </summary>
    [JsonPropertyName("tempLow")]
    public int TempLow { get; init; }

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
    public int WindLevelDay { get; init; }

    /// <summary>
    /// 夜间风级
    /// </summary>
    [JsonPropertyName("windLevelNight")]
    public int WindLevelNight { get; init; }

    /// <summary>
    /// 日出时间
    /// </summary>
    [JsonPropertyName("sunRise")]
    public string? SunRise { get; init; }

    /// <summary>
    /// 日落时间
    /// </summary>
    [JsonPropertyName("sunSet")]
    public string? SunSet { get; init; }

    /// <summary>
    /// 月出时间
    /// </summary>
    [JsonPropertyName("moonrise")]
    public string? MoonRise { get; init; }

    /// <summary>
    /// 月落时间
    /// </summary>
    [JsonPropertyName("moonset")]
    public string? MoonSet { get; init; }

    /// <summary>
    /// 月相
    /// </summary>
    [JsonPropertyName("moonPhase")]
    public string? MoonPhase { get; init; }

    /// <summary>
    /// 降水概率(%)
    /// </summary>
    [JsonPropertyName("pop")]
    public int PrecipitationProbability { get; init; }

    /// <summary>
    /// 紫外线指数
    /// </summary>
    [JsonPropertyName("uvi")]
    public int UvIndex { get; init; }

    /// <summary>
    /// 湿度(%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }
}

using System.Globalization;
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
    /// 白天天气现象
    /// </summary>
    [JsonPropertyName("conditionDay")]
    public string? ConditionDay { get; init; }

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
    /// 夜间天气现象
    /// </summary>
    [JsonPropertyName("conditionNight")]
    public string? ConditionNight { get; init; }

    /// <summary>
    /// 相对湿度
    /// 单位：%
    /// </summary>
    [JsonPropertyName("humidity")]
    public string? Humidity { get; init; }

    /// <summary>
    /// 月相
    /// </summary>
    [JsonPropertyName("moonphase")]
    public string? Moonphase { get; init; }

    /// <summary>
    /// 月出时间
    /// 单位：秒
    /// </summary>
    [JsonPropertyName("moonrise")]
    public string? Moonrise { get; init; }

    [JsonPropertyName("moonriseTime")]
    public DateTime? MoonriseTime => Moonrise == null
        ? null
        : DateTime.ParseExact(Moonrise, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// 月落时间
    /// </summary>
    [JsonPropertyName("moonset")]
    public string? Moonset { get; init; }

    /// <summary>
    /// 月落时间
    /// </summary>
    [JsonPropertyName("moonsetTime")]
    public DateTime? MoonsetTime => Moonset == null
        ? null
        : DateTime.ParseExact(Moonset, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// 降水概率
    /// </summary>
    [JsonPropertyName("pop")]
    public string? Pop { get; init; }

    /// <summary>
    /// 预测日期 (yyyy-MM-dd)
    /// </summary>
    [JsonPropertyName("predictDate")]
    public string? PredictDate { get; init; }

    [JsonPropertyName("predictDateOnly")]
    public DateOnly? PredictDateOnly => PredictDate == null
        ? null
        : DateOnly.ParseExact(PredictDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    /// <summary>
    /// 未来一天降水预报
    /// 单位：毫米
    /// </summary>
    [JsonPropertyName("qpf")]
    public string? Qpf { get; init; }

    /// <summary>
    /// 日出时间
    /// </summary>
    [JsonPropertyName("sunrise")]
    public string? Sunrise { get; init; }

    /// <summary>
    /// 日出时间
    /// </summary>
    [JsonPropertyName("sunriseTime")]
    public DateTime? SunriseTime => Sunrise == null
        ? null
        : DateTime.ParseExact(Sunrise, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// 日落时间
    /// </summary>
    [JsonPropertyName("sunset")]
    public string? Sunset { get; init; }

    /// <summary>
    /// 日落时间
    /// </summary>
    [JsonPropertyName("sunsetTime")]
    public DateTime? SunsetTime => Sunset == null
        ? null
        : DateTime.ParseExact(Sunset, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// 白天温度（最高温）
    /// 单位：摄氏度
    /// </summary>
    [JsonPropertyName("tempDay")]
    public string? TempDay { get; init; }

    /// <summary>
    /// 夜间温度（最低温）
    /// 单位：摄氏度
    /// </summary>
    [JsonPropertyName("tempNight")]
    public string? TempNight { get; init; }

    /// <summary>
    /// 更新时间
    /// 单位：秒
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 紫外线指数
    /// </summary>
    [JsonPropertyName("uvi")]
    public string? UvIndex { get; init; }

    /// <summary>
    /// 白天风向角度
    /// 单位：度
    /// </summary>
    [JsonPropertyName("windDegreesDay")]
    public string? WindDegreesDay { get; init; }

    /// <summary>
    /// 夜间风向角度
    /// 单位：度
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
    /// 夜间风级
    /// </summary>
    [JsonPropertyName("windLevelNight")]
    public string? WindLevelNight { get; init; }

    /// <summary>
    /// 白天风速
    /// 单位：米/秒
    /// </summary>
    [JsonPropertyName("windSpeedDay")]
    public string? WindSpeedDay { get; init; }

    /// <summary>
    /// 夜间风速
    /// 单位：米/秒
    /// </summary>
    [JsonPropertyName("windSpeedNight")]
    public string? WindSpeedNight { get; init; }
}

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
    /// 白天天气图标数值
    /// </summary>
    [JsonIgnore]
    public int? ConditionIdDayValue => int.TryParse(ConditionIdDay, out var v) ? v : null;

    /// <summary>
    /// 夜间天气图标
    /// </summary>
    [JsonPropertyName("conditionIdNight")]
    public string? ConditionIdNight { get; init; }

    /// <summary>
    /// 夜间天气图标数值
    /// </summary>
    [JsonIgnore]
    public int? ConditionIdNightValue => int.TryParse(ConditionIdNight, out var v) ? v : null;

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
    /// 相对湿度数值 (%)
    /// </summary>
    [JsonIgnore]
    public int? HumidityValue => int.TryParse(Humidity, out var v) ? v : null;

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

    /// <summary>
    /// 月出时间
    /// </summary>
    [JsonIgnore]
    public DateTime? MoonriseTime => Moonrise == null
        ? null
        : DateTime.TryParseExact(Moonrise, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    /// <summary>
    /// 月落时间
    /// </summary>
    [JsonPropertyName("moonset")]
    public string? Moonset { get; init; }

    /// <summary>
    /// 月落时间
    /// </summary>
    [JsonIgnore]
    public DateTime? MoonsetTime => Moonset == null
        ? null
        : DateTime.TryParseExact(Moonset, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    /// <summary>
    /// 降水概率
    /// </summary>
    [JsonPropertyName("pop")]
    public string? Pop { get; init; }

    /// <summary>
    /// 降水概率数值 (%)
    /// </summary>
    [JsonIgnore]
    public int? PopValue => int.TryParse(Pop, out var v) ? v : null;

    /// <summary>
    /// 预测日期 (yyyy-MM-dd)
    /// </summary>
    [JsonPropertyName("predictDate")]
    public string? PredictDate { get; init; }

    /// <summary>
    /// 预测日期
    /// </summary>
    [JsonIgnore]
    public DateOnly? PredictDateOnly => PredictDate == null
        ? null
        : DateOnly.TryParseExact(PredictDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;

    /// <summary>
    /// 未来一天降水预报
    /// 单位：毫米
    /// </summary>
    [JsonPropertyName("qpf")]
    public string? Qpf { get; init; }

    /// <summary>
    /// 未来一天降水预报数值 (毫米)
    /// </summary>
    [JsonIgnore]
    public double? QpfValue => double.TryParse(Qpf, out var v) ? v : null;

    /// <summary>
    /// 日出时间
    /// </summary>
    [JsonPropertyName("sunrise")]
    public string? Sunrise { get; init; }

    /// <summary>
    /// 日出时间
    /// </summary>
    [JsonIgnore]
    public DateTime? SunriseTime => Sunrise == null
        ? null
        : DateTime.TryParseExact(Sunrise, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    /// <summary>
    /// 日落时间
    /// </summary>
    [JsonPropertyName("sunset")]
    public string? Sunset { get; init; }

    /// <summary>
    /// 日落时间
    /// </summary>
    [JsonIgnore]
    public DateTime? SunsetTime => Sunset == null
        ? null
        : DateTime.TryParseExact(Sunset, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    /// <summary>
    /// 白天温度（最高温）
    /// 单位：摄氏度
    /// </summary>
    [JsonPropertyName("tempDay")]
    public string? TempDay { get; init; }

    /// <summary>
    /// 白天温度数值 (摄氏度)
    /// </summary>
    [JsonIgnore]
    public int? TempDayValue => int.TryParse(TempDay, out var v) ? v : null;

    /// <summary>
    /// 夜间温度（最低温）
    /// 单位：摄氏度
    /// </summary>
    [JsonPropertyName("tempNight")]
    public string? TempNight { get; init; }

    /// <summary>
    /// 夜间温度数值 (摄氏度)
    /// </summary>
    [JsonIgnore]
    public int? TempNightValue => int.TryParse(TempNight, out var v) ? v : null;

    /// <summary>
    /// 更新时间
    /// 单位：秒
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 更新时间数值 (Unix时间戳秒)
    /// </summary>
    [JsonIgnore]
    public long? UpdateTimeValue => long.TryParse(UpdateTime, out var v) ? v : null;

    /// <summary>
    /// 紫外线指数
    /// </summary>
    [JsonPropertyName("uvi")]
    public string? UvIndex { get; init; }

    /// <summary>
    /// 紫外线指数数值
    /// </summary>
    [JsonIgnore]
    public int? UvIndexValue => int.TryParse(UvIndex, out var v) ? v : null;

    /// <summary>
    /// 白天风向角度
    /// 单位：度
    /// </summary>
    [JsonPropertyName("windDegreesDay")]
    public string? WindDegreesDay { get; init; }

    /// <summary>
    /// 白天风向角度数值 (度)
    /// </summary>
    [JsonIgnore]
    public int? WindDegreesDayValue => int.TryParse(WindDegreesDay, out var v) ? v : null;

    /// <summary>
    /// 夜间风向角度
    /// 单位：度
    /// </summary>
    [JsonPropertyName("windDegreesNight")]
    public string? WindDegreesNight { get; init; }

    /// <summary>
    /// 夜间风向角度数值 (度)
    /// </summary>
    [JsonIgnore]
    public int? WindDegreesNightValue => int.TryParse(WindDegreesNight, out var v) ? v : null;

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
    /// 白天风级数值
    /// </summary>
    [JsonIgnore]
    public int? WindLevelDayValue => int.TryParse(WindLevelDay, out var v) ? v : null;

    /// <summary>
    /// 夜间风级
    /// </summary>
    [JsonPropertyName("windLevelNight")]
    public string? WindLevelNight { get; init; }

    /// <summary>
    /// 夜间风级数值
    /// </summary>
    [JsonIgnore]
    public int? WindLevelNightValue => int.TryParse(WindLevelNight, out var v) ? v : null;

    /// <summary>
    /// 白天风速
    /// 单位：米/秒
    /// </summary>
    [JsonPropertyName("windSpeedDay")]
    public string? WindSpeedDay { get; init; }

    /// <summary>
    /// 白天风速数值 (米/秒)
    /// </summary>
    [JsonIgnore]
    public double? WindSpeedDayValue => double.TryParse(WindSpeedDay, out var v) ? v : null;

    /// <summary>
    /// 夜间风速
    /// 单位：米/秒
    /// </summary>
    [JsonPropertyName("windSpeedNight")]
    public string? WindSpeedNight { get; init; }

    /// <summary>
    /// 夜间风速数值 (米/秒)
    /// </summary>
    [JsonIgnore]
    public double? WindSpeedNightValue => double.TryParse(WindSpeedNight, out var v) ? v : null;
}

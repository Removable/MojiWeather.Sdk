using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 小时预报数据
/// </summary>
public sealed record HourlyForecastData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 小时预报列表
    /// </summary>
    [JsonPropertyName("hourly")]
    public IReadOnlyList<HourlyForecast>? Forecasts { get; init; }
}

/// <summary>
/// 小时天气预报
/// </summary>
public sealed record HourlyForecast
{
    /// <summary>
    /// 天气状况
    /// </summary>
    [JsonPropertyName("condition")]
    public string? Condition { get; init; }

    /// <summary>
    /// 天气现象id
    /// </summary>
    [JsonPropertyName("conditionId")]
    public string? ConditionId { get; init; }

    /// <summary>
    /// 预报日期 (yyyy-MM-dd)
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// 小时
    /// </summary>
    [JsonPropertyName("hour")]
    public string? Hour { get; init; }

    /// <summary>
    /// 湿度 (%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public string? Humidity { get; init; }

    /// <summary>
    /// 降冰量 (毫米)
    /// </summary>
    [JsonPropertyName("ice")]
    public string? Ice { get; init; }

    /// <summary>
    /// 白天icon
    /// </summary>
    [JsonPropertyName("iconDay")]
    public string? IconDay { get; init; }

    /// <summary>
    /// 夜间icon
    /// </summary>
    [JsonPropertyName("iconNight")]
    public string? IconNight { get; init; }

    /// <summary>
    /// 降水概率
    /// </summary>
    [JsonPropertyName("pop")]
    public string? Pop { get; init; }

    /// <summary>
    /// 气压 (百帕)
    /// </summary>
    [JsonPropertyName("pressure")]
    public string? Pressure { get; init; }

    /// <summary>
    /// 未来一小时降水预报 (毫米)
    /// </summary>
    [JsonPropertyName("qpf")]
    public string? Qpf { get; init; }

    /// <summary>
    /// 体感温度 (摄氏度)
    /// </summary>
    [JsonPropertyName("realFeel")]
    public string? RealFeel { get; init; }

    /// <summary>
    /// 降雪量 (mm)
    /// </summary>
    [JsonPropertyName("snow")]
    public string? Snow { get; init; }

    /// <summary>
    /// 实时温度 (摄氏度)
    /// </summary>
    [JsonPropertyName("temp")]
    public string? Temp { get; init; }

    /// <summary>
    /// 更新时间 (yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 紫外线强度
    /// </summary>
    [JsonPropertyName("uvi")]
    public string? Uvi { get; init; }

    /// <summary>
    /// 风向角度 (度)
    /// </summary>
    [JsonPropertyName("windDegrees")]
    public string? WindDegrees { get; init; }

    /// <summary>
    /// 风向
    /// </summary>
    [JsonPropertyName("windDir")]
    public string? WindDir { get; init; }

    /// <summary>
    /// 风速 (千米/时)
    /// </summary>
    [JsonPropertyName("windSpeed")]
    public string? WindSpeed { get; init; }

    /// <summary>
    /// 风力等级
    /// </summary>
    [JsonPropertyName("windlevel")]
    public string? WindLevel { get; init; }
}

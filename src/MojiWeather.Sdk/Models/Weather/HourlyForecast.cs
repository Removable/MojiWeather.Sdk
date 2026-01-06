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
    /// 预测时间 (时间戳毫秒)
    /// </summary>
    [JsonPropertyName("date")]
    public long Date { get; init; }

    /// <summary>
    /// 小时 (0-23)
    /// </summary>
    [JsonPropertyName("hour")]
    public int Hour { get; init; }

    /// <summary>
    /// 温度
    /// </summary>
    [JsonPropertyName("temp")]
    public int Temperature { get; init; }

    /// <summary>
    /// 天气现象
    /// </summary>
    [JsonPropertyName("condition")]
    public string? Condition { get; init; }

    /// <summary>
    /// 天气图标
    /// </summary>
    [JsonPropertyName("conditionId")]
    public string? ConditionId { get; init; }

    /// <summary>
    /// 风向
    /// </summary>
    [JsonPropertyName("windDir")]
    public string? WindDirection { get; init; }

    /// <summary>
    /// 风级
    /// </summary>
    [JsonPropertyName("windLevel")]
    public int WindLevel { get; init; }

    /// <summary>
    /// 风速(m/s)
    /// </summary>
    [JsonPropertyName("windSpeed")]
    public double WindSpeed { get; init; }

    /// <summary>
    /// 降水概率(%)
    /// </summary>
    [JsonPropertyName("pop")]
    public int PrecipitationProbability { get; init; }

    /// <summary>
    /// 湿度(%)
    /// </summary>
    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

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
    /// 紫外线指数
    /// </summary>
    [JsonPropertyName("uvi")]
    public int UvIndex { get; init; }
}

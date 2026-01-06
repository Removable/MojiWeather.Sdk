using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Weather;

/// <summary>
/// 短时预报数据
/// </summary>
public sealed record ShortForecastData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 短时预报
    /// </summary>
    [JsonPropertyName("sfc")]
    public ShortForecast? ShortForecast { get; init; }
}

/// <summary>
/// 短时天气预报
/// </summary>
public sealed record ShortForecast
{
    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }

    /// <summary>
    /// 预报描述
    /// </summary>
    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    /// <summary>
    /// 分钟级降水数据
    /// </summary>
    [JsonPropertyName("precipitation")]
    public IReadOnlyList<MinutePrecipitation>? Precipitations { get; init; }
}

/// <summary>
/// 分钟级降水
/// </summary>
public sealed record MinutePrecipitation
{
    /// <summary>
    /// 时间 (距离当前的分钟数)
    /// </summary>
    [JsonPropertyName("minute")]
    public int Minute { get; init; }

    /// <summary>
    /// 降水量(mm)
    /// </summary>
    [JsonPropertyName("value")]
    public double Value { get; init; }

    /// <summary>
    /// 降水类型 (0:无, 1:雨, 2:雪)
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }
}

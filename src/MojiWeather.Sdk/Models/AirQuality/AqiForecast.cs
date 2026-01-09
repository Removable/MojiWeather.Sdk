using System.Globalization;
using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.AirQuality;

/// <summary>
/// AQI预报数据
/// </summary>
public sealed record AqiForecastData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// AQI预报列表
    /// </summary>
    [JsonPropertyName("aqiForecast")]
    public IReadOnlyList<AqiForecast>? Forecasts { get; init; }
}

/// <summary>
/// AQI预报
/// </summary>
public sealed record AqiForecast
{
    /// <summary>
    /// 发布日期
    /// </summary>
    [JsonPropertyName("publishTime")]
    public required string PublishTime { get; init; }

    [JsonPropertyName("PublishTimeValue")]
    public DateTime PublishTimeValue =>
        DateTime.ParseExact(PublishTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// AQI值
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; init; }

    /// <summary>
    /// 预测日期
    /// </summary>
    [JsonPropertyName("date")]
    public required string Date { get; init; }

    [JsonPropertyName("DateValue")]
    public DateOnly DateValue => DateOnly.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
}

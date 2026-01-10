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
    /// 发布日期 (yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("publishTime")]
    public required string PublishTime { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [JsonIgnore]
    public DateTime? PublishTimeValue => DateTime.TryParseExact(PublishTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    /// <summary>
    /// AQI值
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; init; }

    /// <summary>
    /// 预测日期 (yyyy-MM-dd)
    /// </summary>
    [JsonPropertyName("date")]
    public required string Date { get; init; }

    /// <summary>
    /// 预测日期
    /// </summary>
    [JsonIgnore]
    public DateOnly? DateValue => DateOnly.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;
}

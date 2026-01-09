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
    /// 简短预报文案
    /// </summary>
    [JsonPropertyName("banner")]
    public string? Banner { get; init; }

    /// <summary>
    /// 确认信息
    /// </summary>
    [JsonPropertyName("confirmInfo")]
    public ConfirmInfo? ConfirmInfo { get; init; }

    /// <summary>
    /// 是否正确 (255表示未知)
    /// </summary>
    [JsonPropertyName("isCorrect")]
    public int IsCorrect { get; init; }

    /// <summary>
    /// 是否反馈 (0代表未反馈，1代表已反馈)
    /// </summary>
    [JsonPropertyName("isFeedback")]
    public int IsFeedback { get; init; }

    /// <summary>
    /// 附近降雨信息
    /// </summary>
    [JsonPropertyName("nearRain")]
    public string? NearRain { get; init; }

    /// <summary>
    /// 完整预报文案
    /// </summary>
    [JsonPropertyName("notice")]
    public string? Notice { get; init; }

    /// <summary>
    /// 短时雨强
    /// </summary>
    [JsonPropertyName("percent")]
    public IReadOnlyList<PercentInfo> Percent { get; init; } = [];

    /// <summary>
    /// 是否降水 (0代表当前位置没有降水，1代表有降水)
    /// </summary>
    [JsonPropertyName("rain")]
    public int Rain { get; init; }

    /// <summary>
    /// 降水持续时间 (分钟)
    /// </summary>
    [JsonPropertyName("rainLastTime")]
    public int RainLastTime { get; init; }

    /// <summary>
    /// 反演实况
    /// </summary>
    [JsonPropertyName("sfCondition")]
    public int SfCondition { get; init; }

    /// <summary>
    /// 更新时间戳 (UTC时间戳/毫秒)
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("updatetime")]
    public DateTimeOffset UpdateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp);
}

/// <summary>
/// 确认信息
/// </summary>
public sealed record ConfirmInfo
{
    /// <summary>
    /// 确认图标 (255表示未知)
    /// </summary>
    [JsonPropertyName("comfirmIcon")]
    public int ComfirmIcon { get; init; }

    /// <summary>
    /// 确认图标描述
    /// </summary>
    [JsonPropertyName("comfirmIconDesc")]
    public string? ComfirmIconDesc { get; init; }

    /// <summary>
    /// 是否确认 (0代表未确认，1代表已确认)
    /// </summary>
    [JsonPropertyName("isConfirm")]
    public int IsConfirm { get; init; }
}

/// <summary>
/// 短时雨强数据
/// </summary>
public sealed record PercentInfo
{
    /// <summary>
    /// 雷达反射强度
    /// </summary>
    [JsonPropertyName("dbz")]
    public int Dbz { get; init; }

    /// <summary>
    /// 降雨级别描述
    /// </summary>
    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    /// <summary>
    /// 天气图标 (-1表示无图标)
    /// </summary>
    [JsonPropertyName("icon")]
    public int Icon { get; init; }

    /// <summary>
    /// 短时雨强
    /// <para>[0,0.063)无雨</para>
    /// <para>[0.063,0.33)小雨</para>
    /// <para>[0.33,0.66)中雨</para>
    /// <para>[0.66,1]大雨</para>
    /// </summary>
    [JsonPropertyName("percent")]
    public double Percent { get; init; }
}

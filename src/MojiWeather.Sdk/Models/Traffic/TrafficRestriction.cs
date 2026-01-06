using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Traffic;

/// <summary>
/// 限行数据
/// </summary>
public sealed record TrafficRestrictionData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 限行信息
    /// </summary>
    [JsonPropertyName("limit")]
    public TrafficRestriction? Limit { get; init; }
}

/// <summary>
/// 车辆限行信息
/// </summary>
public sealed record TrafficRestriction
{
    /// <summary>
    /// 日期
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// 是否限行
    /// </summary>
    [JsonPropertyName("isLimit")]
    public bool IsLimit { get; init; }

    /// <summary>
    /// 限行尾号
    /// </summary>
    [JsonPropertyName("tailNumber")]
    public string? TailNumber { get; init; }

    /// <summary>
    /// 限行时段描述
    /// </summary>
    [JsonPropertyName("time")]
    public string? Time { get; init; }

    /// <summary>
    /// 限行区域
    /// </summary>
    [JsonPropertyName("area")]
    public string? Area { get; init; }

    /// <summary>
    /// 限行备注
    /// </summary>
    [JsonPropertyName("remark")]
    public string? Remark { get; init; }

    /// <summary>
    /// 处罚说明
    /// </summary>
    [JsonPropertyName("penalty")]
    public string? Penalty { get; init; }
}

using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.LiveIndex;

/// <summary>
/// 生活指数数据
/// </summary>
public sealed record LiveIndexData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 生活指数列表
    /// </summary>
    [JsonPropertyName("liveIndex")]
    public IReadOnlyList<LiveIndex>? LiveIndexes { get; init; }
}

/// <summary>
/// 生活指数
/// </summary>
public sealed record LiveIndex
{
    /// <summary>
    /// 指数日期
    /// </summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>
    /// 指数ID
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }

    /// <summary>
    /// 指数名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 指数等级
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; init; }

    /// <summary>
    /// 指数状态描述
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// 指数详细描述
    /// </summary>
    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    /// <summary>
    /// 建议
    /// </summary>
    [JsonPropertyName("tips")]
    public string? Tips { get; init; }
}

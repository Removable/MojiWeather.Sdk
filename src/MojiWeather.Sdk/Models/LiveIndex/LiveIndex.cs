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
    /// 生活指数列表 (按日期分组)
    /// </summary>
    [JsonPropertyName("liveIndex")]
    public IReadOnlyDictionary<string, IReadOnlyList<LiveIndex>>? LiveIndexes { get; init; }
}

/// <summary>
/// 生活指数
/// </summary>
public sealed record LiveIndex
{
    /// <summary>
    /// 指数代码
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }

    /// <summary>
    /// 日期 (格式: yyyy-MM-dd)
    /// </summary>
    [JsonPropertyName("day")]
    public string? Day { get; init; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonPropertyName("desc")]
    public string? Description { get; init; }

    /// <summary>
    /// 等级
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; init; }

    /// <summary>
    /// 指数名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 状态
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// 更新时间 (格式: yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("updatetime")]
    public string? UpdateTime { get; init; }
}

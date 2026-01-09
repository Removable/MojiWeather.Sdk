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
    public IReadOnlyList<TrafficRestriction>? Limit { get; init; }
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
    public DateTime? Date { get; init; }

    /// <summary>
    /// 限行尾号
    /// <para>
    /// W表示当天不限行，S表示单号限行，D表示双号限行，27表示限行的尾号是2和7，其它的数字同理。
    /// </para>
    /// </summary>
    [JsonPropertyName("prompt")]
    public string? Prompt { get; init; }

    [JsonPropertyName("promptDesc")]
    public string? PromptDescription => Prompt switch
    {
        "W" => "不限行",
        "S" => "单号限行",
        "D" => "双号限行",
        _ => GetPromptDescriptionForNumberValue(Prompt)
    };

    private static string? GetPromptDescriptionForNumberValue(string? prompt)
    {
        if (prompt == null)
        {
            return null;
        }

        if (int.TryParse(prompt, out _))
        {
            var chars = prompt.ToCharArray();
            return $"限行尾号{string.Join("、", chars)}";
        }

        return prompt;
    }
}

using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Models.Common;

/// <summary>
/// 城市信息
/// </summary>
public sealed record CityInfo
{
    /// <summary>
    /// 城市ID
    /// </summary>
    [JsonPropertyName("cityId")]
    public long CityId { get; init; }

    /// <summary>
    /// 国家名称
    /// </summary>
    [JsonPropertyName("counname")]
    public string? CountryName { get; init; }

    /// <summary>
    /// IANA标准时区名称
    /// </summary>
    [JsonPropertyName("ianatimezone")]
    public string? IanaTimezone { get; init; }

    /// <summary>
    /// 城市名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 省份名称
    /// </summary>
    [JsonPropertyName("pname")]
    public string? ProvinceName { get; init; }

    /// <summary>
    /// 时区，相对于UTC时区的偏移量
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; init; }

    /// <summary>
    /// 上级城市名称
    /// </summary>
    [JsonPropertyName("secondaryname")]
    public string? SecondaryName { get; init; }
}

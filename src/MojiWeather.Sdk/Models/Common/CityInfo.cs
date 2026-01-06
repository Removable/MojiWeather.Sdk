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
    /// 城市名称
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }

    /// <summary>
    /// 城市名称(别名)
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 国家名称
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>
    /// 国家代码
    /// </summary>
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; init; }

    /// <summary>
    /// 省份名称
    /// </summary>
    [JsonPropertyName("pname")]
    public string? Province { get; init; }

    /// <summary>
    /// 纬度
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    /// <summary>
    /// 经度
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    /// <summary>
    /// 时区
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; init; }
}

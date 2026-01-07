using System.Collections.Frozen;
using System.Globalization;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 位置查询抽象基类，支持经纬度或城市ID两种查询方式
/// </summary>
public abstract record LocationQuery
{
    /// <summary>
    /// 通过经纬度创建位置查询
    /// </summary>
    /// <param name="lat">纬度</param>
    /// <param name="lon">经度</param>
    public static LocationQuery FromCoordinates(double lat, double lon)
        => new CoordinatesQuery(lat, lon);

    /// <summary>
    /// 通过城市ID创建位置查询
    /// </summary>
    /// <param name="cityId">城市ID</param>
    public static LocationQuery FromCityId(long cityId)
        => new CityIdQuery(cityId);

    /// <summary>
    /// 获取用于API请求的表单参数
    /// </summary>
    internal abstract IReadOnlyDictionary<string, string> ToFormParameters();

    /// <summary>
    /// 是否为经纬度查询
    /// </summary>
    public abstract bool IsCoordinatesQuery { get; }
}

/// <summary>
/// 经纬度位置查询
/// </summary>
public sealed record CoordinatesQuery : LocationQuery
{
    private readonly FrozenDictionary<string, string> _formParameters;

    public double Lat { get; }
    public double Lon { get; }

    public CoordinatesQuery(double Lat, double Lon)
    {
        this.Lat = Lat;
        this.Lon = Lon;
        // 使用FrozenDictionary缓存表单参数，避免重复分配
        _formParameters = new Dictionary<string, string>
        {
            ["lat"] = Lat.ToString(CultureInfo.InvariantCulture),
            ["lon"] = Lon.ToString(CultureInfo.InvariantCulture)
        }.ToFrozenDictionary();
    }

    public override bool IsCoordinatesQuery => true;

    internal override IReadOnlyDictionary<string, string> ToFormParameters() => _formParameters;

    // 支持解构
    public void Deconstruct(out double lat, out double lon)
    {
        lat = Lat;
        lon = Lon;
    }
}

/// <summary>
/// 城市ID位置查询
/// </summary>
public sealed record CityIdQuery : LocationQuery
{
    private readonly FrozenDictionary<string, string> _formParameters;

    public long CityId { get; }

    public CityIdQuery(long CityId)
    {
        this.CityId = CityId;
        // 使用FrozenDictionary缓存表单参数，避免重复分配
        _formParameters = new Dictionary<string, string>
        {
            ["cityId"] = CityId.ToString()
        }.ToFrozenDictionary();
    }

    public override bool IsCoordinatesQuery => false;

    internal override IReadOnlyDictionary<string, string> ToFormParameters() => _formParameters;

    // 支持解构
    public void Deconstruct(out long cityId)
    {
        cityId = CityId;
    }
}

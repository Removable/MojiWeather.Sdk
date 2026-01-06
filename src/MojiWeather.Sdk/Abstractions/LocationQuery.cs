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
public sealed record CoordinatesQuery(double Lat, double Lon) : LocationQuery
{
    public override bool IsCoordinatesQuery => true;

    internal override IReadOnlyDictionary<string, string> ToFormParameters() => new Dictionary<string, string>
    {
        ["lat"] = Lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
        ["lon"] = Lon.ToString(System.Globalization.CultureInfo.InvariantCulture)
    };
}

/// <summary>
/// 城市ID位置查询
/// </summary>
public sealed record CityIdQuery(long CityId) : LocationQuery
{
    public override bool IsCoordinatesQuery => false;

    internal override IReadOnlyDictionary<string, string> ToFormParameters() => new Dictionary<string, string>
    {
        ["cityId"] = CityId.ToString()
    };
}

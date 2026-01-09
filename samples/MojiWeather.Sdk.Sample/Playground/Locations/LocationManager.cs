using MojiWeather.Sdk.Abstractions;

namespace MojiWeather.Sdk.Sample.Playground.Locations;

/// <summary>
/// 预设位置
/// </summary>
public sealed record PresetLocation(
    string ChineseName,
    string EnglishName,
    double Latitude,
    double Longitude)
{
    /// <summary>
    /// 转换为位置查询对象
    /// </summary>
    public LocationQuery ToLocationQuery() => LocationQuery.FromCoordinates(Latitude, Longitude);

    /// <summary>
    /// 获取显示名称
    /// </summary>
    public string DisplayName => $"{ChineseName} ({EnglishName})";

    /// <summary>
    /// 获取完整显示信息
    /// </summary>
    public string FullDisplay => $"{DisplayName} - {Latitude:F4}, {Longitude:F4}";
}

/// <summary>
/// 位置管理器
/// </summary>
public class LocationManager
{
    /// <summary>
    /// 预设城市列表
    /// </summary>
    public static readonly IReadOnlyList<PresetLocation> Presets =
    [
        new("北京", "Beijing", 39.9042, 116.4074),
        new("上海", "Shanghai", 31.2304, 121.4737),
        new("广州", "Guangzhou", 23.1291, 113.2644),
        new("深圳", "Shenzhen", 22.5431, 114.0579),
        new("成都", "Chengdu", 30.5728, 104.0668),
        new("杭州", "Hangzhou", 30.2741, 120.1551),
        new("西安", "Xi'an", 34.3416, 108.9398),
        new("武汉", "Wuhan", 30.5928, 114.3055),
        new("重庆", "Chongqing", 29.4316, 106.9123),
        new("南京", "Nanjing", 32.0603, 118.7969)
    ];

    private PresetLocation? _currentPreset;
    private double _customLat;
    private double _customLon;
    private bool _isCustom;

    /// <summary>
    /// 当前位置查询对象
    /// </summary>
    public LocationQuery CurrentLocation { get; private set; } = null!;

    /// <summary>
    /// 当前位置名称
    /// </summary>
    public string CurrentLocationName { get; private set; } = null!;

    /// <summary>
    /// 当前纬度
    /// </summary>
    public double CurrentLatitude => _isCustom ? _customLat : _currentPreset?.Latitude ?? 0;

    /// <summary>
    /// 当前经度
    /// </summary>
    public double CurrentLongitude => _isCustom ? _customLon : _currentPreset?.Longitude ?? 0;

    /// <summary>
    /// 创建位置管理器，默认选择北京
    /// </summary>
    public LocationManager()
    {
        SelectPreset(0);
    }

    /// <summary>
    /// 选择预设位置
    /// </summary>
    /// <param name="index">预设位置索引（0-based）</param>
    /// <returns>是否选择成功</returns>
    public bool SelectPreset(int index)
    {
        if (index < 0 || index >= Presets.Count)
            return false;

        _currentPreset = Presets[index];
        _isCustom = false;
        CurrentLocation = _currentPreset.ToLocationQuery();
        CurrentLocationName = _currentPreset.DisplayName;
        return true;
    }

    /// <summary>
    /// 设置自定义坐标
    /// </summary>
    public bool SetCustomCoordinates(double lat, double lon)
    {
        if (lat is < -90 or > 90 || lon is < -180 or > 180)
            return false;

        _customLat = lat;
        _customLon = lon;
        _isCustom = true;
        _currentPreset = null;
        CurrentLocation = LocationQuery.FromCoordinates(lat, lon);
        CurrentLocationName = $"自定义 ({lat:F4}, {lon:F4})";
        return true;
    }

    /// <summary>
    /// 获取当前位置显示文本
    /// </summary>
    public string GetCurrentLocationDisplay()
    {
        return $"{CurrentLocationName} ({CurrentLatitude:F4}, {CurrentLongitude:F4})";
    }
}

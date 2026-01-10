namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// API端点Token配置
/// 包含所有端点的默认Token值，支持通过配置覆盖
/// </summary>
/// <remarks>
/// Token是墨迹天气API的端点标识符，不同于用户的AppCode凭证。
/// 默认值来自官方API文档，可通过配置文件或环境变量覆盖。
/// </remarks>
public sealed class EndpointTokens
{
    /// <summary>
    /// 经纬度查询的Token配置
    /// </summary>
    public CoordinatesTokens Coordinates { get; set; } = new();

    /// <summary>
    /// 城市ID查询的Token配置
    /// </summary>
    public CityIdTokens CityId { get; set; } = new();
}

/// <summary>
/// 共享端点Token基类
/// </summary>
/// <remarks>
/// 包含经纬度查询和城市ID查询共有的端点Token
/// </remarks>
public abstract class BaseEndpointTokens
{
    /// <summary>
    /// 精简AQI Token
    /// </summary>
    public string BriefAqi { get; set; } = "";

    /// <summary>
    /// 精简实况 Token
    /// </summary>
    public string BriefCondition { get; set; } = "";

    /// <summary>
    /// 精简预报3天 Token
    /// </summary>
    public string Forecast3Days { get; set; } = "";

    /// <summary>
    /// 天气预警 Token
    /// </summary>
    public string Alert { get; set; } = "";

    /// <summary>
    /// 空气质量指数 Token
    /// </summary>
    public string DetailedAqi { get; set; } = "";

    /// <summary>
    /// 精简预报6天 Token
    /// </summary>
    public string Forecast6Days { get; set; } = "";

    /// <summary>
    /// 限行数据 Token
    /// </summary>
    public string TrafficRestriction { get; set; } = "";

    /// <summary>
    /// AQI预报5天 Token
    /// </summary>
    public string AqiForecast5Days { get; set; } = "";

    /// <summary>
    /// 天气实况 Token
    /// </summary>
    public string DetailedCondition { get; set; } = "";

    /// <summary>
    /// 天气预报15天 Token
    /// </summary>
    public string Forecast15Days { get; set; } = "";

    /// <summary>
    /// 天气预报24小时 Token
    /// </summary>
    public string Forecast24Hours { get; set; } = "";

    /// <summary>
    /// 生活指数 Token
    /// </summary>
    public string LiveIndex { get; set; } = "";
}

/// <summary>
/// 经纬度查询方式的端点Token
/// </summary>
public sealed class CoordinatesTokens : BaseEndpointTokens
{
    /// <summary>
    /// 初始化经纬度查询Token，设置默认值
    /// </summary>
    public CoordinatesTokens()
    {
        BriefAqi = DefaultTokens.Coordinates.BriefAqi;
        BriefCondition = DefaultTokens.Coordinates.BriefCondition;
        Forecast3Days = DefaultTokens.Coordinates.Forecast3Days;
        Alert = DefaultTokens.Coordinates.Alert;
        DetailedAqi = DefaultTokens.Coordinates.DetailedAqi;
        Forecast6Days = DefaultTokens.Coordinates.Forecast6Days;
        TrafficRestriction = DefaultTokens.Coordinates.TrafficRestriction;
        AqiForecast5Days = DefaultTokens.Coordinates.AqiForecast5Days;
        DetailedCondition = DefaultTokens.Coordinates.DetailedCondition;
        Forecast15Days = DefaultTokens.Coordinates.Forecast15Days;
        Forecast24Hours = DefaultTokens.Coordinates.Forecast24Hours;
        LiveIndex = DefaultTokens.Coordinates.LiveIndex;
    }

    /// <summary>
    /// 短时预报 Token (仅经纬度查询支持)
    /// </summary>
    public string ShortForecast { get; set; } = DefaultTokens.Coordinates.ShortForecast;
}

/// <summary>
/// 城市ID查询方式的端点Token
/// </summary>
public sealed class CityIdTokens : BaseEndpointTokens
{
    /// <summary>
    /// 初始化城市ID查询Token，设置默认值
    /// </summary>
    public CityIdTokens()
    {
        BriefAqi = DefaultTokens.CityId.BriefAqi;
        BriefCondition = DefaultTokens.CityId.BriefCondition;
        Forecast3Days = DefaultTokens.CityId.Forecast3Days;
        Alert = DefaultTokens.CityId.Alert;
        DetailedAqi = DefaultTokens.CityId.DetailedAqi;
        Forecast6Days = DefaultTokens.CityId.Forecast6Days;
        TrafficRestriction = DefaultTokens.CityId.TrafficRestriction;
        AqiForecast5Days = DefaultTokens.CityId.AqiForecast5Days;
        DetailedCondition = DefaultTokens.CityId.DetailedCondition;
        Forecast15Days = DefaultTokens.CityId.Forecast15Days;
        Forecast24Hours = DefaultTokens.CityId.Forecast24Hours;
        LiveIndex = DefaultTokens.CityId.LiveIndex;
    }
}

/// <summary>
/// 默认Token值（来自官方API文档）
/// </summary>
/// <remarks>
/// 这些值可以通过配置文件覆盖:
/// <code>
/// {
///   "MojiWeather": {
///     "Tokens": {
///       "Coordinates": {
///         "BriefAqi": "your-custom-token"
///       }
///     }
///   }
/// }
/// </code>
/// </remarks>
internal static class DefaultTokens
{
    internal static class Coordinates
    {
        internal const string BriefAqi = "b87cea6abc73f77148534e03adff4d09";
        internal const string BriefCondition = "a231972c3e7ba6b33d8ec71fd4774f5e";
        internal const string Forecast3Days = "443847fa1ffd4e69d929807d42c2db1b";
        internal const string Alert = "d01246ac6284b5a591f875173e9e2a18";
        internal const string DetailedAqi = "6e9a127c311094245fc1b2aa6d0a54fd";
        internal const string Forecast6Days = "0f9d7e535dfbfad15b8fd2a84fee3e36";
        internal const string TrafficRestriction = "c712899b393c7b262dd7984f6eb52657";
        internal const string AqiForecast5Days = "17dbf48dff33b6228f3199dce7b9a6d6";
        internal const string DetailedCondition = "ff826c205f8f4a59701e64e9e64e01c4";
        internal const string Forecast15Days = "7538f7246218bdbf795b329ab09cc524";
        internal const string Forecast24Hours = "1b89050d9f64191d494c806f78e8ea36";
        internal const string LiveIndex = "42b0c7e2e8d00d6e80d92797fe5360fd";
        internal const string ShortForecast = "bbc0fdc738a3877f3f72f69b1a4d30fe";
    }

    internal static class CityId
    {
        internal const string BriefAqi = "4dc41ae4c14189b47b2dc00c85b9d124";
        internal const string BriefCondition = "46e13b7aab9bb77ee3358c3b672a2ae4";
        internal const string Forecast3Days = "677282c2f1b3d718152c4e25ed434bc4";
        internal const string Alert = "7ebe966ee2e04bbd8cdbc0b84f7f3bc7";
        internal const string DetailedAqi = "8b36edf8e3444047812be3a59d27bab9";
        internal const string Forecast6Days = "073854b56a84f8a4956ba3e273f6c9d7";
        internal const string TrafficRestriction = "27200005b3475f8b0e26428f9bfb13e9";
        internal const string AqiForecast5Days = "0418c1f4e5e66405d33556418189d2d0";
        internal const string DetailedCondition = "50b53ff8dd7d9fa320d3d3ca32cf8ed1";
        internal const string Forecast15Days = "f9f212e1996e79e0e602b08ea297ffb0";
        internal const string Forecast24Hours = "008d2ad9197090c5dddc76f583616606";
        internal const string LiveIndex = "5944a84ec4a071359cc4f6928b797f91";
    }
}

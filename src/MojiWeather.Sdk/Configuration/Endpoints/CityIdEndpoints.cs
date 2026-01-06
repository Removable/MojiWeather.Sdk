namespace MojiWeather.Sdk.Configuration.Endpoints;

/// <summary>
/// 城市ID查询方式的API端点配置
/// </summary>
public static class CityIdEndpoints
{
    private const string BasePath = "/whapi/json/alicityweather";

    /// <summary>
    /// 精简AQI (试用版+)
    /// </summary>
    public static readonly EndpointInfo BriefAqi = new(
        "精简AQI",
        "4dc41ae4c14189b47b2dc00c85b9d124",
        $"{BasePath}/briefaqi",
        SubscriptionTier.Trial);

    /// <summary>
    /// 精简实况 (试用版+)
    /// </summary>
    public static readonly EndpointInfo BriefCondition = new(
        "精简实况",
        "46e13b7aab9bb77ee3358c3b672a2ae4",
        $"{BasePath}/briefcondition",
        SubscriptionTier.Trial);

    /// <summary>
    /// 精简预报3天 (试用版+)
    /// </summary>
    public static readonly EndpointInfo Forecast3Days = new(
        "精简预报3天",
        "677282c2f1b3d718152c4e25ed434bc4",
        $"{BasePath}/forecast3days",
        SubscriptionTier.Trial);

    /// <summary>
    /// 天气预警 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo Alert = new(
        "天气预警",
        "7ebe966ee2e04bbd8cdbc0b84f7f3bc7",
        $"{BasePath}/alert",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 空气质量指数 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo DetailedAqi = new(
        "空气质量指数",
        "8b36edf8e3444047812be3a59d27bab9",
        $"{BasePath}/aqi",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 精简预报6天 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo Forecast6Days = new(
        "精简预报6天",
        "073854b56a84f8a4956ba3e273f6c9d7",
        $"{BasePath}/forecast6days",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 限行数据 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo TrafficRestriction = new(
        "限行数据",
        "27200005b3475f8b0e26428f9bfb13e9",
        $"{BasePath}/limit",
        SubscriptionTier.Pm25);

    /// <summary>
    /// AQI预报5天 (专业版)
    /// </summary>
    public static readonly EndpointInfo AqiForecast5Days = new(
        "AQI预报5天",
        "0418c1f4e5e66405d33556418189d2d0",
        $"{BasePath}/aqiforecast5days",
        SubscriptionTier.Professional);

    /// <summary>
    /// 天气实况 (专业版/基础版)
    /// </summary>
    public static readonly EndpointInfo DetailedCondition = new(
        "天气实况",
        "50b53ff8dd7d9fa320d3d3ca32cf8ed1",
        $"{BasePath}/condition",
        SubscriptionTier.Basic);

    /// <summary>
    /// 天气预报15天 (专业版)
    /// </summary>
    public static readonly EndpointInfo Forecast15Days = new(
        "天气预报15天",
        "f9f212e1996e79e0e602b08ea297ffb0",
        $"{BasePath}/forecast15days",
        SubscriptionTier.Professional);

    /// <summary>
    /// 天气预报24小时 (专业版)
    /// </summary>
    public static readonly EndpointInfo Forecast24Hours = new(
        "天气预报24小时",
        "008d2ad9197090c5dddc76f583616606",
        $"{BasePath}/forecast24hours",
        SubscriptionTier.Professional);

    /// <summary>
    /// 生活指数 (专业版)
    /// </summary>
    public static readonly EndpointInfo LiveIndex = new(
        "生活指数",
        "5944a84ec4a071359cc4f6928b797f91",
        $"{BasePath}/index",
        SubscriptionTier.Professional);
}

namespace MojiWeather.Sdk.Configuration.Endpoints;

/// <summary>
/// 经纬度查询方式的API端点配置
/// </summary>
public static class CoordinatesEndpoints
{
    private const string BasePath = "/whapi/json/alicityweather";

    /// <summary>
    /// 精简AQI (试用版+)
    /// </summary>
    public static readonly EndpointInfo BriefAqi = new(
        "精简AQI",
        "b87cea6abc73f77148534e03adff4d09",
        $"{BasePath}/briefaqi",
        SubscriptionTier.Trial);

    /// <summary>
    /// 精简实况 (试用版+)
    /// </summary>
    public static readonly EndpointInfo BriefCondition = new(
        "精简实况",
        "a231972c3e7ba6b33d8ec71fd4774f5e",
        $"{BasePath}/briefcondition",
        SubscriptionTier.Trial);

    /// <summary>
    /// 精简预报3天 (试用版+)
    /// </summary>
    public static readonly EndpointInfo Forecast3Days = new(
        "精简预报3天",
        "443847fa1ffd4e69d929807d42c2db1b",
        $"{BasePath}/forecast3days",
        SubscriptionTier.Trial);

    /// <summary>
    /// 天气预警 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo Alert = new(
        "天气预警",
        "d01246ac6284b5a591f875173e9e2a18",
        $"{BasePath}/alert",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 空气质量指数 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo DetailedAqi = new(
        "空气质量指数",
        "6e9a127c311094245fc1b2aa6d0a54fd",
        $"{BasePath}/aqi",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 精简预报6天 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo Forecast6Days = new(
        "精简预报6天",
        "0f9d7e535dfbfad15b8fd2a84fee3e36",
        $"{BasePath}/forecast6days",
        SubscriptionTier.Pm25);

    /// <summary>
    /// 限行数据 (PM2.5版+)
    /// </summary>
    public static readonly EndpointInfo TrafficRestriction = new(
        "限行数据",
        "c712899b393c7b262dd7984f6eb52657",
        $"{BasePath}/limit",
        SubscriptionTier.Pm25);

    /// <summary>
    /// AQI预报5天 (专业版)
    /// </summary>
    public static readonly EndpointInfo AqiForecast5Days = new(
        "AQI预报5天",
        "17dbf48dff33b6228f3199dce7b9a6d6",
        $"{BasePath}/aqiforecast5days",
        SubscriptionTier.Professional);

    /// <summary>
    /// 天气实况 (专业版)
    /// </summary>
    public static readonly EndpointInfo DetailedCondition = new(
        "天气实况",
        "ff826c205f8f4a59701e64e9e64e01c4",
        $"{BasePath}/condition",
        SubscriptionTier.Professional);

    /// <summary>
    /// 天气预报15天 (专业版)
    /// </summary>
    public static readonly EndpointInfo Forecast15Days = new(
        "天气预报15天",
        "7538f7246218bdbf795b329ab09cc524",
        $"{BasePath}/forecast15days",
        SubscriptionTier.Professional);

    /// <summary>
    /// 天气预报24小时 (专业版)
    /// </summary>
    public static readonly EndpointInfo Forecast24Hours = new(
        "天气预报24小时",
        "1b89050d9f64191d494c806f78e8ea36",
        $"{BasePath}/forecast24hours",
        SubscriptionTier.Professional);

    /// <summary>
    /// 生活指数 (专业版)
    /// </summary>
    public static readonly EndpointInfo LiveIndex = new(
        "生活指数",
        "42b0c7e2e8d00d6e80d92797fe5360fd",
        $"{BasePath}/index",
        SubscriptionTier.Professional);

    /// <summary>
    /// 短时预报 (专业版)
    /// </summary>
    public static readonly EndpointInfo ShortForecast = new(
        "短时预报",
        "bbc0fdc738a3877f3f72f69b1a4d30fe",
        $"{BasePath}/shortforecast",
        SubscriptionTier.Professional);
}

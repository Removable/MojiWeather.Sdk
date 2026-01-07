using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;

namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// 端点提供者实现 - 使用可配置的Token
/// </summary>
public sealed class EndpointProvider(IOptions<MojiWeatherOptions> options) : IEndpointProvider
{
    private readonly MojiWeatherOptions _options = options.Value;
    private readonly EndpointTokens _tokens = options.Value.Tokens;

    /// <inheritdoc />
    public EndpointInfo GetBriefAqi(LocationQuery location) =>
        CreateEndpoint(location, "精简AQI", "briefaqi", SubscriptionTier.Trial,
            t => t.BriefAqi, t => t.BriefAqi);

    /// <inheritdoc />
    public EndpointInfo GetBriefCondition(LocationQuery location) =>
        CreateEndpoint(location, "精简实况", "briefcondition", SubscriptionTier.Trial,
            t => t.BriefCondition, t => t.BriefCondition);

    /// <inheritdoc />
    public EndpointInfo GetForecast3Days(LocationQuery location) =>
        CreateEndpoint(location, "精简预报3天", "forecast3days", SubscriptionTier.Trial,
            t => t.Forecast3Days, t => t.Forecast3Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast6Days(LocationQuery location) =>
        CreateEndpoint(location, "精简预报6天", "forecast6days", SubscriptionTier.Pm25,
            t => t.Forecast6Days, t => t.Forecast6Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast15Days(LocationQuery location) =>
        CreateEndpoint(location, "天气预报15天", "forecast15days", SubscriptionTier.Professional,
            t => t.Forecast15Days, t => t.Forecast15Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast24Hours(LocationQuery location) =>
        CreateEndpoint(location, "天气预报24小时", "forecast24hours", SubscriptionTier.Professional,
            t => t.Forecast24Hours, t => t.Forecast24Hours);

    /// <inheritdoc />
    public EndpointInfo GetShortForecast()
    {
        var baseUrl = GetBaseUrl(SubscriptionTier.Professional, isCoordinates: true);
        var path = GetPath(isCoordinates: true, "shortforecast");
        return new EndpointInfo("短时预报", _tokens.Coordinates.ShortForecast, baseUrl, path, SubscriptionTier.Professional);
    }

    /// <inheritdoc />
    public EndpointInfo GetDetailedAqi(LocationQuery location) =>
        CreateEndpoint(location, "空气质量指数", "aqi", SubscriptionTier.Pm25,
            t => t.DetailedAqi, t => t.DetailedAqi);

    /// <inheritdoc />
    public EndpointInfo GetAqiForecast5Days(LocationQuery location) =>
        CreateEndpoint(location, "AQI预报5天", "aqiforecast5days", SubscriptionTier.Professional,
            t => t.AqiForecast5Days, t => t.AqiForecast5Days);

    /// <inheritdoc />
    public EndpointInfo GetAlert(LocationQuery location) =>
        CreateEndpoint(location, "天气预警", "alert", SubscriptionTier.Pm25,
            t => t.Alert, t => t.Alert);

    /// <inheritdoc />
    public EndpointInfo GetLiveIndex(LocationQuery location) =>
        CreateEndpoint(location, "生活指数", "index", SubscriptionTier.Professional,
            t => t.LiveIndex, t => t.LiveIndex);

    /// <inheritdoc />
    public EndpointInfo GetTrafficRestriction(LocationQuery location) =>
        CreateEndpoint(location, "限行数据", "limit", SubscriptionTier.Pm25,
            t => t.TrafficRestriction, t => t.TrafficRestriction);

    /// <inheritdoc />
    public EndpointInfo GetDetailedCondition(LocationQuery location) =>
        CreateEndpoint(location, "天气实况", "condition",
            coordTier: SubscriptionTier.Professional,
            cityIdTier: SubscriptionTier.Basic,
            t => t.DetailedCondition, t => t.DetailedCondition);

    /// <summary>
    /// 创建端点信息（相同订阅级别）
    /// </summary>
    private EndpointInfo CreateEndpoint(
        LocationQuery location,
        string name,
        string pathSuffix,
        SubscriptionTier tier,
        Func<CoordinatesTokens, string> coordTokenSelector,
        Func<CityIdTokens, string> cityIdTokenSelector) =>
        CreateEndpoint(location, name, pathSuffix, tier, tier, coordTokenSelector, cityIdTokenSelector);

    /// <summary>
    /// 创建端点信息（不同订阅级别）
    /// </summary>
    private EndpointInfo CreateEndpoint(
        LocationQuery location,
        string name,
        string pathSuffix,
        SubscriptionTier coordTier,
        SubscriptionTier cityIdTier,
        Func<CoordinatesTokens, string> coordTokenSelector,
        Func<CityIdTokens, string> cityIdTokenSelector)
    {
        var isCoordinates = location.IsCoordinatesQuery;
        var tier = isCoordinates ? coordTier : cityIdTier;
        var token = isCoordinates
            ? coordTokenSelector(_tokens.Coordinates)
            : cityIdTokenSelector(_tokens.CityId);
        var baseUrl = GetBaseUrl(tier, isCoordinates);
        var path = GetPath(isCoordinates, pathSuffix);

        return new EndpointInfo(name, token, baseUrl, path, tier);
    }

    /// <summary>
    /// 根据订阅级别和查询类型获取BaseUrl
    /// </summary>
    private string GetBaseUrl(SubscriptionTier tier, bool isCoordinates)
    {
        var useHttps = _options.UseHttps;

        return (tier, isCoordinates, useHttps) switch
        {
            // 试用版
            (SubscriptionTier.Trial, true, true) => "https://freelat.mojicb.com",
            (SubscriptionTier.Trial, true, false) => "http://apifreelat.market.alicloudapi.com",
            (SubscriptionTier.Trial, false, true) => "https://freecityid.mojicb.com",
            (SubscriptionTier.Trial, false, false) => "http://freecityid.market.alicloudapi.com",

            // PM2.5版
            (SubscriptionTier.Pm25, true, true) => "https://basiclat.mojicb.com",
            (SubscriptionTier.Pm25, true, false) => "http://mojibasic.market.alicloudapi.com",
            (SubscriptionTier.Pm25, false, true) => "https://basiccity.mojicb.com",
            (SubscriptionTier.Pm25, false, false) => "http://basiccity.market.alicloudapi.com",

            // 基础版
            (SubscriptionTier.Basic, true, true) => "https://aliv1.mojicb.com",
            (SubscriptionTier.Basic, true, false) => "http://aliv1.data.moji.com",
            (SubscriptionTier.Basic, false, true) => "https://aliv13.mojicb.com",
            (SubscriptionTier.Basic, false, false) => "http://aliv13.data.moji.com",

            // 专业版
            (SubscriptionTier.Professional, true, true) => "https://aliv8.mojicb.com",
            (SubscriptionTier.Professional, true, false) => "http://aliv8.data.moji.com",
            (SubscriptionTier.Professional, false, true) => "https://aliv18.mojicb.com",
            (SubscriptionTier.Professional, false, false) => "http://aliv18.data.moji.com",

            _ => throw new ArgumentException($"Unsupported tier: {tier}")
        };
    }

    /// <summary>
    /// 根据查询类型获取路径前缀
    /// </summary>
    private static string GetPath(bool isCoordinates, string pathSuffix)
    {
        var basePath = isCoordinates
            ? "/whapi/json/aliweather"
            : "/whapi/json/alicityweather";

        return $"{basePath}/{pathSuffix}";
    }
}

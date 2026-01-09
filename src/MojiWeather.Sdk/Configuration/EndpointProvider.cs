using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Exceptions;

namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// 端点提供者实现 - 使用可配置的Token
/// </summary>
public sealed class EndpointProvider(IOptions<MojiWeatherOptions> options) : IEndpointProvider
{
    private readonly MojiWeatherOptions _options = options.Value;
    private readonly EndpointTokens _tokens = options.Value.Tokens;

    /// <summary>
    /// 经纬度查询方式的接口可用性
    /// </summary>
    private static readonly Dictionary<string, SubscriptionTier[]> CoordEndpointAvailability = new()
    {
        ["briefaqi"] = [SubscriptionTier.Trial],
        ["briefcondition"] = [SubscriptionTier.Trial, SubscriptionTier.Pm25, SubscriptionTier.Basic],
        ["briefforecast3days"] = [SubscriptionTier.Trial],
        ["alert"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["aqi"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["briefforecast6days"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic],
        ["limit"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["aqiforecast5days"] = [SubscriptionTier.Professional],
        ["condition"] = [SubscriptionTier.Professional],
        ["forecast15days"] = [SubscriptionTier.Professional],
        ["forecast24hours"] = [SubscriptionTier.Professional],
        ["index"] = [SubscriptionTier.Professional],
        ["shortforecast"] = [SubscriptionTier.Professional],
    };

    /// <summary>
    /// CityID查询方式的接口可用性
    /// </summary>
    private static readonly Dictionary<string, SubscriptionTier[]> CityIdEndpointAvailability = new()
    {
        ["briefaqi"] = [SubscriptionTier.Trial],
        ["briefcondition"] = [SubscriptionTier.Trial, SubscriptionTier.Pm25],  // 注意：基础版CityID没有精简实况
        ["briefforecast3days"] = [SubscriptionTier.Trial],
        ["alert"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["aqi"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["briefforecast6days"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic],
        ["limit"] = [SubscriptionTier.Pm25, SubscriptionTier.Basic, SubscriptionTier.Professional],
        ["aqiforecast5days"] = [SubscriptionTier.Professional],
        ["condition"] = [SubscriptionTier.Basic, SubscriptionTier.Professional],  // 注意：基础版CityID有天气实况
        ["forecast15days"] = [SubscriptionTier.Professional],
        ["forecast24hours"] = [SubscriptionTier.Professional],
        ["index"] = [SubscriptionTier.Professional],
    };

    /// <inheritdoc />
    public EndpointInfo GetBriefAqi(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "精简AQI", "briefaqi",
            t => t.BriefAqi, t => t.BriefAqi);

    /// <inheritdoc />
    public EndpointInfo GetBriefCondition(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "精简实况", "briefcondition",
            t => t.BriefCondition, t => t.BriefCondition);

    /// <inheritdoc />
    public EndpointInfo GetForecast3Days(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "精简预报3天", "briefforecast3days",
            t => t.Forecast3Days, t => t.Forecast3Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast6Days(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "精简预报6天", "briefforecast6days",
            t => t.Forecast6Days, t => t.Forecast6Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast15Days(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "天气预报15天", "forecast15days",
            t => t.Forecast15Days, t => t.Forecast15Days);

    /// <inheritdoc />
    public EndpointInfo GetForecast24Hours(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "天气预报24小时", "forecast24hours",
            t => t.Forecast24Hours, t => t.Forecast24Hours);

    /// <inheritdoc />
    public EndpointInfo GetShortForecast()
    {
        var userTier = _options.Tier;
        const string pathSuffix = "shortforecast";
        const string name = "短时预报";

        // 短时预报只支持经纬度查询
        if (!CoordEndpointAvailability.TryGetValue(pathSuffix, out var availableTiers))
        {
            throw new InvalidOperationException($"未知的接口路径: {pathSuffix}");
        }

        if (!availableTiers.Contains(userTier))
        {
            throw new SubscriptionTierNotSupportedException(name, "经纬度", userTier, availableTiers);
        }

        var baseUrl = GetBaseUrl(userTier, isCoordinates: true);
        var path = GetPath(isCoordinates: true, pathSuffix);
        return new EndpointInfo(name, _tokens.Coordinates.ShortForecast, baseUrl, path, userTier);
    }

    /// <inheritdoc />
    public EndpointInfo GetDetailedAqi(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "空气质量指数", "aqi",
            t => t.DetailedAqi, t => t.DetailedAqi);

    /// <inheritdoc />
    public EndpointInfo GetAqiForecast5Days(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "AQI预报5天", "aqiforecast5days",
            t => t.AqiForecast5Days, t => t.AqiForecast5Days);

    /// <inheritdoc />
    public EndpointInfo GetAlert(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "天气预警", "alert",
            t => t.Alert, t => t.Alert);

    /// <inheritdoc />
    public EndpointInfo GetLiveIndex(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "生活指数", "index",
            t => t.LiveIndex, t => t.LiveIndex);

    /// <inheritdoc />
    public EndpointInfo GetTrafficRestriction(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "限行数据", "limit",
            t => t.TrafficRestriction, t => t.TrafficRestriction);

    /// <inheritdoc />
    public EndpointInfo GetDetailedCondition(LocationQuery location) =>
        CreateEndpointWithUserTier(location, "天气实况", "condition",
            t => t.DetailedCondition, t => t.DetailedCondition);

    /// <summary>
    /// 创建端点信息 - 使用用户配置的订阅级别
    /// </summary>
    private EndpointInfo CreateEndpointWithUserTier(
        LocationQuery location,
        string name,
        string pathSuffix,
        Func<CoordinatesTokens, string> coordTokenSelector,
        Func<CityIdTokens, string> cityIdTokenSelector)
    {
        var userTier = _options.Tier;
        var isCoordinates = location.IsCoordinatesQuery;

        // 根据查询方式选择可用性字典
        var availability = isCoordinates ? CoordEndpointAvailability : CityIdEndpointAvailability;

        // 检查接口是否存在于可用性字典中
        if (!availability.TryGetValue(pathSuffix, out var availableTiers))
        {
            throw new InvalidOperationException($"未知的接口路径: {pathSuffix}");
        }

        // 验证用户的订阅级别是否支持该接口
        if (!availableTiers.Contains(userTier))
        {
            var queryType = isCoordinates ? "经纬度" : "CityID";
            throw new SubscriptionTierNotSupportedException(name, queryType, userTier, availableTiers);
        }

        var token = isCoordinates
            ? coordTokenSelector(_tokens.Coordinates)
            : cityIdTokenSelector(_tokens.CityId);
        var baseUrl = GetBaseUrl(userTier, isCoordinates);
        var path = GetPath(isCoordinates, pathSuffix);

        return new EndpointInfo(name, token, baseUrl, path, userTier);
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

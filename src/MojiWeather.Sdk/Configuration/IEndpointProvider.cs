using MojiWeather.Sdk.Abstractions;

namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// 端点提供者接口 - 根据位置查询类型返回正确的端点配置
/// </summary>
public interface IEndpointProvider
{
    /// <summary>
    /// 获取精简AQI端点
    /// </summary>
    EndpointInfo GetBriefAqi(LocationQuery location);

    /// <summary>
    /// 获取精简实况端点
    /// </summary>
    EndpointInfo GetBriefCondition(LocationQuery location);

    /// <summary>
    /// 获取精简预报3天端点
    /// </summary>
    EndpointInfo GetForecast3Days(LocationQuery location);

    /// <summary>
    /// 获取精简预报6天端点
    /// </summary>
    EndpointInfo GetForecast6Days(LocationQuery location);

    /// <summary>
    /// 获取天气预报15天端点
    /// </summary>
    EndpointInfo GetForecast15Days(LocationQuery location);

    /// <summary>
    /// 获取天气预报24小时端点
    /// </summary>
    EndpointInfo GetForecast24Hours(LocationQuery location);

    /// <summary>
    /// 获取短时预报端点（仅经纬度查询）
    /// </summary>
    EndpointInfo GetShortForecast();

    /// <summary>
    /// 获取详细AQI端点
    /// </summary>
    EndpointInfo GetDetailedAqi(LocationQuery location);

    /// <summary>
    /// 获取AQI预报5天端点
    /// </summary>
    EndpointInfo GetAqiForecast5Days(LocationQuery location);

    /// <summary>
    /// 获取天气预警端点
    /// </summary>
    EndpointInfo GetAlert(LocationQuery location);

    /// <summary>
    /// 获取生活指数端点
    /// </summary>
    EndpointInfo GetLiveIndex(LocationQuery location);

    /// <summary>
    /// 获取限行数据端点
    /// </summary>
    EndpointInfo GetTrafficRestriction(LocationQuery location);

    /// <summary>
    /// 获取精简实况端点
    /// </summary>
    EndpointInfo GetDetailedCondition(LocationQuery location);
}

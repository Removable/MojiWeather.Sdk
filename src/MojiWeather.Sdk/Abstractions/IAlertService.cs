using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Abstractions;

/// <summary>
/// 天气预警服务接口
/// </summary>
public interface IAlertService
{
    /// <summary>
    /// 获取当前有效预警
    /// </summary>
    Task<ApiResponse<WeatherAlertData>> GetActiveAlertsAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default);
}

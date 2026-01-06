using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration.Endpoints;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 天气预警服务实现
/// </summary>
public sealed class AlertService(IMojiHttpClient httpClient) : IAlertService
{
    /// <inheritdoc />
    public async Task<ApiResponse<WeatherAlertData>> GetActiveAlertsAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.Alert
            : CityIdEndpoints.Alert;

        return await httpClient.SendAsync<WeatherAlertData>(endpoint, location, cancellationToken: cancellationToken);
    }
}

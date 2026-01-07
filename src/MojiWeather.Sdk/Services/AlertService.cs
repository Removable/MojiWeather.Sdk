using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 天气预警服务实现
/// </summary>
public sealed class AlertService(
    IMojiHttpClient httpClient,
    IEndpointProvider endpointProvider) : IAlertService
{
    /// <inheritdoc />
    public async Task<ApiResponse<WeatherAlertData>> GetActiveAlertsAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetAlert(location);
        return await httpClient.SendAsync<WeatherAlertData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 空气质量服务实现
/// </summary>
public sealed class AirQualityService(
    IMojiHttpClient httpClient,
    IEndpointProvider endpointProvider) : IAirQualityService
{
    /// <inheritdoc />
    public async Task<ApiResponse<BriefAqiData>> GetBriefAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetBriefAqi(location);
        return await httpClient.SendAsync<BriefAqiData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DetailedAqiData>> GetDetailedAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetDetailedAqi(location);
        return await httpClient.SendAsync<DetailedAqiData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AqiForecastData>> GetAqiForecast5DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetAqiForecast5Days(location);
        return await httpClient.SendAsync<AqiForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

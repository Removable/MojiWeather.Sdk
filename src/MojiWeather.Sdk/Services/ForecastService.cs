using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 天气预报服务实现
/// </summary>
public sealed class ForecastService(
    IMojiHttpClient httpClient,
    IEndpointProvider endpointProvider) : IForecastService
{
    /// <inheritdoc />
    public async Task<ApiResponse<BriefDailyForecastData>> GetBriefForecast3DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetForecast3Days(location);
        return await httpClient.SendAsync<BriefDailyForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<BriefDailyForecastData>> GetBriefForecast6DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetForecast6Days(location);
        return await httpClient.SendAsync<BriefDailyForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DailyForecastData>> GetForecast15DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetForecast15Days(location);
        return await httpClient.SendAsync<DailyForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<HourlyForecastData>> GetForecast24HoursAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetForecast24Hours(location);
        return await httpClient.SendAsync<HourlyForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ShortForecastData>> GetShortForecastAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        // 短时预报仅支持经纬度查询
        if (!location.IsCoordinatesQuery)
        {
            return ApiResponse<ShortForecastData>.Failure(-1, "Short forecast only supports coordinates query.");
        }

        var endpoint = endpointProvider.GetShortForecast();
        return await httpClient.SendAsync<ShortForecastData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

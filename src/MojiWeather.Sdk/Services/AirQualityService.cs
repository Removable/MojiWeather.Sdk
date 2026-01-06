using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration.Endpoints;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 空气质量服务实现
/// </summary>
public sealed class AirQualityService(IMojiHttpClient httpClient) : IAirQualityService
{
    /// <inheritdoc />
    public async Task<ApiResponse<BriefAqiData>> GetBriefAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.BriefAqi
            : CityIdEndpoints.BriefAqi;

        return await httpClient.SendAsync<BriefAqiData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DetailedAqiData>> GetDetailedAqiAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.DetailedAqi
            : CityIdEndpoints.DetailedAqi;

        return await httpClient.SendAsync<DetailedAqiData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AqiForecastData>> GetAqiForecast5DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.AqiForecast5Days
            : CityIdEndpoints.AqiForecast5Days;

        return await httpClient.SendAsync<AqiForecastData>(endpoint, location, cancellationToken: cancellationToken);
    }
}

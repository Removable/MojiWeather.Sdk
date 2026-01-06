using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration.Endpoints;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 天气预报服务实现
/// </summary>
public sealed class ForecastService(IMojiHttpClient httpClient) : IForecastService
{
    /// <inheritdoc />
    public async Task<ApiResponse<DailyForecastData>> GetForecast3DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.Forecast3Days
            : CityIdEndpoints.Forecast3Days;

        return await httpClient.SendAsync<DailyForecastData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DailyForecastData>> GetForecast6DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.Forecast6Days
            : CityIdEndpoints.Forecast6Days;

        return await httpClient.SendAsync<DailyForecastData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DailyForecastData>> GetForecast15DaysAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.Forecast15Days
            : CityIdEndpoints.Forecast15Days;

        return await httpClient.SendAsync<DailyForecastData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<HourlyForecastData>> GetForecast24HoursAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.Forecast24Hours
            : CityIdEndpoints.Forecast24Hours;

        return await httpClient.SendAsync<HourlyForecastData>(endpoint, location, cancellationToken: cancellationToken);
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

        return await httpClient.SendAsync<ShortForecastData>(
            CoordinatesEndpoints.ShortForecast, location, cancellationToken: cancellationToken);
    }
}

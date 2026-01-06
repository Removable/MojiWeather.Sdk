using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Configuration.Endpoints;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 天气实况服务实现
/// </summary>
public sealed class WeatherService(IMojiHttpClient httpClient) : IWeatherService
{
    /// <inheritdoc />
    public async Task<ApiResponse<BriefConditionData>> GetBriefConditionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.BriefCondition
            : CityIdEndpoints.BriefCondition;

        return await httpClient.SendAsync<BriefConditionData>(endpoint, location, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<DetailedConditionData>> GetDetailedConditionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = location.IsCoordinatesQuery
            ? CoordinatesEndpoints.DetailedCondition
            : CityIdEndpoints.DetailedCondition;

        return await httpClient.SendAsync<DetailedConditionData>(endpoint, location, cancellationToken: cancellationToken);
    }
}

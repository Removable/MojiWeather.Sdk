using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Traffic;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 限行数据服务实现
/// </summary>
public sealed class TrafficService(
    IMojiHttpClient httpClient,
    IEndpointProvider endpointProvider) : ITrafficService
{
    /// <inheritdoc />
    public async Task<ApiResponse<TrafficRestrictionData>> GetRestrictionAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetTrafficRestriction(location);
        return await httpClient.SendAsync<TrafficRestrictionData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

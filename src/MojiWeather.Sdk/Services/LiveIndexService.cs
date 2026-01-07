using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;

namespace MojiWeather.Sdk.Services;

/// <summary>
/// 生活指数服务实现
/// </summary>
public sealed class LiveIndexService(
    IMojiHttpClient httpClient,
    IEndpointProvider endpointProvider) : ILiveIndexService
{
    /// <inheritdoc />
    public async Task<ApiResponse<LiveIndexData>> GetLiveIndexAsync(
        LocationQuery location,
        CancellationToken cancellationToken = default)
    {
        var endpoint = endpointProvider.GetLiveIndex(location);
        return await httpClient.SendAsync<LiveIndexData>(endpoint, location, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}

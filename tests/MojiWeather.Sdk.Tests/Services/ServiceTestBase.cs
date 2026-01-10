using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using NSubstitute;

namespace MojiWeather.Sdk.Tests.Services;

public abstract class ServiceTestBase<TService>
{
    protected IMojiHttpClient HttpClient { get; }
    protected IEndpointProvider EndpointProvider { get; }
    protected TService Service { get; }

    protected ServiceTestBase()
    {
        HttpClient = Substitute.For<IMojiHttpClient>();
        EndpointProvider = Substitute.For<IEndpointProvider>();
        Service = CreateService();
    }

    protected abstract TService CreateService();

    protected static LocationQuery CreateCoordinatesLocation() => LocationQuery.FromCoordinates(39.9, 116.4);

    protected static LocationQuery CreateCityLocation() => LocationQuery.FromCityId(101010100);

    protected void SetupResponse<TData>(
        EndpointInfo endpoint,
        LocationQuery location,
        ApiResponse<TData> response,
        CancellationToken? cancellationToken = null) where TData : class
    {
        if (cancellationToken is null)
        {
            HttpClient.SendAsync<TData>(endpoint, location, null, Arg.Any<CancellationToken>())
                .Returns(response);
            return;
        }

        HttpClient.SendAsync<TData>(endpoint, location, null, cancellationToken.Value)
            .Returns(response);
    }

    protected Task VerifySendAsyncCalled<TData>(
        EndpointInfo endpoint,
        LocationQuery location,
        CancellationToken? cancellationToken = null) where TData : class
    {
        return cancellationToken is null
            ? HttpClient.Received(1).SendAsync<TData>(endpoint, location, null, Arg.Any<CancellationToken>())
            : HttpClient.Received(1).SendAsync<TData>(endpoint, location, null, cancellationToken.Value);
    }
}

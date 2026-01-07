using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class LiveIndexServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly LiveIndexService _service;

    public LiveIndexServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new LiveIndexService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetLiveIndexAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        _endpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<LiveIndexData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetLiveIndexAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetLiveIndex(location);
        await _httpClient.Received(1).SendAsync<LiveIndexData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetLiveIndexAsync_WithCityIdQuery_ShouldWork()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        _endpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<LiveIndexData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetLiveIndexAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetLiveIndex(location);
    }

    [Fact]
    public async Task GetLiveIndexAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        _endpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<LiveIndexData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetLiveIndexAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<LiveIndexData>(
            expectedEndpoint, location, null, cts.Token);
    }
}

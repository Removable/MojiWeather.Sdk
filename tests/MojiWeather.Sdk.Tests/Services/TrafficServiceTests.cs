using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Traffic;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class TrafficServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly TrafficService _service;

    public TrafficServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new TrafficService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetRestrictionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        _endpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<TrafficRestrictionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetRestrictionAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetTrafficRestriction(location);
        await _httpClient.Received(1).SendAsync<TrafficRestrictionData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetRestrictionAsync_WithCityIdQuery_ShouldWork()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        _endpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<TrafficRestrictionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetRestrictionAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetTrafficRestriction(location);
    }

    [Fact]
    public async Task GetRestrictionAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        _endpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<TrafficRestrictionData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetRestrictionAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<TrafficRestrictionData>(
            expectedEndpoint, location, null, cts.Token);
    }
}

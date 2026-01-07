using FluentAssertions;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class WeatherServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly WeatherService _service;

    public WeatherServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new WeatherService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetBriefConditionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        _endpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefConditionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefConditionAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetBriefCondition(location);
        await _httpClient.Received(1).SendAsync<BriefConditionData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBriefConditionAsync_WithCityIdQuery_ShouldWork()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        _endpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefConditionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefConditionAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetDetailedConditionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<DetailedConditionData>.Success(new DetailedConditionData());

        _endpointProvider.GetDetailedCondition(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<DetailedConditionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetDetailedConditionAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetDetailedCondition(location);
    }

    [Fact]
    public async Task GetBriefConditionAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        _endpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefConditionData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefConditionAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<BriefConditionData>(
            expectedEndpoint, location, null, cts.Token);
    }
}

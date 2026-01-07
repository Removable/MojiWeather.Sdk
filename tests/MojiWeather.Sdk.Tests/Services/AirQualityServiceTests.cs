using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class AirQualityServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly AirQualityService _service;

    public AirQualityServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new AirQualityService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetBriefAqiAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        _endpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefAqiData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefAqiAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetBriefAqi(location);
        await _httpClient.Received(1).SendAsync<BriefAqiData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBriefAqiAsync_WithCityIdQuery_ShouldWork()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        _endpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefAqiData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefAqiAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetBriefAqi(location);
    }

    [Fact]
    public async Task GetBriefAqiAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        _endpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefAqiData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefAqiAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<BriefAqiData>(
            expectedEndpoint, location, null, cts.Token);
    }

    [Fact]
    public async Task GetDetailedAqiAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("DetailedAqi", "token", "https://test.api.com", "/test/aqi/detailed", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<DetailedAqiData>.Success(new DetailedAqiData());

        _endpointProvider.GetDetailedAqi(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<DetailedAqiData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetDetailedAqiAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetDetailedAqi(location);
        await _httpClient.Received(1).SendAsync<DetailedAqiData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetDetailedAqiAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("DetailedAqi", "token", "https://test.api.com", "/test/aqi/detailed", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<DetailedAqiData>.Success(new DetailedAqiData());

        _endpointProvider.GetDetailedAqi(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<DetailedAqiData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetDetailedAqiAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<DetailedAqiData>(
            expectedEndpoint, location, null, cts.Token);
    }

    [Fact]
    public async Task GetAqiForecast5DaysAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("AqiForecast5Days", "token", "https://test.api.com", "/test/aqi/forecast", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<AqiForecastData>.Success(new AqiForecastData());

        _endpointProvider.GetAqiForecast5Days(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<AqiForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetAqiForecast5DaysAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetAqiForecast5Days(location);
        await _httpClient.Received(1).SendAsync<AqiForecastData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAqiForecast5DaysAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("AqiForecast5Days", "token", "https://test.api.com", "/test/aqi/forecast", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<AqiForecastData>.Success(new AqiForecastData());

        _endpointProvider.GetAqiForecast5Days(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<AqiForecastData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetAqiForecast5DaysAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<AqiForecastData>(
            expectedEndpoint, location, null, cts.Token);
    }
}

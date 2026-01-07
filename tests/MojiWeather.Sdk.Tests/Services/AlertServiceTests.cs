using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class AlertServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly AlertService _service;

    public AlertServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new AlertService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetActiveAlertsAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        _endpointProvider.GetAlert(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<WeatherAlertData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetActiveAlertsAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetAlert(location);
        await _httpClient.Received(1).SendAsync<WeatherAlertData>(
            expectedEndpoint, location, null, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WithCityIdQuery_ShouldWork()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        _endpointProvider.GetAlert(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<WeatherAlertData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetActiveAlertsAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetAlert(location);
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        _endpointProvider.GetAlert(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<WeatherAlertData>(expectedEndpoint, location, null, cts.Token)
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetActiveAlertsAsync(location, cts.Token);

        // Assert
        await _httpClient.Received(1).SendAsync<WeatherAlertData>(
            expectedEndpoint, location, null, cts.Token);
    }
}

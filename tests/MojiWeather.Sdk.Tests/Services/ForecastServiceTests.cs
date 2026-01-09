using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class ForecastServiceTests
{
    private readonly IMojiHttpClient _httpClient;
    private readonly IEndpointProvider _endpointProvider;
    private readonly ForecastService _service;

    public ForecastServiceTests()
    {
        _httpClient = Substitute.For<IMojiHttpClient>();
        _endpointProvider = Substitute.For<IEndpointProvider>();
        _service = new ForecastService(_httpClient, _endpointProvider);
    }

    [Fact]
    public async Task GetForecast3DaysAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("3天预报", "token", "https://test.api.com", "/forecast3days", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefDailyForecastData>.Success(new BriefDailyForecastData());

        _endpointProvider.GetForecast3Days(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefDailyForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefForecast3DaysAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetForecast3Days(location);
    }

    [Fact]
    public async Task GetForecast6DaysAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("6天预报", "token", "https://test.api.com", "/forecast6days", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<BriefDailyForecastData>.Success(new BriefDailyForecastData());

        _endpointProvider.GetForecast6Days(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<BriefDailyForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetBriefForecast6DaysAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetForecast6Days(location);
    }

    [Fact]
    public async Task GetForecast15DaysAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("15天预报", "token", "https://test.api.com", "/forecast15days", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<DailyForecastData>.Success(new DailyForecastData());

        _endpointProvider.GetForecast15Days(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<DailyForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetForecast15DaysAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetForecast15Days(location);
    }

    [Fact]
    public async Task GetForecast24HoursAsync_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("24小时预报", "token", "https://test.api.com", "/forecast24hours", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<HourlyForecastData>.Success(new HourlyForecastData());

        _endpointProvider.GetForecast24Hours(location).Returns(expectedEndpoint);
        _httpClient.SendAsync<HourlyForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetForecast24HoursAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetForecast24Hours(location);
    }

    [Fact]
    public async Task GetShortForecastAsync_WithCoordinates_ShouldCallCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var expectedEndpoint = new EndpointInfo("短时预报", "token", "https://test.api.com", "/shortforecast", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<ShortForecastData>.Success(new ShortForecastData());

        _endpointProvider.GetShortForecast().Returns(expectedEndpoint);
        _httpClient.SendAsync<ShortForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(expectedResponse);

        // Act
        var result = await _service.GetShortForecastAsync(location);

        // Assert
        result.Should().Be(expectedResponse);
        _endpointProvider.Received(1).GetShortForecast();
    }

    [Fact]
    public async Task GetShortForecastAsync_WithCityId_ShouldReturnFailure()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);

        // Act
        var result = await _service.GetShortForecastAsync(location);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(-1);
        result.Message.Should().Contain("coordinates query");

        // Verify http client was never called
        await _httpClient.DidNotReceive().SendAsync<ShortForecastData>(
            Arg.Any<EndpointInfo>(), Arg.Any<LocationQuery>(),
            Arg.Any<IReadOnlyDictionary<string, string>?>(), Arg.Any<CancellationToken>());
    }
}

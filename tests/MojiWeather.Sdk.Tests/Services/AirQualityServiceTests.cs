using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class AirQualityServiceTests : ServiceTestBase<AirQualityService>
{
    protected override AirQualityService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetBriefAqiAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        EndpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefAqiAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetBriefAqi(location);
        await VerifySendAsyncCalled<BriefAqiData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetBriefAqiAsync_WithCityIdQuery_ShouldWork()
    {
        // 准备
        var location = CreateCityLocation();
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        EndpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefAqiAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetBriefAqi(location);
    }

    [Fact]
    public async Task GetBriefAqiAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<BriefAqiData>.Success(new BriefAqiData());

        EndpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetBriefAqiAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<BriefAqiData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetDetailedAqiAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("DetailedAqi", "token", "https://test.api.com", "/test/aqi/detailed", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<DetailedAqiData>.Success(new DetailedAqiData());

        EndpointProvider.GetDetailedAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetDetailedAqiAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetDetailedAqi(location);
        await VerifySendAsyncCalled<DetailedAqiData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetDetailedAqiAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("DetailedAqi", "token", "https://test.api.com", "/test/aqi/detailed", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<DetailedAqiData>.Success(new DetailedAqiData());

        EndpointProvider.GetDetailedAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetDetailedAqiAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<DetailedAqiData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetAqiForecast5DaysAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("AqiForecast5Days", "token", "https://test.api.com", "/test/aqi/forecast", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<AqiForecastData>.Success(new AqiForecastData());

        EndpointProvider.GetAqiForecast5Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetAqiForecast5DaysAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetAqiForecast5Days(location);
        await VerifySendAsyncCalled<AqiForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetAqiForecast5DaysAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("AqiForecast5Days", "token", "https://test.api.com", "/test/aqi/forecast", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<AqiForecastData>.Success(new AqiForecastData());

        EndpointProvider.GetAqiForecast5Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetAqiForecast5DaysAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<AqiForecastData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetBriefAqiAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<BriefAqiData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<BriefAqiData>>(exception));

        // 执行
        var action = () => Service.GetBriefAqiAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetBriefAqiAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "精简AQI",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Professional });

        EndpointProvider.When(provider => provider.GetBriefAqi(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetBriefAqiAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*精简AQI*");
    }

    [Fact]
    public async Task GetBriefAqiAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("BriefAqi", "token", "https://test.api.com", "/test/aqi", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefAqiData>.Failure(400, "失败");

        EndpointProvider.GetBriefAqi(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefAqiAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(400);
        result.Message.Should().Be("失败");
    }
}

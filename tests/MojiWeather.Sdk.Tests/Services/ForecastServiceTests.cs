using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class ForecastServiceTests : ServiceTestBase<ForecastService>
{
    protected override ForecastService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetForecast3DaysAsync_ShouldCallCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("3天预报", "token", "https://test.api.com", "/forecast3days", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefDailyForecastData>.Success(new BriefDailyForecastData());

        EndpointProvider.GetForecast3Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefForecast3DaysAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetForecast3Days(location);
        await VerifySendAsyncCalled<BriefDailyForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetForecast6DaysAsync_ShouldCallCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("6天预报", "token", "https://test.api.com", "/forecast6days", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<BriefDailyForecastData>.Success(new BriefDailyForecastData());

        EndpointProvider.GetForecast6Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefForecast6DaysAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetForecast6Days(location);
        await VerifySendAsyncCalled<BriefDailyForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetForecast15DaysAsync_ShouldCallCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("15天预报", "token", "https://test.api.com", "/forecast15days", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<DailyForecastData>.Success(new DailyForecastData());

        EndpointProvider.GetForecast15Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetForecast15DaysAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetForecast15Days(location);
        await VerifySendAsyncCalled<DailyForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetForecast24HoursAsync_ShouldCallCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("24小时预报", "token", "https://test.api.com", "/forecast24hours", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<HourlyForecastData>.Success(new HourlyForecastData());

        EndpointProvider.GetForecast24Hours(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetForecast24HoursAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetForecast24Hours(location);
        await VerifySendAsyncCalled<HourlyForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetShortForecastAsync_WithCoordinates_ShouldCallCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("短时预报", "token", "https://test.api.com", "/shortforecast", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<ShortForecastData>.Success(new ShortForecastData());

        EndpointProvider.GetShortForecast().Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetShortForecastAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetShortForecast();
        await VerifySendAsyncCalled<ShortForecastData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetShortForecastAsync_WithCityId_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCityLocation();

        // 执行
        var result = await Service.GetShortForecastAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(-1);
        result.Message.Should().Contain("coordinates query");

        await HttpClient.DidNotReceive().SendAsync<ShortForecastData>(
            Arg.Any<EndpointInfo>(), Arg.Any<LocationQuery>(),
            Arg.Any<IReadOnlyDictionary<string, string>?>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetBriefForecast3DaysAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("3天预报", "token", "https://test.api.com", "/forecast3days", SubscriptionTier.Trial);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetForecast3Days(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<BriefDailyForecastData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<BriefDailyForecastData>>(exception));

        // 执行
        var action = () => Service.GetBriefForecast3DaysAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetBriefForecast3DaysAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "精简预报3天",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Pm25 });

        EndpointProvider.When(provider => provider.GetForecast3Days(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetBriefForecast3DaysAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*精简预报3天*");
    }

    [Fact]
    public async Task GetBriefForecast3DaysAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("3天预报", "token", "https://test.api.com", "/forecast3days", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefDailyForecastData>.Failure(500, "失败");

        EndpointProvider.GetForecast3Days(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefForecast3DaysAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(500);
        result.Message.Should().Be("失败");
    }
}

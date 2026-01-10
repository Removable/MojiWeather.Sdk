using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class AlertServiceTests : ServiceTestBase<AlertService>
{
    protected override AlertService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetActiveAlertsAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        EndpointProvider.GetAlert(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetActiveAlertsAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetAlert(location);
        await VerifySendAsyncCalled<WeatherAlertData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WithCityIdQuery_ShouldWork()
    {
        // 准备
        var location = CreateCityLocation();
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        EndpointProvider.GetAlert(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetActiveAlertsAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetAlert(location);
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<WeatherAlertData>.Success(new WeatherAlertData());

        EndpointProvider.GetAlert(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetActiveAlertsAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<WeatherAlertData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetAlert(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<WeatherAlertData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<WeatherAlertData>>(exception));

        // 执行
        var action = () => Service.GetActiveAlertsAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "天气预警",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Pm25 });

        EndpointProvider.When(provider => provider.GetAlert(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetActiveAlertsAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*天气预警*");
    }

    [Fact]
    public async Task GetActiveAlertsAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Alert", "token", "https://test.api.com", "/test/alert", SubscriptionTier.Pm25);
        var expectedResponse = ApiResponse<WeatherAlertData>.Failure(404, "失败");

        EndpointProvider.GetAlert(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetActiveAlertsAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(404);
        result.Message.Should().Be("失败");
    }
}

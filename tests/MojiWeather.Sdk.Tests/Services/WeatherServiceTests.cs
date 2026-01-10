using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class WeatherServiceTests : ServiceTestBase<WeatherService>
{
    protected override WeatherService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetBriefConditionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        EndpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefConditionAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetBriefCondition(location);
        await VerifySendAsyncCalled<BriefConditionData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetBriefConditionAsync_WithCityIdQuery_ShouldWork()
    {
        // 准备
        var location = CreateCityLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        EndpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefConditionAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetDetailedConditionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<DetailedConditionData>.Success(new DetailedConditionData());

        EndpointProvider.GetDetailedCondition(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetDetailedConditionAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetDetailedCondition(location);
    }

    [Fact]
    public async Task GetBriefConditionAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<BriefConditionData>.Success(new BriefConditionData());

        EndpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetBriefConditionAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<BriefConditionData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetBriefConditionAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<BriefConditionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<BriefConditionData>>(exception));

        // 执行
        var action = () => Service.GetBriefConditionAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetBriefConditionAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "精简实况",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Professional });

        EndpointProvider.When(provider => provider.GetBriefCondition(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetBriefConditionAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*精简实况*");
    }

    [Fact]
    public async Task GetBriefConditionAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Test", "token", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var expectedResponse = ApiResponse<BriefConditionData>.Failure(500, "失败");

        EndpointProvider.GetBriefCondition(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetBriefConditionAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(500);
        result.Message.Should().Be("失败");
    }
}

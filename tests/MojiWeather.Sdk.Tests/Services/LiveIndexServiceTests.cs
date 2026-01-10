using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class LiveIndexServiceTests : ServiceTestBase<LiveIndexService>
{
    protected override LiveIndexService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetLiveIndexAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        EndpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetLiveIndexAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetLiveIndex(location);
        await VerifySendAsyncCalled<LiveIndexData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetLiveIndexAsync_WithCityIdQuery_ShouldWork()
    {
        // 准备
        var location = CreateCityLocation();
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        EndpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetLiveIndexAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetLiveIndex(location);
    }

    [Fact]
    public async Task GetLiveIndexAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<LiveIndexData>.Success(new LiveIndexData());

        EndpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetLiveIndexAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<LiveIndexData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetLiveIndexAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<LiveIndexData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<LiveIndexData>>(exception));

        // 执行
        var action = () => Service.GetLiveIndexAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetLiveIndexAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "生活指数",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Professional });

        EndpointProvider.When(provider => provider.GetLiveIndex(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetLiveIndexAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*生活指数*");
    }

    [Fact]
    public async Task GetLiveIndexAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("LiveIndex", "token", "https://test.api.com", "/test/index", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<LiveIndexData>.Failure(503, "失败");

        EndpointProvider.GetLiveIndex(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetLiveIndexAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(503);
        result.Message.Should().Be("失败");
    }
}

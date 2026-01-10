using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Traffic;
using MojiWeather.Sdk.Services;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Services;

public class TrafficServiceTests : ServiceTestBase<TrafficService>
{
    protected override TrafficService CreateService() => new(HttpClient, EndpointProvider);

    [Fact]
    public async Task GetRestrictionAsync_ShouldCallHttpClientWithCorrectEndpoint()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        EndpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetRestrictionAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetTrafficRestriction(location);
        await VerifySendAsyncCalled<TrafficRestrictionData>(expectedEndpoint, location);
    }

    [Fact]
    public async Task GetRestrictionAsync_WithCityIdQuery_ShouldWork()
    {
        // 准备
        var location = CreateCityLocation();
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        EndpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetRestrictionAsync(location);

        // 断言
        result.Should().Be(expectedResponse);
        EndpointProvider.Received(1).GetTrafficRestriction(location);
    }

    [Fact]
    public async Task GetRestrictionAsync_WithCancellation_ShouldPassCancellationToken()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var cts = new CancellationTokenSource();
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Success(new TrafficRestrictionData());

        EndpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse, cts.Token);

        // 执行
        await Service.GetRestrictionAsync(location, cts.Token);

        // 断言
        await VerifySendAsyncCalled<TrafficRestrictionData>(expectedEndpoint, location, cts.Token);
    }

    [Fact]
    public async Task GetRestrictionAsync_WhenHttpClientThrows_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var exception = new HttpRequestException("boom");

        EndpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        HttpClient.SendAsync<TrafficRestrictionData>(expectedEndpoint, location, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ApiResponse<TrafficRestrictionData>>(exception));

        // 执行
        var action = () => Service.GetRestrictionAsync(location);

        // 断言
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("*boom*");
    }

    [Fact]
    public async Task GetRestrictionAsync_WhenEndpointProviderThrowsTierException_ShouldPropagateException()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var exception = new SubscriptionTierNotSupportedException(
            "限行数据",
            "经纬度",
            SubscriptionTier.Trial,
            new[] { SubscriptionTier.Professional });

        EndpointProvider.When(provider => provider.GetTrafficRestriction(location))
            .Do(_ => throw exception);

        // 执行
        var action = () => Service.GetRestrictionAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>()
            .WithMessage("*限行数据*");
    }

    [Fact]
    public async Task GetRestrictionAsync_WhenResponseFails_ShouldReturnFailure()
    {
        // 准备
        var location = CreateCoordinatesLocation();
        var expectedEndpoint = new EndpointInfo("Traffic", "token", "https://test.api.com", "/test/traffic", SubscriptionTier.Professional);
        var expectedResponse = ApiResponse<TrafficRestrictionData>.Failure(401, "失败");

        EndpointProvider.GetTrafficRestriction(location).Returns(expectedEndpoint);
        SetupResponse(expectedEndpoint, location, expectedResponse);

        // 执行
        var result = await Service.GetRestrictionAsync(location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(401);
        result.Message.Should().Be("失败");
    }
}

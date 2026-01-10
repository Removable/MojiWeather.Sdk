using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Services;
using MojiWeather.Sdk.Tests.TestUtilities;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Integration;

public class EndToEndTests
{
    [Fact]
    public async Task FullRequestFlow_WithValidConfiguration_ShouldSucceed()
    {
        // 准备
        HttpRequestMessage? capturedRequest = null;
        var handler = new MockHttpMessageHandler(request =>
        {
            capturedRequest = request;
            var response = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(response))
            };
        });

        var options = new MojiWeatherOptions
        {
            AppCode = "test-appcode-12345",
            Tier = SubscriptionTier.Trial
        };

        var client = CreateClient(handler, options);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var result = await client.Weather.GetBriefConditionAsync(location);

        // 断言
        result.IsSuccess.Should().BeTrue();
        capturedRequest.Should().NotBeNull();
        capturedRequest!.RequestUri!.ToString().Should().Be(
            "https://freelat.mojicb.com/whapi/json/aliweather/briefcondition");
        capturedRequest.Headers.Authorization!.ToString().Should().Be("APPCODE test-appcode-12345");
    }

    [Fact]
    public async Task FullRequestFlow_WithInvalidAppCode_ShouldThrowAuthenticationException()
    {
        // 准备
        var handler = new MockHttpMessageHandler("Unauthorized", HttpStatusCode.Unauthorized);
        var options = new MojiWeatherOptions
        {
            AppCode = "test-appcode-12345",
            Tier = SubscriptionTier.Trial
        };

        var client = CreateClient(handler, options);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var action = () => client.Weather.GetBriefConditionAsync(location);

        // 断言
        await action.Should().ThrowAsync<AuthenticationException>();
    }

    [Fact]
    public async Task FullRequestFlow_WithInsufficientTier_ShouldThrowTierException()
    {
        // 准备
        var handler = new MockHttpMessageHandler("{}", HttpStatusCode.OK);
        var options = new MojiWeatherOptions
        {
            AppCode = "test-appcode-12345",
            Tier = SubscriptionTier.Trial
        };

        var client = CreateClient(handler, options);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var action = () => client.Forecast.GetForecast15DaysAsync(location);

        // 断言
        await action.Should().ThrowAsync<SubscriptionTierNotSupportedException>();
    }

    private static MojiWeatherClient CreateClient(HttpMessageHandler handler, MojiWeatherOptions options)
    {
        var httpClient = new HttpClient(handler);
        var logger = Substitute.For<ILogger<MojiHttpClient>>();
        var mojiHttpClient = new MojiHttpClient(httpClient, Options.Create(options), logger);
        var endpointProvider = new EndpointProvider(Options.Create(options));

        return new MojiWeatherClient(
            new WeatherService(mojiHttpClient, endpointProvider),
            new ForecastService(mojiHttpClient, endpointProvider),
            new AirQualityService(mojiHttpClient, endpointProvider),
            new AlertService(mojiHttpClient, endpointProvider),
            new LiveIndexService(mojiHttpClient, endpointProvider),
            new TrafficService(mojiHttpClient, endpointProvider));
    }
}

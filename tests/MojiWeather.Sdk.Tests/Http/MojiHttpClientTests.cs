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
using MojiWeather.Sdk.Tests.TestUtilities;
using NSubstitute;
using Xunit;

namespace MojiWeather.Sdk.Tests.Http;

public class MojiHttpClientTests
{
    private readonly IOptions<MojiWeatherOptions> _options;
    private readonly ILogger<MojiHttpClient> _logger;

    public MojiHttpClientTests()
    {
        _options = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode-12345",
            Tier = SubscriptionTier.Trial
        });
        _logger = Substitute.For<ILogger<MojiHttpClient>>();
    }

    [Fact]
    public async Task SendAsync_WithSuccessfulResponse_ShouldReturnDeserializedData()
    {
        // 准备
        var responseData = new BriefConditionData();
        var apiResponse = ApiResponse<BriefConditionData>.Success(responseData);
        var json = JsonSerializer.Serialize(apiResponse);

        var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var result = await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        result.IsSuccess.Should().BeTrue();
        result.Code.Should().Be(0);
    }

    [Fact]
    public async Task SendAsync_WithInsufficientTier_ShouldReturnFailure()
    {
        // 准备
        var trialOptions = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode-12345",
            Tier = SubscriptionTier.Trial
        });

        var handler = new MockHttpMessageHandler("{}", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, trialOptions, _logger);

        var endpoint = new EndpointInfo("Pro Only", "token123", "https://test.api.com", "/pro", SubscriptionTier.Professional);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var result = await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(-1);
        result.Message.Should().Contain("Professional");
        result.Message.Should().Contain("tier");
    }

    [Fact]
    public async Task SendAsync_WithUnauthorizedResponse_ShouldThrowAuthenticationException()
    {
        // 准备
        var handler = new MockHttpMessageHandler("Unauthorized", HttpStatusCode.Unauthorized);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("*Invalid AppCode*");
    }

    [Fact]
    public async Task SendAsync_WithInvalidJsonResponse_ShouldThrowApiException()
    {
        // 准备
        var handler = new MockHttpMessageHandler("not valid json {{{", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*Failed to parse*");
    }

    [Fact]
    public async Task SendAsync_WithNullResponse_ShouldThrowApiException()
    {
        // 准备
        var handler = new MockHttpMessageHandler("null", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*Failed to deserialize*");
    }

    [Fact]
    public async Task SendAsync_WithHttpRequestException_ShouldThrowApiException()
    {
        // 准备
        var handler = new MockHttpMessageHandler(new HttpRequestException("Connection refused"));
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*HTTP request*failed*");
    }

    [Fact]
    public async Task SendAsync_WithCityIdLocation_ShouldSendCorrectFormData()
    {
        // 准备
        string? capturedContent = null;
        var handler = new MockHttpMessageHandler(async request =>
        {
            capturedContent = await request.Content!.ReadAsStringAsync();
            var responseData = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData))
            };
        });

        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCityId(101010100);

        // 执行
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        capturedContent.Should().Contain("token=token123");
        capturedContent.Should().Contain("cityId=101010100");
    }

    [Fact]
    public async Task SendAsync_WithCoordinatesLocation_ShouldSendCorrectFormData()
    {
        // 准备
        string? capturedContent = null;
        var handler = new MockHttpMessageHandler(async request =>
        {
            capturedContent = await request.Content!.ReadAsStringAsync();
            var responseData = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData))
            };
        });

        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        capturedContent.Should().Contain("token=token123");
        capturedContent.Should().Contain("lat=39.9");
        capturedContent.Should().Contain("lon=116.4");
    }

    [Fact]
    public async Task SendAsync_WithAdditionalParameters_ShouldIncludeThemInFormData()
    {
        // 准备
        string? capturedContent = null;
        var handler = new MockHttpMessageHandler(async request =>
        {
            capturedContent = await request.Content!.ReadAsStringAsync();
            var responseData = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData))
            };
        });

        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var additionalParams = new Dictionary<string, string>
        {
            ["customParam"] = "customValue",
            ["anotherParam"] = "anotherValue"
        };

        // 执行
        await client.SendAsync<BriefConditionData>(endpoint, location, additionalParams);

        // 断言
        capturedContent.Should().Contain("customParam=customValue");
        capturedContent.Should().Contain("anotherParam=anotherValue");
    }

    [Fact]
    public async Task SendAsync_WithAdditionalParameters_ShouldNotOverrideReservedParameters()
    {
        // 准备
        string? capturedContent = null;
        var handler = new MockHttpMessageHandler(async request =>
        {
            capturedContent = await request.Content!.ReadAsStringAsync();
            var responseData = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData))
            };
        });

        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);
        var additionalParams = new Dictionary<string, string>
        {
            ["token"] = "override-token",
            ["lat"] = "0",
            ["lon"] = "0"
        };

        // 执行
        await client.SendAsync<BriefConditionData>(endpoint, location, additionalParams);

        // 断言
        capturedContent.Should().Contain("token=token123");
        capturedContent.Should().Contain("lat=39.9");
        capturedContent.Should().Contain("lon=116.4");
        capturedContent.Should().NotContain("token=override-token");
    }

    [Fact]
    public async Task SendAsync_ShouldIncludeAppCodeInAuthorizationHeader()
    {
        // 准备
        string? authHeader = null;
        var handler = new MockHttpMessageHandler(request =>
        {
            authHeader = request.Headers.Authorization?.ToString();
            var responseData = ApiResponse<BriefConditionData>.Success(new BriefConditionData());
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseData))
            };
        });

        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        authHeader.Should().Be("APPCODE test-appcode-12345");
    }

    [Fact]
    public async Task SendAsync_WithApiErrorResponse_ShouldReturnFailure()
    {
        // 准备
        var errorResponse = ApiResponse<BriefConditionData>.Failure(500, "Internal server error");
        var json = JsonSerializer.Serialize(errorResponse);

        var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var result = await client.SendAsync<BriefConditionData>(endpoint, location);

        // 断言
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(500);
        result.Message.Should().Be("Internal server error");
    }

    [Fact]
    public async Task SendAsync_WithCancellationToken_ShouldPassToHttpClient()
    {
        // 准备
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var handler = new MockHttpMessageHandler("{}", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location, cancellationToken: cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task SendAsync_WithHttpRequestException_ShouldIncludeEndpointNameInMessage()
    {
        // 准备
        var handler = new MockHttpMessageHandler(new HttpRequestException("Connection refused"));
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("BriefCondition", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*'BriefCondition'*");
    }

    [Fact]
    public async Task SendAsync_WithInvalidJsonResponse_ShouldIncludeResponsePreviewInMessage()
    {
        // 准备
        var invalidJson = "this is not valid json content for testing preview";
        var handler = new MockHttpMessageHandler(invalidJson, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("TestEndpoint", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        var exception = await act.Should().ThrowAsync<ApiException>();
        exception.Which.Message.Should().Contain("'TestEndpoint'");
        exception.Which.Message.Should().Contain("this is not valid json");
    }

    [Fact]
    public async Task SendAsync_WithNullResponse_ShouldIncludeEndpointNameInMessage()
    {
        // 准备
        var handler = new MockHttpMessageHandler("null", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("DetailedAqi", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*'DetailedAqi'*");
    }

    [Fact]
    public async Task SendAsync_WithUnauthorizedResponse_ShouldIncludeEndpointNameInMessage()
    {
        // 准备
        var handler = new MockHttpMessageHandler("Unauthorized", HttpStatusCode.Unauthorized);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Forecast15Days", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("*'Forecast15Days'*");
    }

}

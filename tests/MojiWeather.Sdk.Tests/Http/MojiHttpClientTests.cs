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
        // Arrange
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

        // Act
        var result = await client.SendAsync<BriefConditionData>(endpoint, location);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(-1);
        result.Message.Should().Contain("Professional");
        result.Message.Should().Contain("tier");
    }

    [Fact]
    public async Task SendAsync_WithUnauthorizedResponse_ShouldThrowAuthenticationException()
    {
        // Arrange
        var handler = new MockHttpMessageHandler("Unauthorized", HttpStatusCode.Unauthorized);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act & Assert
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<AuthenticationException>()
            .WithMessage("*Invalid AppCode*");
    }

    [Fact]
    public async Task SendAsync_WithInvalidJsonResponse_ShouldThrowApiException()
    {
        // Arrange
        var handler = new MockHttpMessageHandler("not valid json {{{", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act & Assert
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*Failed to parse*");
    }

    [Fact]
    public async Task SendAsync_WithNullResponse_ShouldThrowApiException()
    {
        // Arrange
        var handler = new MockHttpMessageHandler("null", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act & Assert
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*Failed to deserialize*");
    }

    [Fact]
    public async Task SendAsync_WithHttpRequestException_ShouldThrowApiException()
    {
        // Arrange
        var handler = new MockHttpMessageHandler(new HttpRequestException("Connection refused"));
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act & Assert
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location);
        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("*HTTP request failed*");
    }

    [Fact]
    public async Task SendAsync_WithCityIdLocation_ShouldSendCorrectFormData()
    {
        // Arrange
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

        // Act
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // Assert
        capturedContent.Should().Contain("token=token123");
        capturedContent.Should().Contain("cityId=101010100");
    }

    [Fact]
    public async Task SendAsync_WithCoordinatesLocation_ShouldSendCorrectFormData()
    {
        // Arrange
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

        // Act
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // Assert
        capturedContent.Should().Contain("token=token123");
        capturedContent.Should().Contain("lat=39.9");
        capturedContent.Should().Contain("lon=116.4");
    }

    [Fact]
    public async Task SendAsync_WithAdditionalParameters_ShouldIncludeThemInFormData()
    {
        // Arrange
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

        // Act
        await client.SendAsync<BriefConditionData>(endpoint, location, additionalParams);

        // Assert
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
        // Arrange
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

        // Act
        await client.SendAsync<BriefConditionData>(endpoint, location);

        // Assert
        authHeader.Should().Be("APPCODE test-appcode-12345");
    }

    [Fact]
    public async Task SendAsync_WithApiErrorResponse_ShouldReturnFailure()
    {
        // Arrange
        var errorResponse = ApiResponse<BriefConditionData>.Failure(500, "Internal server error");
        var json = JsonSerializer.Serialize(errorResponse);

        var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var result = await client.SendAsync<BriefConditionData>(endpoint, location);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Code.Should().Be(500);
        result.Message.Should().Be("Internal server error");
    }

    [Fact]
    public async Task SendAsync_WithCancellationToken_ShouldPassToHttpClient()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var handler = new MockHttpMessageHandler("{}", HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);
        var client = new MojiHttpClient(httpClient, _options, _logger);

        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act & Assert
        var act = () => client.SendAsync<BriefConditionData>(endpoint, location, cancellationToken: cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    /// <summary>
    /// Mock HTTP message handler for testing
    /// </summary>
    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

        public MockHttpMessageHandler(string responseContent, HttpStatusCode statusCode)
        {
            _handler = _ => Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(responseContent)
            });
        }

        public MockHttpMessageHandler(Exception exception)
        {
            _handler = _ => throw exception;
        }

        public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = request => Task.FromResult(handler(request));
        }

        public MockHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _handler(request);
        }
    }
}

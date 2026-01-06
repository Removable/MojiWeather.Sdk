using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Serialization;

namespace MojiWeather.Sdk.Http;

/// <summary>
/// 墨迹天气HTTP客户端实现
/// </summary>
public sealed class MojiHttpClient(
    HttpClient httpClient,
    IOptions<MojiWeatherOptions> options,
    ILogger<MojiHttpClient> logger) : IMojiHttpClient
{
    private readonly MojiWeatherOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<ApiResponse<T>> SendAsync<T>(
        EndpointInfo endpoint,
        LocationQuery location,
        IReadOnlyDictionary<string, string>? additionalParameters = null,
        CancellationToken cancellationToken = default) where T : class
    {
        // 验证端点访问权限
        if (!endpoint.IsAccessibleWith(_options.Tier))
        {
            logger.LogWarning("Endpoint {EndpointName} requires {RequiredTier} tier, current tier is {CurrentTier}",
                endpoint.Name, endpoint.MinimumTier, _options.Tier);

            return ApiResponse<T>.Failure(-1,
                $"Endpoint '{endpoint.Name}' requires {endpoint.MinimumTier} tier or higher.");
        }

        // 构建请求
        var url = $"{_options.BaseUrl}{endpoint.Path}";
        var formData = BuildFormData(endpoint.Token, location, additionalParameters);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("APPCODE", _options.AppCode);
        request.Content = new FormUrlEncodedContent(formData);

        logger.LogDebug("Sending request to {Url} with token {Token}",
            url, endpoint.Token[..8] + "...");

        try
        {
            using var response = await httpClient.SendAsync(request, cancellationToken);

            // 处理认证错误
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException("Invalid AppCode or insufficient permissions.");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            // 尝试解析响应
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, MojiJsonContext.SerializerOptions);

            if (result is null)
            {
                throw new ApiException("Failed to deserialize API response.", (int)response.StatusCode);
            }

            if (!result.IsSuccess)
            {
                logger.LogWarning("API returned error: Code={Code}, Message={Message}",
                    result.Code, result.Message);
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request failed for endpoint {EndpointName}", endpoint.Name);
            throw new ApiException($"HTTP request failed: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to parse response for endpoint {EndpointName}", endpoint.Name);
            throw new ApiException($"Failed to parse API response: {ex.Message}", ex);
        }
    }

    private static Dictionary<string, string> BuildFormData(
        string token,
        LocationQuery location,
        IReadOnlyDictionary<string, string>? additionalParameters)
    {
        var formData = new Dictionary<string, string>
        {
            ["token"] = token
        };

        foreach (var (key, value) in location.ToFormParameters())
        {
            formData[key] = value;
        }

        if (additionalParameters is not null)
        {
            foreach (var (key, value) in additionalParameters)
            {
                formData[key] = value;
            }
        }

        return formData;
    }
}

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
                $"Endpoint '{endpoint.Name}' requires {endpoint.MinimumTier} tier.");
        }

        // 构建请求
        var url = $"{endpoint.BaseUrl}{endpoint.Path}";
        var formData = BuildFormData(endpoint.Token, location, additionalParameters);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("APPCODE", _options.AppCode);
        request.Content = new FormUrlEncodedContent(formData);

        logger.LogDebug("Sending request to {Url} for endpoint {EndpointName}",
            url, endpoint.Name);

        var startTime = System.Diagnostics.Stopwatch.GetTimestamp();
        string? content = null;

        try
        {
            using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var elapsedMs = System.Diagnostics.Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;
            logger.LogDebug("Request to {EndpointName} completed in {ElapsedMs:F1}ms with status {StatusCode}",
                endpoint.Name, elapsedMs, (int)response.StatusCode);

            // 处理认证错误
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException($"Invalid AppCode or insufficient permissions for endpoint '{endpoint.Name}'.");
            }

            content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            // 尝试解析响应
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, MojiJsonContext.SerializerOptions);

            if (result is null)
            {
                var preview = GetResponsePreview(content);
                throw new ApiException($"Failed to deserialize response from '{endpoint.Name}'. Response preview: {preview}", (int)response.StatusCode);
            }

            if (!result.IsSuccess)
            {
                logger.LogWarning("API returned error for {EndpointName}: Code={Code}, Message={Message}",
                    endpoint.Name, result.Code, result.Message);
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request to {Url} failed for endpoint {EndpointName}", url, endpoint.Name);
            throw new ApiException($"HTTP request to '{endpoint.Name}' failed: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            var preview = GetResponsePreview(content);
            logger.LogError(ex, "Failed to parse response from {Url} for endpoint {EndpointName}. Response preview: {ResponsePreview}",
                url, endpoint.Name, preview);
            throw new ApiException($"Failed to parse '{endpoint.Name}' response: {ex.Message}. Response preview: {preview}", ex);
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
                formData.TryAdd(key, value);
            }
        }

        return formData;
    }

    private static string GetResponsePreview(string? content)
    {
        if (string.IsNullOrEmpty(content))
            return "(empty)";

        const int maxLength = 200;
        return content.Length <= maxLength
            ? content
            : string.Concat(content.AsSpan(0, maxLength), "...");
    }
}

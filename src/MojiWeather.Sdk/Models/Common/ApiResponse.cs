using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Models.Common;

/// <summary>
/// API响应基类
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public sealed record ApiResponse<T> where T : class
{
    /// <summary>
    /// 响应状态码 (0 表示成功)
    /// </summary>
    [JsonPropertyName("code")]
    public required int Code { get; init; }

    /// <summary>
    /// 响应消息
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Message { get; init; }

    /// <summary>
    /// 响应数据
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; init; }

    /// <summary>
    /// RC (返回码) - 部分接口使用
    /// </summary>
    [JsonPropertyName("rc")]
    public ResponseContext? ResponseContext { get; init; }

    /// <summary>
    /// 是否成功
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Code == 0;

    /// <summary>
    /// 创建成功响应
    /// </summary>
    public static ApiResponse<T> Success(T data, string? message = null) => new()
    {
        Code = 0,
        Message = message ?? "Success",
        Data = data
    };

    /// <summary>
    /// 创建失败响应
    /// </summary>
    public static ApiResponse<T> Failure(int code, string message) => new()
    {
        Code = code,
        Message = message,
        Data = null
    };
}

/// <summary>
/// 响应上下文
/// </summary>
public sealed record ResponseContext
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonPropertyName("c")]
    public int Code { get; init; }

    /// <summary>
    /// 状态描述
    /// </summary>
    [JsonPropertyName("p")]
    public string? Description { get; init; }
}

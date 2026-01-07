using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;
using MojiWeather.Sdk.Models.Traffic;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Serialization.Converters;

namespace MojiWeather.Sdk.Serialization;

/// <summary>
/// 墨迹天气JSON序列化上下文 (Source Generator)
/// </summary>
/// <remarks>
/// 使用源生成器优化JSON序列化性能，支持AOT/Trimming。
/// 自定义转换器用于处理API返回的字符串格式数字。
/// </remarks>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
[JsonSerializable(typeof(ApiResponse<BriefConditionData>))]
[JsonSerializable(typeof(ApiResponse<DetailedConditionData>))]
[JsonSerializable(typeof(ApiResponse<DailyForecastData>))]
[JsonSerializable(typeof(ApiResponse<HourlyForecastData>))]
[JsonSerializable(typeof(ApiResponse<ShortForecastData>))]
[JsonSerializable(typeof(ApiResponse<BriefAqiData>))]
[JsonSerializable(typeof(ApiResponse<DetailedAqiData>))]
[JsonSerializable(typeof(ApiResponse<AqiForecastData>))]
[JsonSerializable(typeof(ApiResponse<WeatherAlertData>))]
[JsonSerializable(typeof(ApiResponse<LiveIndexData>))]
[JsonSerializable(typeof(ApiResponse<TrafficRestrictionData>))]
public partial class MojiJsonContext : JsonSerializerContext
{
    private static JsonSerializerOptions? _cachedOptions;

    /// <summary>
    /// 获取配置好的 JsonSerializerOptions（包含源生成器元数据和自定义转换器）
    /// </summary>
    public static JsonSerializerOptions SerializerOptions => _cachedOptions ??= CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            // 使用源生成器元数据，支持AOT/Trimming
            TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault
                ? JsonTypeInfoResolver.Combine(Default, new DefaultJsonTypeInfoResolver())
                : Default
        };

        // API返回字符串格式数字，需要自定义转换器
        options.Converters.Add(new StringToIntConverter());
        options.Converters.Add(new StringToNullableIntConverter());
        options.Converters.Add(new StringToDoubleConverter());
        options.Converters.Add(new StringToNullableDoubleConverter());
        options.Converters.Add(new UnixMillisecondsConverter());
        options.Converters.Add(new UnixMillisecondsNullableConverter());

        return options;
    }
}

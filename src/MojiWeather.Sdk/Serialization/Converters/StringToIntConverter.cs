using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Serialization.Converters;

/// <summary>
/// JSON转换器通用解析辅助方法
/// </summary>
internal static class JsonParseHelper
{
    /// <summary>
    /// 从JSON读取器解析int值（支持字符串和数字格式）
    /// </summary>
    public static int ParseInt(ref Utf8JsonReader reader) =>
        reader.TokenType switch
        {
            JsonTokenType.String when int.TryParse(reader.GetString(), out var result) => result,
            JsonTokenType.String => throw new JsonException("Invalid int value."),
            JsonTokenType.Number => reader.GetInt32(),
            _ => throw new JsonException($"Unexpected token type for int: {reader.TokenType}")
        };

    /// <summary>
    /// 从JSON读取器解析double值（支持字符串和数字格式）
    /// </summary>
    public static double ParseDouble(ref Utf8JsonReader reader) =>
        reader.TokenType switch
        {
            JsonTokenType.String when double.TryParse(reader.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result) => result,
            JsonTokenType.String => throw new JsonException("Invalid double value."),
            JsonTokenType.Number => reader.GetDouble(),
            _ => throw new JsonException($"Unexpected token type for double: {reader.TokenType}")
        };

    /// <summary>
    /// 从JSON读取器解析long值用于Unix时间戳（支持字符串和数字格式）
    /// </summary>
    public static long ParseLong(ref Utf8JsonReader reader) =>
        reader.TokenType switch
        {
            JsonTokenType.String when long.TryParse(reader.GetString(), out var result) => result,
            JsonTokenType.String => throw new JsonException("Invalid unix milliseconds value."),
            JsonTokenType.Number => reader.GetInt64(),
            _ => throw new JsonException($"Unexpected token type for unix timestamp: {reader.TokenType}")
        };
}

/// <summary>
/// 字符串转int的JSON转换器
/// </summary>
public sealed class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null
            ? throw new JsonException("Null value is not allowed for int.")
            : JsonParseHelper.ParseInt(ref reader);

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

/// <summary>
/// 字符串转可空int的JSON转换器
/// </summary>
public sealed class StringToNullableIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null
            ? null
            : JsonParseHelper.ParseInt(ref reader);

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Serialization.Converters;

/// <summary>
/// 字符串转double的JSON转换器
/// </summary>
public sealed class StringToDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => double.TryParse(reader.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0.0,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.Null => 0.0,
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

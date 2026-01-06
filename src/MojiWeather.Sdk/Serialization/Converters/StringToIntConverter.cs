using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Serialization.Converters;

/// <summary>
/// 字符串转int的JSON转换器
/// </summary>
public sealed class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => int.TryParse(reader.GetString(), out var result) ? result : 0,
            JsonTokenType.Number => reader.GetInt32(),
            JsonTokenType.Null => 0,
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

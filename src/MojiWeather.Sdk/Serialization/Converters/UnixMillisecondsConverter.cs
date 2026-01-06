using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Serialization.Converters;

/// <summary>
/// Unix时间戳(毫秒)转DateTimeOffset的JSON转换器
/// </summary>
public sealed class UnixMillisecondsConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long milliseconds = reader.TokenType switch
        {
            JsonTokenType.String => long.TryParse(reader.GetString(), out var result) ? result : 0,
            JsonTokenType.Number => reader.GetInt64(),
            JsonTokenType.Null => 0,
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };

        return milliseconds > 0
            ? DateTimeOffset.FromUnixTimeMilliseconds(milliseconds)
            : DateTimeOffset.MinValue;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
}

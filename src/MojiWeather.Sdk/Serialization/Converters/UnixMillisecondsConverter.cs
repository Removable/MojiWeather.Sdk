using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojiWeather.Sdk.Serialization.Converters;

/// <summary>
/// Unix时间戳(毫秒)转DateTimeOffset的JSON转换器
/// </summary>
public sealed class UnixMillisecondsConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null
            ? throw new JsonException("Null value is not allowed for DateTimeOffset.")
            : DateTimeOffset.FromUnixTimeMilliseconds(JsonParseHelper.ParseLong(ref reader));

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
}

/// <summary>
/// Unix时间戳(毫秒)转可空DateTimeOffset的JSON转换器
/// </summary>
public sealed class UnixMillisecondsNullableConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null
            ? null
            : DateTimeOffset.FromUnixTimeMilliseconds(JsonParseHelper.ParseLong(ref reader));

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteNumberValue(value.Value.ToUnixTimeMilliseconds());
        }
    }
}

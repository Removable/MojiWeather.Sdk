using System.Text.Json;
using FluentAssertions;
using MojiWeather.Sdk.Serialization;
using MojiWeather.Sdk.Serialization.Converters;
using Xunit;

namespace MojiWeather.Sdk.Tests.Serialization;

public class JsonConverterTests
{
    [Theory]
    [InlineData("\"42\"", 42)]
    [InlineData("42", 42)]
    [InlineData("\"0\"", 0)]
    [InlineData("\"-10\"", -10)]
    public void StringToIntConverter_ShouldConvertCorrectly(string json, int expected)
    {
        // 准备
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToIntConverter());

        // 执行
        var result = JsonSerializer.Deserialize<int>(json, options);

        // 断言
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("\"invalid\"")]
    public void StringToIntConverter_ShouldThrowOnInvalidOrNull(string json)
    {
        // 准备
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToIntConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<int>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void StringToNullableIntConverter_ShouldHandleNull()
    {
        // 准备
        const string json = "null";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToNullableIntConverter());

        // 执行
        var result = JsonSerializer.Deserialize<int?>(json, options);

        // 断言
        result.Should().BeNull();
    }

    [Fact]
    public void StringToNullableIntConverter_ShouldThrowOnInvalid()
    {
        // 准备
        const string json = "\"invalid\"";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToNullableIntConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<int?>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Theory]
    [InlineData("\"3.14\"", 3.14)]
    [InlineData("3.14", 3.14)]
    [InlineData("\"0.0\"", 0.0)]
    [InlineData("\"-2.5\"", -2.5)]
    public void StringToDoubleConverter_ShouldConvertCorrectly(string json, double expected)
    {
        // 准备
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToDoubleConverter());

        // 执行
        var result = JsonSerializer.Deserialize<double>(json, options);

        // 断言
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("\"invalid\"")]
    public void StringToDoubleConverter_ShouldThrowOnInvalidOrNull(string json)
    {
        // 准备
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToDoubleConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<double>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void StringToNullableDoubleConverter_ShouldHandleNull()
    {
        // 准备
        const string json = "null";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToNullableDoubleConverter());

        // 执行
        var result = JsonSerializer.Deserialize<double?>(json, options);

        // 断言
        result.Should().BeNull();
    }

    [Fact]
    public void StringToNullableDoubleConverter_ShouldThrowOnInvalid()
    {
        // 准备
        const string json = "\"invalid\"";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToNullableDoubleConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<double?>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void UnixMillisecondsConverter_ShouldConvertCorrectly()
    {
        // 准备
        const string json = "\"1704067200000\""; // 2024-01-01 00:00:00 UTC
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsConverter());

        // 执行
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // 断言
        result.Year.Should().Be(2024);
        result.Month.Should().Be(1);
        result.Day.Should().Be(1);
    }

    [Fact]
    public void UnixMillisecondsConverter_ShouldHandleNumericInput()
    {
        // 准备
        const string json = "1704067200000";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsConverter());

        // 执行
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // 断言
        result.Year.Should().Be(2024);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("\"invalid\"")]
    public void UnixMillisecondsConverter_ShouldThrowOnInvalidOrNull(string json)
    {
        // 准备
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void UnixMillisecondsNullableConverter_ShouldHandleNull()
    {
        // 准备
        const string json = "null";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsNullableConverter());

        // 执行
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // 断言
        result.Should().BeNull();
    }

    [Fact]
    public void UnixMillisecondsNullableConverter_ShouldThrowOnInvalid()
    {
        // 准备
        const string json = "\"invalid\"";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsNullableConverter());

        // 执行
        var act = () => JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // 断言
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void MojiJsonContext_SerializerOptions_ShouldContainAllConverters()
    {
        // 准备 & Act
        var options = MojiJsonContext.SerializerOptions;

        // 断言
        options.Converters.Should().Contain(c => c is StringToIntConverter);
        options.Converters.Should().Contain(c => c is StringToNullableIntConverter);
        options.Converters.Should().Contain(c => c is StringToDoubleConverter);
        options.Converters.Should().Contain(c => c is StringToNullableDoubleConverter);
        options.Converters.Should().Contain(c => c is UnixMillisecondsConverter);
        options.Converters.Should().Contain(c => c is UnixMillisecondsNullableConverter);
    }
}

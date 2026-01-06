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
        // Arrange
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToIntConverter());

        // Act
        var result = JsonSerializer.Deserialize<int>(json, options);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"3.14\"", 3.14)]
    [InlineData("3.14", 3.14)]
    [InlineData("\"0.0\"", 0.0)]
    [InlineData("\"-2.5\"", -2.5)]
    public void StringToDoubleConverter_ShouldConvertCorrectly(string json, double expected)
    {
        // Arrange
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringToDoubleConverter());

        // Act
        var result = JsonSerializer.Deserialize<double>(json, options);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void UnixMillisecondsConverter_ShouldConvertCorrectly()
    {
        // Arrange
        const string json = "\"1704067200000\""; // 2024-01-01 00:00:00 UTC
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsConverter());

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert
        result.Year.Should().Be(2024);
        result.Month.Should().Be(1);
        result.Day.Should().Be(1);
    }

    [Fact]
    public void UnixMillisecondsConverter_ShouldHandleNumericInput()
    {
        // Arrange
        const string json = "1704067200000";
        var options = new JsonSerializerOptions();
        options.Converters.Add(new UnixMillisecondsConverter());

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert
        result.Year.Should().Be(2024);
    }

    [Fact]
    public void MojiJsonContext_SerializerOptions_ShouldContainAllConverters()
    {
        // Arrange & Act
        var options = MojiJsonContext.SerializerOptions;

        // Assert
        options.Converters.Should().Contain(c => c is StringToIntConverter);
        options.Converters.Should().Contain(c => c is StringToDoubleConverter);
        options.Converters.Should().Contain(c => c is UnixMillisecondsConverter);
    }
}

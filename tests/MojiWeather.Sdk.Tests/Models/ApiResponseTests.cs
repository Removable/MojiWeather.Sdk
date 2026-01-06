using System.Text.Json;
using FluentAssertions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Serialization;
using Xunit;

namespace MojiWeather.Sdk.Tests.Models;

public class ApiResponseTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResponse()
    {
        // Arrange
        var data = new TestData { Value = "test" };

        // Act
        var response = ApiResponse<TestData>.Success(data, "OK");

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(0);
        response.Message.Should().Be("OK");
        response.Data.Should().Be(data);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResponse()
    {
        // Arrange & Act
        var response = ApiResponse<TestData>.Failure(500, "Internal Error");

        // Assert
        response.IsSuccess.Should().BeFalse();
        response.Code.Should().Be(500);
        response.Message.Should().Be("Internal Error");
        response.Data.Should().BeNull();
    }

    [Fact]
    public void Deserialize_ShouldParseJsonCorrectly()
    {
        // Arrange
        const string json = """
            {
                "code": 0,
                "msg": "success",
                "data": {
                    "value": "test123"
                }
            }
            """;

        // Act
        var response = JsonSerializer.Deserialize<ApiResponse<TestData>>(json, MojiJsonContext.SerializerOptions);

        // Assert
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(0);
        response.Message.Should().Be("success");
        response.Data.Should().NotBeNull();
        response.Data!.Value.Should().Be("test123");
    }

    private sealed record TestData
    {
        public string? Value { get; init; }
    }
}

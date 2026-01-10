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
        // 准备
        var data = new TestData { Value = "test" };

        // 执行
        var response = ApiResponse<TestData>.Success(data, "OK");

        // 断言
        response.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(0);
        response.Message.Should().Be("OK");
        response.Data.Should().Be(data);
    }

    [Fact]
    public void Success_WithNullData_ShouldAllowNull()
    {
        // 执行
        var response = ApiResponse<TestData>.Success(null!, "OK");

        // 断言
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().BeNull();
        response.Message.Should().Be("OK");
    }

    [Fact]
    public void Failure_ShouldCreateFailureResponse()
    {
        // 准备与执行
        var response = ApiResponse<TestData>.Failure(500, "Internal Error");

        // 断言
        response.IsSuccess.Should().BeFalse();
        response.Code.Should().Be(500);
        response.Message.Should().Be("Internal Error");
        response.Data.Should().BeNull();
    }

    [Fact]
    public void Failure_WithEmptyMessage_ShouldPreserveMessage()
    {
        // 准备与执行
        var response = ApiResponse<TestData>.Failure(400, string.Empty);

        // 断言
        response.IsSuccess.Should().BeFalse();
        response.Message.Should().BeEmpty();
    }

    [Fact]
    public void Failure_WithNegativeCode_ShouldPreserveCode()
    {
        // 准备与执行
        var response = ApiResponse<TestData>.Failure(-10, "error");

        // 断言
        response.IsSuccess.Should().BeFalse();
        response.Code.Should().Be(-10);
        response.Message.Should().Be("error");
    }

    [Fact]
    public void Deserialize_ShouldParseJsonCorrectly()
    {
        // 准备
        const string json = """
            {
                "code": 0,
                "msg": "success",
                "data": {
                    "value": "test123"
                }
            }
            """;

        // 执行
        var response = JsonSerializer.Deserialize<ApiResponse<TestData>>(json, MojiJsonContext.SerializerOptions);

        // 断言
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Code.Should().Be(0);
        response.Message.Should().Be("success");
        response.Data.Should().NotBeNull();
        response.Data!.Value.Should().Be("test123");
    }

    [Fact]
    public void Deserialize_WithMalformedJson_ShouldThrowJsonException()
    {
        // 准备
        const string json = "{ invalid json }";

        // 执行
        var action = () => JsonSerializer.Deserialize<ApiResponse<TestData>>(json, MojiJsonContext.SerializerOptions);

        // 断言
        action.Should().Throw<JsonException>();
    }

    private sealed record TestData
    {
        public string? Value { get; init; }
    }
}

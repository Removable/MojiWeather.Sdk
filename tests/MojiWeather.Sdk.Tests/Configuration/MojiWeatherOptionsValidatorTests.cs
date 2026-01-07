using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using Xunit;

namespace MojiWeather.Sdk.Tests.Configuration;

public class MojiWeatherOptionsValidatorTests
{
    private readonly MojiWeatherOptionsValidator _validator = new();

    [Fact]
    public void Validate_WithValidOptions_ShouldSucceed()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345"
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithMissingAppCode_ShouldFail(string? appCode)
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = appCode!
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("AppCode is required");
    }

    [Fact]
    public void Validate_WithShortAppCode_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "short"
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("AppCode appears to be invalid");
    }

    [Fact]
    public void Validate_WithZeroTimeout_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Timeout = TimeSpan.Zero
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("Timeout must be greater than zero");
    }

    [Fact]
    public void Validate_WithExcessiveTimeout_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Timeout = TimeSpan.FromMinutes(10)
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("Timeout should not exceed 5 minutes");
    }

    [Fact]
    public void Validate_WithNegativeMaxRetries_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Retry = new RetryOptions { MaxRetries = -1 }
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("MaxRetries cannot be negative");
    }

    [Fact]
    public void Validate_WithExcessiveMaxRetries_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Retry = new RetryOptions { MaxRetries = 20 }
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("MaxRetries should not exceed 10");
    }

    [Fact]
    public void Validate_WithNegativeInitialDelay_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Retry = new RetryOptions { InitialDelay = TimeSpan.FromSeconds(-1) }
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("InitialDelay cannot be negative");
    }

    [Fact]
    public void Validate_WithBackoffMultiplierLessThanOne_ShouldFail()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "valid-appcode-12345",
            Retry = new RetryOptions { BackoffMultiplier = 0.5 }
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.FailureMessage.Should().Contain("BackoffMultiplier must be at least 1.0");
    }

    [Fact]
    public void Validate_WithMultipleErrors_ShouldReportAllErrors()
    {
        // Arrange
        var options = new MojiWeatherOptions
        {
            AppCode = "",
            Timeout = TimeSpan.Zero,
            Retry = new RetryOptions { MaxRetries = -1 }
        };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        result.Succeeded.Should().BeFalse();
        // Should contain multiple error messages
        result.FailureMessage.Should().Contain("AppCode");
        result.FailureMessage.Should().Contain("Timeout");
        result.FailureMessage.Should().Contain("MaxRetries");
    }
}

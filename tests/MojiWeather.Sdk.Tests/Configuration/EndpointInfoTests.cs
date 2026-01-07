using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using Xunit;

namespace MojiWeather.Sdk.Tests.Configuration;

public class EndpointInfoTests
{
    [Theory]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Trial, true)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Trial, false)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Trial, false)]
    [InlineData(SubscriptionTier.Basic, SubscriptionTier.Trial, false)]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Pm25, false)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Pm25, true)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Pm25, false)]
    [InlineData(SubscriptionTier.Basic, SubscriptionTier.Pm25, false)]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Professional, false)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Professional, false)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Professional, true)]
    [InlineData(SubscriptionTier.Basic, SubscriptionTier.Professional, false)]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Basic, false)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Basic, false)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Basic, false)]
    [InlineData(SubscriptionTier.Basic, SubscriptionTier.Basic, true)]
    public void IsAccessibleWith_ShouldReturnCorrectResult(
        SubscriptionTier userTier,
        SubscriptionTier requiredTier,
        bool expectedResult)
    {
        // Arrange
        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", requiredTier);

        // Act
        var result = endpoint.IsAccessibleWith(userTier);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void MojiWeatherOptions_ShouldHaveCorrectDefaults()
    {
        // Arrange & Act
        var options = new MojiWeatherOptions { AppCode = "test" };

        // Assert
        options.Tier.Should().Be(SubscriptionTier.Trial);
        options.UseHttps.Should().BeTrue();
        options.Timeout.Should().Be(TimeSpan.FromSeconds(30));
        options.Retry.Should().NotBeNull();
        options.Retry.MaxRetries.Should().Be(3);
    }

    [Fact]
    public void MojiWeatherOptions_UseHttps_ShouldBeConfigurable()
    {
        // Arrange & Act
        var httpsOptions = new MojiWeatherOptions { AppCode = "test", UseHttps = true };
        var httpOptions = new MojiWeatherOptions { AppCode = "test", UseHttps = false };

        // Assert
        httpsOptions.UseHttps.Should().BeTrue();
        httpOptions.UseHttps.Should().BeFalse();
    }
}

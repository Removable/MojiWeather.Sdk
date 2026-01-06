using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using Xunit;

namespace MojiWeather.Sdk.Tests.Configuration;

public class EndpointInfoTests
{
    [Theory]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Trial, true)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Trial, true)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Trial, true)]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Pm25, false)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Pm25, true)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Pm25, true)]
    [InlineData(SubscriptionTier.Trial, SubscriptionTier.Professional, false)]
    [InlineData(SubscriptionTier.Pm25, SubscriptionTier.Professional, false)]
    [InlineData(SubscriptionTier.Professional, SubscriptionTier.Professional, true)]
    public void IsAccessibleWith_ShouldReturnCorrectResult(
        SubscriptionTier userTier,
        SubscriptionTier requiredTier,
        bool expectedResult)
    {
        // Arrange
        var endpoint = new EndpointInfo("Test", "token123", "/test", requiredTier);

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
        options.QueryType.Should().Be(LocationQueryType.Coordinates);
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

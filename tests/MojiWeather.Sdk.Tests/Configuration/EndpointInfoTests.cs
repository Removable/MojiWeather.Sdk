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
        // 准备
        var endpoint = new EndpointInfo("Test", "token123", "https://test.api.com", "/test", requiredTier);

        // 执行
        var result = endpoint.IsAccessibleWith(userTier);

        // 断言
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void MojiWeatherOptions_ShouldHaveCorrectDefaults()
    {
        // 准备与执行
        var options = new MojiWeatherOptions { AppCode = "test" };

        // 断言
        options.Tier.Should().Be(SubscriptionTier.Trial);
        options.UseHttps.Should().BeTrue();
        options.Timeout.Should().Be(TimeSpan.FromSeconds(30));
        options.Retry.Should().NotBeNull();
        options.Retry.MaxRetries.Should().Be(3);
    }

    [Fact]
    public void MojiWeatherOptions_UseHttps_ShouldBeConfigurable()
    {
        // 准备与执行
        var httpsOptions = new MojiWeatherOptions { AppCode = "test", UseHttps = true };
        var httpOptions = new MojiWeatherOptions { AppCode = "test", UseHttps = false };

        // 断言
        httpsOptions.UseHttps.Should().BeTrue();
        httpOptions.UseHttps.Should().BeFalse();
    }
}

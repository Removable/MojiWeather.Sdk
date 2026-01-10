using FluentAssertions;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Exceptions;
using Xunit;

namespace MojiWeather.Sdk.Tests.Configuration;

public class EndpointProviderTests
{
    private static IEndpointProvider CreateProvider(SubscriptionTier tier = SubscriptionTier.Trial)
    {
        var options = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode",
            Tier = tier
        });
        return new EndpointProvider(options);
    }

    [Fact]
    public void GetBriefCondition_WithCoordinates_ShouldReturnCorrectEndpoint()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetBriefCondition(location);

        // 断言
        endpoint.Name.Should().Be("精简实况");
        endpoint.Path.Should().Contain("/briefcondition");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetBriefCondition_WithCityId_ShouldReturnCorrectEndpoint()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);
        var location = LocationQuery.FromCityId(101010100);

        // 执行
        var endpoint = provider.GetBriefCondition(location);

        // 断言
        endpoint.Name.Should().Be("精简实况");
        endpoint.Path.Should().Contain("/briefcondition");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetForecast3Days_ShouldHaveTrialTier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetForecast3Days(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetForecast6Days_ShouldHavePm25Tier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Pm25);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetForecast6Days(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetForecast15Days_ShouldHaveProfessionalTier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Professional);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetForecast15Days(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetShortForecast_ShouldReturnCorrectEndpoint()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Professional);

        // 执行
        var endpoint = provider.GetShortForecast();

        // 断言
        endpoint.Name.Should().Be("短时预报");
        endpoint.Path.Should().Contain("/shortforecast");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedAqi_ShouldHavePm25Tier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Pm25);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetDetailedAqi(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetAlert_ShouldHavePm25Tier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Pm25);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetAlert(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetLiveIndex_ShouldHaveProfessionalTier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Professional);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetLiveIndex(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedCondition_WithCoordinates_ShouldHaveProfessionalTier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Professional);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetDetailedCondition(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedCondition_WithCityId_ShouldHaveBasicTier()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Basic);
        var location = LocationQuery.FromCityId(101010100);

        // 执行
        var endpoint = provider.GetDetailedCondition(location);

        // 断言
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Basic);
    }

    [Fact]
    public void EndpointProvider_WithCustomTokens_ShouldUseCustomTokens()
    {
        // 准备
        var customToken = "custom-test-token-12345";
        var options = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode",
            Tier = SubscriptionTier.Trial,
            Tokens = new EndpointTokens
            {
                Coordinates = new CoordinatesTokens
                {
                    BriefCondition = customToken
                }
            }
        });
        var provider = new EndpointProvider(options);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetBriefCondition(location);

        // 断言
        endpoint.Token.Should().Be(customToken);
    }

    [Fact]
    public void EndpointProvider_WithDefaultTokens_ShouldUseDefaultTokens()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetBriefCondition(location);

        // 断言
        endpoint.Token.Should().NotBeNullOrEmpty();
        endpoint.Token.Should().HaveLength(32); // Default tokens are 32 char hex strings
    }

    // 新增：订阅级别验证测试

    [Theory]
    [InlineData(SubscriptionTier.Pm25, "https://basiclat.mojicb.com")]
    [InlineData(SubscriptionTier.Basic, "https://aliv1.mojicb.com")]
    [InlineData(SubscriptionTier.Professional, "https://aliv8.mojicb.com")]
    public void GetDetailedAqi_WithDifferentTiers_ShouldUseCorrectBaseUrl(SubscriptionTier tier, string expectedBaseUrl)
    {
        // 准备
        var provider = CreateProvider(tier);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行
        var endpoint = provider.GetDetailedAqi(location);

        // 断言
        endpoint.BaseUrl.Should().Be(expectedBaseUrl);
    }

    [Fact]
    public void GetDetailedAqi_WithTrialTier_ShouldThrowException()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var action = () => provider.GetDetailedAqi(location);
        action.Should().Throw<SubscriptionTierNotSupportedException>()
            .Which.CurrentTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetBriefCondition_WithCityId_BasicTier_ShouldThrowException()
    {
        // 准备 - 基础版CityID查询不支持精简实况
        var provider = CreateProvider(SubscriptionTier.Basic);
        var location = LocationQuery.FromCityId(101010100);

        // 执行并断言
        var action = () => provider.GetBriefCondition(location);
        action.Should().Throw<SubscriptionTierNotSupportedException>();
    }

    [Fact]
    public void GetDetailedCondition_WithCoordinates_BasicTier_ShouldThrowException()
    {
        // 准备 - 基础版经纬度查询不支持天气实况（只有专业版支持）
        var provider = CreateProvider(SubscriptionTier.Basic);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // 执行并断言
        var action = () => provider.GetDetailedCondition(location);
        action.Should().Throw<SubscriptionTierNotSupportedException>();
    }

    [Fact]
    public void GetShortForecast_WithTrialTier_ShouldThrowException()
    {
        // 准备
        var provider = CreateProvider(SubscriptionTier.Trial);

        // 执行并断言
        var action = () => provider.GetShortForecast();
        action.Should().Throw<SubscriptionTierNotSupportedException>();
    }
}

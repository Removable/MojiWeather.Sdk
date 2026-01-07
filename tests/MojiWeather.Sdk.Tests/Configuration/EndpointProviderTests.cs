using FluentAssertions;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using Xunit;

namespace MojiWeather.Sdk.Tests.Configuration;

public class EndpointProviderTests
{
    private readonly IEndpointProvider _provider;

    public EndpointProviderTests()
    {
        var options = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode"
        });
        _provider = new EndpointProvider(options);
    }

    [Fact]
    public void GetBriefCondition_WithCoordinates_ShouldReturnCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetBriefCondition(location);

        // Assert
        endpoint.Name.Should().Be("精简实况");
        endpoint.Path.Should().Contain("/briefcondition");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetBriefCondition_WithCityId_ShouldReturnCorrectEndpoint()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);

        // Act
        var endpoint = _provider.GetBriefCondition(location);

        // Assert
        endpoint.Name.Should().Be("精简实况");
        endpoint.Path.Should().Contain("/briefcondition");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetForecast3Days_ShouldHaveTrialTier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetForecast3Days(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Trial);
    }

    [Fact]
    public void GetForecast6Days_ShouldHavePm25Tier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetForecast6Days(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetForecast15Days_ShouldHaveProfessionalTier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetForecast15Days(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetShortForecast_ShouldReturnCorrectEndpoint()
    {
        // Act
        var endpoint = _provider.GetShortForecast();

        // Assert
        endpoint.Name.Should().Be("短时预报");
        endpoint.Path.Should().Contain("/shortforecast");
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedAqi_ShouldHavePm25Tier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetDetailedAqi(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetAlert_ShouldHavePm25Tier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetAlert(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Pm25);
    }

    [Fact]
    public void GetLiveIndex_ShouldHaveProfessionalTier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetLiveIndex(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedCondition_WithCoordinates_ShouldHaveProfessionalTier()
    {
        // Arrange
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = _provider.GetDetailedCondition(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Professional);
    }

    [Fact]
    public void GetDetailedCondition_WithCityId_ShouldHaveBasicTier()
    {
        // Arrange
        var location = LocationQuery.FromCityId(101010100);

        // Act
        var endpoint = _provider.GetDetailedCondition(location);

        // Assert
        endpoint.MinimumTier.Should().Be(SubscriptionTier.Basic);
    }

    [Fact]
    public void EndpointProvider_WithCustomTokens_ShouldUseCustomTokens()
    {
        // Arrange
        var customToken = "custom-test-token-12345";
        var options = Options.Create(new MojiWeatherOptions
        {
            AppCode = "test-appcode",
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

        // Act
        var endpoint = provider.GetBriefCondition(location);

        // Assert
        endpoint.Token.Should().Be(customToken);
    }

    [Fact]
    public void EndpointProvider_WithDefaultTokens_ShouldUseDefaultTokens()
    {
        // Arrange
        var options = Options.Create(new MojiWeatherOptions { AppCode = "test-appcode" });
        var provider = new EndpointProvider(options);
        var location = LocationQuery.FromCoordinates(39.9, 116.4);

        // Act
        var endpoint = provider.GetBriefCondition(location);

        // Assert
        endpoint.Token.Should().NotBeNullOrEmpty();
        endpoint.Token.Should().HaveLength(32); // Default tokens are 32 char hex strings
    }
}

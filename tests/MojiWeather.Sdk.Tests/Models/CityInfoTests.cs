using System.Text.Json;
using FluentAssertions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Serialization;
using Xunit;

namespace MojiWeather.Sdk.Tests.Models;

public class CityInfoTests
{
    [Fact]
    public void Deserialize_ShouldParseCityInfo()
    {
        const string json = """
            {
                "cityId": 101010100,
                "counname": "China",
                "ianatimezone": "Asia/Shanghai",
                "name": "Beijing",
                "pname": "Beijing",
                "timezone": "+08:00",
                "secondaryname": "Beijing"
            }
            """;

        var result = JsonSerializer.Deserialize<CityInfo>(json, MojiJsonContext.SerializerOptions);

        result.Should().NotBeNull();
        result!.CityId.Should().Be(101010100);
        result.CountryName.Should().Be("China");
        result.IanaTimezone.Should().Be("Asia/Shanghai");
        result.Name.Should().Be("Beijing");
        result.ProvinceName.Should().Be("Beijing");
        result.Timezone.Should().Be("+08:00");
        result.SecondaryName.Should().Be("Beijing");
    }

    [Fact]
    public void Deserialize_WithMissingFields_ShouldHandleGracefully()
    {
        const string json = """
            {
                "cityId": 101010100,
                "name": "Beijing"
            }
            """;

        var result = JsonSerializer.Deserialize<CityInfo>(json, MojiJsonContext.SerializerOptions);

        result.Should().NotBeNull();
        result!.CityId.Should().Be(101010100);
        result.Name.Should().Be("Beijing");
        result.CountryName.Should().BeNull();
        result.IanaTimezone.Should().BeNull();
        result.ProvinceName.Should().BeNull();
        result.Timezone.Should().BeNull();
        result.SecondaryName.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WithNullValues_ShouldSetNull()
    {
        const string json = """
            {
                "cityId": 101010100,
                "counname": null,
                "ianatimezone": null,
                "secondaryname": null
            }
            """;

        var result = JsonSerializer.Deserialize<CityInfo>(json, MojiJsonContext.SerializerOptions);

        result.Should().NotBeNull();
        result!.CountryName.Should().BeNull();
        result.IanaTimezone.Should().BeNull();
        result.SecondaryName.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WithEmptyString_ShouldPreserveValue()
    {
        const string json = """
            {
                "cityId": 101010100,
                "secondaryname": ""
            }
            """;

        var result = JsonSerializer.Deserialize<CityInfo>(json, MojiJsonContext.SerializerOptions);

        result.Should().NotBeNull();
        result!.SecondaryName.Should().BeEmpty();
    }
}

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
}

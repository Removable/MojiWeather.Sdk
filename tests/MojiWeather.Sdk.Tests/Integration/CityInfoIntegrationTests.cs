using System.Text.Json;
using FluentAssertions;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.Weather;
using MojiWeather.Sdk.Serialization;
using Xunit;

namespace MojiWeather.Sdk.Tests.Integration;

public class CityInfoIntegrationTests
{
    [Fact]
    public void Deserialize_BriefConditionResponse_ShouldIncludeCityInfo()
    {
        const string json = """
            {
                "code": 0,
                "msg": "success",
                "data": {
                    "city": {
                        "cityId": 101010100,
                        "counname": "China",
                        "ianatimezone": "Asia/Shanghai",
                        "name": "Beijing",
                        "pname": "Beijing",
                        "timezone": "+08:00",
                        "secondaryname": "Beijing"
                    },
                    "condition": {
                        "temp": "20",
                        "humidity": "60",
                        "vis": "10",
                        "windDegrees": "90",
                        "windLevel": "2",
                        "condition": "Sunny",
                        "icon": "00",
                        "updatetime": "2024-01-01 10:00",
                        "windDir": "E",
                        "pressure": 1010,
                        "realFeel": 20,
                        "tips": "Comfortable"
                    }
                }
            }
            """;

        var result = JsonSerializer.Deserialize<ApiResponse<BriefConditionData>>(json, MojiJsonContext.SerializerOptions);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.City.Should().NotBeNull();
        result.Data.City!.CountryName.Should().Be("China");
        result.Data.City.IanaTimezone.Should().Be("Asia/Shanghai");
        result.Data.City.SecondaryName.Should().Be("Beijing");
    }
}

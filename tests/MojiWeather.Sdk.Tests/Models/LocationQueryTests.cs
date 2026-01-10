using System.Globalization;
using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using Xunit;

namespace MojiWeather.Sdk.Tests.Models;

public class LocationQueryTests
{
    [Fact]
    public void FromCoordinates_ShouldCreateCoordinatesQuery()
    {
        // 准备与执行
        var query = LocationQuery.FromCoordinates(39.9042, 116.4074);

        // 断言
        query.Should().BeOfType<CoordinatesQuery>();
        query.IsCoordinatesQuery.Should().BeTrue();

        var coordsQuery = (CoordinatesQuery)query;
        coordsQuery.Lat.Should().Be(39.9042);
        coordsQuery.Lon.Should().Be(116.4074);
    }

    [Fact]
    public void FromCityId_ShouldCreateCityIdQuery()
    {
        // 准备与执行
        var query = LocationQuery.FromCityId(284609);

        // 断言
        query.Should().BeOfType<CityIdQuery>();
        query.IsCoordinatesQuery.Should().BeFalse();

        var cityIdQuery = (CityIdQuery)query;
        cityIdQuery.CityId.Should().Be(284609);
    }

    [Fact]
    public void CoordinatesQuery_ShouldStoreCoordinates()
    {
        // 准备
        var query = LocationQuery.FromCoordinates(39.9042, 116.4074);

        // 断言
        var coordsQuery = (CoordinatesQuery)query;
        coordsQuery.Lat.Should().BeApproximately(39.9042, 0.0001);
        coordsQuery.Lon.Should().BeApproximately(116.4074, 0.0001);
    }

    [Fact]
    public void CityIdQuery_ShouldStoreCityId()
    {
        // 准备
        var query = LocationQuery.FromCityId(284609);

        // 断言
        var cityIdQuery = (CityIdQuery)query;
        cityIdQuery.CityId.Should().Be(284609);
    }

    [Theory]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void FromCoordinates_WithBoundaryValues_ShouldWork(double lat, double lon)
    {
        // 准备与执行
        var query = LocationQuery.FromCoordinates(lat, lon);

        // 断言
        var coordsQuery = (CoordinatesQuery)query;
        coordsQuery.Lat.Should().Be(lat);
        coordsQuery.Lon.Should().Be(lon);

        var parameters = query.ToFormParameters();
        parameters["lat"].Should().Be(lat.ToString(CultureInfo.InvariantCulture));
        parameters["lon"].Should().Be(lon.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void FromCityId_WithZeroOrNegative_ShouldStoreValue(long cityId)
    {
        // 准备与执行
        var query = LocationQuery.FromCityId(cityId);

        // 断言
        var cityIdQuery = (CityIdQuery)query;
        cityIdQuery.CityId.Should().Be(cityId);

        var parameters = query.ToFormParameters();
        parameters["cityId"].Should().Be(cityId.ToString());
    }
}

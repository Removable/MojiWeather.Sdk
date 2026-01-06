using FluentAssertions;
using MojiWeather.Sdk.Abstractions;
using Xunit;

namespace MojiWeather.Sdk.Tests.Models;

public class LocationQueryTests
{
    [Fact]
    public void FromCoordinates_ShouldCreateCoordinatesQuery()
    {
        // Arrange & Act
        var query = LocationQuery.FromCoordinates(39.9042, 116.4074);

        // Assert
        query.Should().BeOfType<CoordinatesQuery>();
        query.IsCoordinatesQuery.Should().BeTrue();

        var coordsQuery = (CoordinatesQuery)query;
        coordsQuery.Lat.Should().Be(39.9042);
        coordsQuery.Lon.Should().Be(116.4074);
    }

    [Fact]
    public void FromCityId_ShouldCreateCityIdQuery()
    {
        // Arrange & Act
        var query = LocationQuery.FromCityId(284609);

        // Assert
        query.Should().BeOfType<CityIdQuery>();
        query.IsCoordinatesQuery.Should().BeFalse();

        var cityIdQuery = (CityIdQuery)query;
        cityIdQuery.CityId.Should().Be(284609);
    }

    [Fact]
    public void CoordinatesQuery_ShouldStoreCoordinates()
    {
        // Arrange
        var query = LocationQuery.FromCoordinates(39.9042, 116.4074);

        // Assert
        var coordsQuery = (CoordinatesQuery)query;
        coordsQuery.Lat.Should().BeApproximately(39.9042, 0.0001);
        coordsQuery.Lon.Should().BeApproximately(116.4074, 0.0001);
    }

    [Fact]
    public void CityIdQuery_ShouldStoreCityId()
    {
        // Arrange
        var query = LocationQuery.FromCityId(284609);

        // Assert
        var cityIdQuery = (CityIdQuery)query;
        cityIdQuery.CityId.Should().Be(284609);
    }
}

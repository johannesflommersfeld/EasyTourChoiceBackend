using Microsoft.Extensions.Logging;
using NSubstitute;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models.BaseModels;

namespace EasyTourChoice.API.Test.Services;

public class EAWSRegionServiceTest
{
    private ILogger<EAWSRegionService> _loggerMock;
    private IHttpService _httpServiceMock;
    private FileStream _regionsStream;

    [SetUp]
    public void SetUp()
    {
        _regionsStream = new(Path.Combine("resources", "EAWSRegionsMinimal.json"), FileMode.Open, FileAccess.Read);
        _loggerMock = Substitute.For<ILogger<EAWSRegionService>>();
        _httpServiceMock = Substitute.For<IHttpService>();
        _httpServiceMock.PerformGetRequestAsync(string.Empty).ReturnsForAnyArgs(_regionsStream);
    }

    [TearDown]
    public void TearDown()
    {
        _regionsStream.Dispose();
    }

    [Test]
    public void LoadRegions_LoadedCorrectly()
    {
        // arrange
        var regionService = new EAWSRegionService(_loggerMock, _httpServiceMock);

        // act
        regionService.LoadRegionsAsync().Wait();

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(regionService.Regions, Is.Not.Null);
            Assert.That(regionService.Regions!.Features, Has.Count.EqualTo(2));
            Assert.That(regionService.Regions!.Type, Is.EqualTo(RegionsType.FeatureCollection));
            foreach (Feature feature in regionService.Regions.Features)
            {
                Assert.That(feature.Type, Is.EqualTo(FeatureType.Feature));
                Assert.That(feature.Properties.Id, Is.Not.Empty);
                Assert.That(feature.Geometry, Is.Not.Null);
                Assert.That(feature.Geometry!.Type, Is.EqualTo(GeometryType.MultiPolygon));
                Assert.That(feature.Geometry!.Coordinates, Has.Count.GreaterThan(0));
            }
        });
    }

    [TestCase(1.6, 42.6, "AD-01")]
    [TestCase(25.2, 48.0, "UA-05")]
    [TestCase(1, 30, null)]
    public void GetRegionID_PointInRegion_ReturnsCorrectIDOrNull(double longitude, double latitude, string? expectedID)
    {
        // arrange
        var regionService = new EAWSRegionService(_loggerMock, _httpServiceMock);
        regionService.LoadRegionsAsync().Wait();
        var location = new LocationBase() { Longitude = longitude, Latitude = latitude };

        // act, assert
        Assert.That(regionService.GetRegionID(location), Is.EqualTo(expectedID));
    }

    [Test]
    public void GetRegionIDWithoutLoadingFirst_NullReferenceExceptionThrown()
    {
        // arrange
        var regionService = new EAWSRegionService(_loggerMock, _httpServiceMock);
        var location = new LocationBase() { Longitude = 0, Latitude = 0 };

        // act, assert
        Assert.Throws<NullReferenceException>(() => regionService.GetRegionID(location));
    }
}
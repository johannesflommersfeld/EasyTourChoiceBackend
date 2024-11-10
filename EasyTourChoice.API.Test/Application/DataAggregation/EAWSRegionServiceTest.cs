using Microsoft.Extensions.Logging;
using NSubstitute;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models.BaseModels;
using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Repositories;

namespace EasyTourChoice.API.Test.Application.DataAggregation;

public class EAWSRegionServiceTest
{
    private ILogger<EAWSRegionService> _loggerMock;
    private IHttpService _httpServiceMock;
    private FileStream _regionsStream;
    private IAvalancheRegionsRepository _regionRepo;


    [SetUp]
    public void SetUp()
    {
        _regionsStream = new(Path.Combine("resources", "EAWSRegionsMinimal.json"), FileMode.Open, FileAccess.Read);
        _loggerMock = Substitute.For<ILogger<EAWSRegionService>>();
        _httpServiceMock = Substitute.For<IHttpService>();
        _httpServiceMock.PerformGetRequestAsync(string.Empty).ReturnsForAnyArgs(_regionsStream);
        _regionRepo = new AvalancheRegionsRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _regionsStream.Dispose();
    }

    [TestCase(1.6, 42.6, "AD-01")]
    [TestCase(25.2, 48.0, "UA-05")]
    [TestCase(1, 30, null)]
    public async Task GetRegionID_PointInRegion_ReturnsCorrectIDOrNull(double longitude, double latitude, string? expectedID)
    {
        // arrange
        var regionService = new EAWSRegionService(_loggerMock, _httpServiceMock, _regionRepo);
        var location = new LocationBase() { Longitude = longitude, Latitude = latitude };

        var id = await regionService.GetRegionIDAsync(location);

        // assert
        Assert.That(id, Is.EqualTo(expectedID));
    }
}
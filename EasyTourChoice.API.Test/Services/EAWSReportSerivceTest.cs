using Microsoft.Extensions.Logging;
using NSubstitute;
using EasyTourChoice.API.Services;
using AutoMapper;
using EasyTourChoice.API.Profiles;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Test.Services;

public class EAWSReportServiceTest
{
    private ILogger<EAWSReportService> _loggerMock;
    private IHttpService _httpServiceMock;
    private FileStream _reportStream;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _reportStream = new(Path.Combine("resources", "EAWSReport.json"), FileMode.Open, FileAccess.Read);
        _loggerMock = Substitute.For<ILogger<EAWSReportService>>();
        _httpServiceMock = Substitute.For<IHttpService>();
        _httpServiceMock.PerformGetRequestAsync(string.Empty).ReturnsForAnyArgs(_reportStream);
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AvalancheReportProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
    }

    [TearDown]
    public void TearDown()
    {
        _reportStream.Dispose();
    }

    [Test]
    public async Task LoadRegions_LoadedCorrectly()
    {
        // arrange
        var reportService = new EAWSReportService(_loggerMock, _httpServiceMock, _mapper);

        // act
        var bulletin = await reportService.GetLatestAvalancheReportAsync("IT-32-TN-11");

        // assert
        Assert.That(bulletin, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(bulletin.Tendency, Is.EqualTo(TendencyType.STEADY));
            Assert.That(bulletin.AvalancheProblems, Has.Count.EqualTo(2));
            Assert.That(bulletin.AvalancheProblems[0].ProblemType, Is.EqualTo(AvalancheProblemType.WIND_SLAB));
            Assert.That(bulletin.AvalancheProblems[0].ProblemType, Is.EqualTo(AvalancheProblemType.WIND_SLAB));
            Assert.That(bulletin.DangerRatings, Has.Count.EqualTo(2));
            Assert.That(bulletin.DangerRatings[0].MainValue, Is.EqualTo(AvalancheDangerRating.LOW));
            Assert.That(bulletin.DangerRatings[0].Elevation.UpperBound, Is.EqualTo("2600"));
            Assert.That(bulletin.DangerRatings[0].Elevation.LowerBound, Is.Null);
            Assert.That(bulletin.DangerRatings[1].MainValue, Is.EqualTo(AvalancheDangerRating.MODERATE));
            Assert.That(bulletin.DangerRatings[1].Elevation.UpperBound, Is.Null);
            Assert.That(bulletin.DangerRatings[1].Elevation.LowerBound, Is.EqualTo("2600"));
        });
    }
}
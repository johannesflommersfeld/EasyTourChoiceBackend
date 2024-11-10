using Microsoft.Extensions.Logging;
using NSubstitute;
using AutoMapper;
using EasyTourChoice.API.Application.DataAggregation;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Profiles;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Repositories;

namespace EasyTourChoice.API.Test.Application.DataAggregation;

public class EAWSReportServiceTest
{
    private ILogger<EAWSReportService> _loggerMock;
    private IHttpService _httpServiceMock;
    private FileStream _reportStream; 
    private IMapper _mapper;
    private IAvalancheReportsRepository _avalancheReportsRepository;

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
            mc.AddProfile(new AvalancheProblemProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
        _avalancheReportsRepository = new AvalancheReportsRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _reportStream.Dispose();
    }

    [Test]
    public async Task GetReport_MustBeValid_ReturnsNull()
    {
        // arrange
        var reportService = new EAWSReportService(_loggerMock, _httpServiceMock, _mapper, _avalancheReportsRepository);

        // act
        var bulletin = await reportService.GetAvalancheReportAsync("IT-32-TN-11", true);

        // assert
        Assert.That(bulletin, Is.Null);
    }

    [Test]
    public async Task GetReport_IgnoreValidity_LoadedCorrectly()
    {
        // arrange
        var reportService = new EAWSReportService(_loggerMock, _httpServiceMock, _mapper, _avalancheReportsRepository);

        // act
        var bulletin = await reportService.GetAvalancheReportAsync("IT-32-TN-11", false);

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
            Assert.That(bulletin.DangerRatings[0].UpperBound, Is.EqualTo("2600"));
            Assert.That(bulletin.DangerRatings[0].LowerBound, Is.Null);
            Assert.That(bulletin.DangerRatings[1].MainValue, Is.EqualTo(AvalancheDangerRating.MODERATE));
            Assert.That(bulletin.DangerRatings[1].UpperBound, Is.Null);
            Assert.That(bulletin.DangerRatings[1].LowerBound, Is.EqualTo("2600"));
        });
    }
}
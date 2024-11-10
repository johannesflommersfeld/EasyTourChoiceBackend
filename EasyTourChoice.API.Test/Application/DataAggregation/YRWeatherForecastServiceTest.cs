using Microsoft.Extensions.Logging;
using NSubstitute;
using AutoMapper;
using EasyTourChoice.API.Application.DataAggregation;
using EasyTourChoice.API.Application.Profiles;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories;

namespace EasyTourChoice.API.Test.Application.DataAggregation;

public class YRWeatherForecastServiceTest
{
    private ILogger<YRWeatherForecastService> _loggerMock;
    private IHttpService _httpServiceMock;
    private FileStream _regionsStream;
    private IMapper _mapper;
    private WeatherForecastRepository _forecastRepo;

    [SetUp]
    public void SetUp()
    {
        _regionsStream = new(Path.Combine("resources", "YRForecast.json"), FileMode.Open, FileAccess.Read);
        _loggerMock = Substitute.For<ILogger<YRWeatherForecastService>>();
        _httpServiceMock = Substitute.For<IHttpService>();
        _httpServiceMock.PerformGetRequestAsync(string.Empty, string.Empty).ReturnsForAnyArgs(_regionsStream);
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new WeatherForecastProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
        _forecastRepo = new WeatherForecastRepository();
    }

    [TearDown]
    public void TearDown()
    {
        _regionsStream.Dispose();
    }

    [Test]
    public async Task LoadWeatherForecast_LoadedCorrectly()
    {
        // arrange
        var forecastService = new YRWeatherForecastService(_loggerMock, _httpServiceMock, _mapper, _forecastRepo);

        // act
        var forecast = await forecastService.GetWeatherForecastAsync(new Location());

        // assert
        Assert.That(forecast, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(forecast.Meta.UpdatedAt, Is.EqualTo(new DateTime(2024, 9, 28, 17, 16, 10)));
            Assert.That(forecast.Meta.Units.AirTemperature, Is.EqualTo("celsius"));
            Assert.That(forecast.Timeseries, Has.Count.EqualTo(28));
        });
        Assert.Multiple(() =>
        {
            Assert.That(forecast.Timeseries[0].Time, Is.EqualTo(new DateTime(2024, 9, 28, 17, 00, 00)));
            Assert.That(forecast.Timeseries[1].Time, Is.EqualTo(new DateTime(2024, 9, 28, 18, 00, 00)));
            Assert.That(forecast.Timeseries[2].Time, Is.EqualTo(new DateTime(2024, 9, 28, 19, 00, 00)));
            Assert.That(forecast.Timeseries[0].Data.Instant.AirTemperature,
                Is.EqualTo(7.0).Within(Tolerances.DOUBLE_EPS));
            Assert.That(forecast.Timeseries[0].Data.Instant.WindSpeed,
                Is.EqualTo(3.3).Within(Tolerances.DOUBLE_EPS));
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.SymbolCode,
                Is.EqualTo(WeatherSymbolDto.PARTLY_CLOUDY_NIGHT));
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.PrecipitationAmount,
                Is.EqualTo(0.0).Within(Tolerances.DOUBLE_EPS));
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.AirTemperatureMax, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.AirTemperatureMin, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.PrecipitationAmountMax, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.PrecipitationAmountMin, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.ProbabilityOfPrecipitation, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.ProbabilityOfThunder, Is.Null);
            Assert.That(forecast.Timeseries[0].Data.NextOneHours.Details!.UVIndexClearSkyMax, Is.Null);
        });
    }
}
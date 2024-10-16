using AutoMapper;
using EasyTourChoice.API.Application.DataAggregation;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;

public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<YRForecastUnits, ForecastUnits>();
        CreateMap<YRMeta, Meta>();
        CreateMap<YRForecastTimeInstant, ForecastTimeInstant>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Details, dest));
        CreateMap<YRForecastTimeInstantDetails, ForecastTimeInstant>();
        CreateMap<YRForecastTimePeriodDetails, ForecastTimePeriodDetails>();
        CreateMap<YRForecastTimePeriod, ForecastTimePeriod>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Summary, dest));
        CreateMap<YRForecastSummary, ForecastTimePeriod>();
        CreateMap<YRForecastData, ForecastData>();
        CreateMap<YRForecastTimeStep, ForecastTimeStep>();
        CreateMap<YRResponse, WeatherForecast>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Forecast, dest));
        CreateMap<YRForecast, WeatherForecast>();

        CreateMap<Meta, MetaDto>();
        CreateMap<ForecastUnits, ForecastUnitsDto>();
        CreateMap<ForecastTimeStep, ForecastTimeStepDto>();
        CreateMap<ForecastData, ForecastDataDto>();
        CreateMap<ForecastTimeInstant, ForecastTimeInstantDto>();
        CreateMap<ForecastTimePeriod, ForecastTimePeriodDto>();
        CreateMap<ForecastTimePeriodDetails, ForecastTimePeriodDetailsDto>();
        CreateMap<WeatherForecast, WeatherForecastDto>();
    }
}
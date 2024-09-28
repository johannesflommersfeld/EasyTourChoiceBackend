using AutoMapper;
using EasyTourChoice.API.Models;
using EasyTourChoice.API.Services;

namespace EasyTourChoice.API.Profiles;
public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<ForecastUnits, ForecastUnitsDto>();
        CreateMap<Meta, MetaDto>();
        CreateMap<ForecastTimeInstant, ForecastTimeInstantDto>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Details, dest));
        CreateMap<ForecastTimeInstantDetails, ForecastTimeInstantDto>();
        CreateMap<ForecastTimePeriodDetails, ForecastTimePeriodDetailsDto>();
        CreateMap<ForecastTimePeriod, ForecastTimePeriodDto>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Summary, dest));
        CreateMap<ForecastSummary, ForecastTimePeriodDto>();
        CreateMap<ForecastData, ForecastDataDto>();
        CreateMap<ForecastTimeStep, ForecastTimeStepDto>();
        CreateMap<YRResponse, WeatherForecastDto>()
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Forecast, dest));
        CreateMap<Forecast, WeatherForecastDto>();
    }
}
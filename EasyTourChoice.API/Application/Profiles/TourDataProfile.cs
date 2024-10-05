using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
namespace EasyTourChoice.API.Application.Profiles;

public class TourDataProfile : Profile
{
    public TourDataProfile()
    {
        CreateMap<TourData, TourDataDto>();
        CreateMap<TourDataForCreationDto, TourData>();
        CreateMap<TourDataForUpdateDto, TourData>();
        CreateMap<TourData, TourDataForUpdateDto>();
    }
}
using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class TourDataProfile : Profile
{
    public TourDataProfile()
    {
        CreateMap<TourDataForCreationDto, TourData>();
        CreateMap<TourData, TourDataDto>();
    }
}
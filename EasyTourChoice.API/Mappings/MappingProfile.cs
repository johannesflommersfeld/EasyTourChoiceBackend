using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TourDataForCreationDto, TourData>();
        CreateMap<TourData, TourDataDto>();
        CreateMap<IEnumerable<TourData>, IEnumerable<TourDataDto>>();
    }
}
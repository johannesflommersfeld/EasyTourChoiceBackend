using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;
public class AreaProfile : Profile
{
    public AreaProfile()
    {
        CreateMap<Area, AreaDto>()
            .ForMember(dest => dest.NumberOfTours, opt => opt.MapFrom<TourNumberResolver>());
        CreateMap<AreaForCreationDto, Area>();
        CreateMap<AreaForUpdateDto, Area>();
        CreateMap<Area, AreaForUpdateDto>();
    }
}

public class TourNumberResolver : IValueResolver<Area, AreaDto, uint>
{
    public uint Resolve(Area source, AreaDto destination, uint member, ResolutionContext context)
    {
        return (uint)source.Tours.Count;
    }
}
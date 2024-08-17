using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class AreaProfile : Profile
{
    public AreaProfile()
    {
        CreateMap<Area, AreaDto>()
            .ForMember(dest => dest.NumberOfTours, opt => opt.MapFrom<TourNumberResolver>());
        CreateMap<AreaForCreationDto, Area>();
    }
}

public class TourNumberResolver : IValueResolver<Area, AreaDto, uint>
{
	public uint Resolve(Area source, AreaDto destination, uint member, ResolutionContext context)
	{
        return (uint)source.Tours.Count;
	}
}
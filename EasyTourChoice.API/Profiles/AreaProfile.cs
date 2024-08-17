using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class AreaProfile : Profile
{
    public AreaProfile()
    {
        CreateMap<Area, AreaDto>();
        CreateMap<AreaDto, Area>();
        CreateMap<AreaForCreationDto, Area>();
    }
}
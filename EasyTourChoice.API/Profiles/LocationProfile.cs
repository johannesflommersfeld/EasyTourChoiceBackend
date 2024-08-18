using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<LocationForCreationDto, Location>();
        CreateMap<LocationForUpdateDto, Location>();
        CreateMap<Location, LocationForUpdateDto>();
    }
}
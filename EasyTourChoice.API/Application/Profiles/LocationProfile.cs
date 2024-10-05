using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;


namespace EasyTourChoice.API.Application.Profiles;

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
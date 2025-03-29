using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
using AutoMapper;

namespace EasyTourChoice.API.Application.Profiles;

public class TravelInformationProfile: Profile
{
    public TravelInformationProfile()
    {
        CreateMap<TravelInformation, TravelInformationDto>();
        CreateMap<TravelInformation, TravelInformationWithRouteDto>();
    }
}
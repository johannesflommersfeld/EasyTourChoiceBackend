using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;

public class AvalancheReportProfile : Profile
{
    public AvalancheReportProfile()
    {
        CreateMap<AvalancheProblem, AvalancheProblemDto>();
        CreateMap<DangerRating, DangerRatingDto>();
        CreateMap<AvalancheReport, AvalancheReportDto>();
    }
}
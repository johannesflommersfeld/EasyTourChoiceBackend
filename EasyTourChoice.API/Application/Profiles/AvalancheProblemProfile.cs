using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;

public class AvalancheProblemProfile : Profile
{
    public AvalancheProblemProfile()
    {
        CreateMap<EAWSAvalancheProblem, AvalancheProblemDto>()
            .ForMember(dest => dest.Aspect, opt => opt.MapFrom<AspectResolver>());
    }
}

public class AspectResolver : IValueResolver<EAWSAvalancheProblem, AvalancheProblemDto, Aspect>
{
    public Aspect Resolve(EAWSAvalancheProblem source, AvalancheProblemDto destination, Aspect member, ResolutionContext context)
    {
        var aspect = Aspect.UNKNOWN;
        foreach (var a in source.Aspects)
        {
            aspect |= a;
        }
        return aspect;
    }
}
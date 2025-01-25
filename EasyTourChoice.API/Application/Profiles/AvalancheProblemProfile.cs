using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;

public class AvalancheProblemProfile : Profile
{
    public AvalancheProblemProfile()
    {
        CreateMap<EAWSAvalancheProblem, AvalancheProblemDto>()
            .ForMember(dest => dest.Aspect, opt => opt.MapFrom<AspectResolverForDto>());
    }
}

public class AspectResolverForDto : IValueResolver<EAWSAvalancheProblem, AvalancheProblemDto, Aspect>
{
    public Aspect Resolve(EAWSAvalancheProblem source, AvalancheProblemDto destination, Aspect member, ResolutionContext context)
    {
        if (source.Aspects.Count == 0)
        {
            return (Aspect)0b1111_1111;
        }

        var aspect = Aspect.UNKNOWN;
        foreach (var a in source.Aspects)
        {
            aspect |= a;
        }
        return aspect;
    }
}
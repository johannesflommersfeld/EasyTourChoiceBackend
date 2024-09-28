using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class AvalancheProblemProfile : Profile
{
    public AvalancheProblemProfile()
    {
        CreateMap<AvalancheProblem, AvalancheProblemDto>()
            .ForMember(dest => dest.Aspect, opt => opt.MapFrom<AspectResolver>());
    }
}

public class AspectResolver : IValueResolver<AvalancheProblem, AvalancheProblemDto, Aspect>
{
    public Aspect Resolve(AvalancheProblem source, AvalancheProblemDto destination, Aspect member, ResolutionContext context)
    {
        var aspect = Aspect.UNKNOWN;
        foreach (var a in source.Aspects)
        {
            aspect |= a;
        }
        return aspect;
    }
}
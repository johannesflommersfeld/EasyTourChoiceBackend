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

        CreateMap<EAWSBulletin, AvalancheReport>()
            .ForMember(dest => dest.ReportBody, opt => opt.MapFrom<ReportBodyResolver>())
            .ForMember(dest => dest.Tendency, opt => opt.MapFrom<TendencyTypeResolver>())
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom<StartTimeResolver>())
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom<EndTimeResolver>());
        CreateMap<EAWSAvalancheProblem, AvalancheProblem>()
            .ForMember(dest => dest.UpperBound, opt => opt.MapFrom<AvalancheProblemUpperBoundResolver>())
            .ForMember(dest => dest.LowerBound, opt => opt.MapFrom<AvalancheProblemLowerBoundResolver>())
            .ForMember(dest => dest.Aspect, opt => opt.MapFrom<AspectResolver>());
        CreateMap<EAWSDangerRating, DangerRating>()
            .ForMember(dest => dest.UpperBound, opt => opt.MapFrom<DangerRatingUpperBoundResolver>())
            .ForMember(dest => dest.LowerBound, opt => opt.MapFrom<DangerRatingLowerBoundResolver>());
    }
}

public class ReportBodyResolver : IValueResolver<EAWSBulletin, AvalancheReport, Dictionary<string, List<string>>>
{
    public Dictionary<string, List<string>> Resolve(EAWSBulletin source, AvalancheReport destination,
        Dictionary<string, List<string>> member, ResolutionContext context)
    {
        Dictionary<string, List<string>> body = [];
        body["Avalanche activity"] = [source.AvalancheActivity.Highlights, source.AvalancheActivity.Comment];
        body["Snowpack structure"] = [source.SnowpackStructure.Highlights, source.SnowpackStructure.Comment];
        List<string> tendencies = [];
        foreach (var tendency in source.Tendency)
        {
            if (tendency.Highlights != string.Empty)
            {
                tendencies.Add(tendency.Highlights);
            }
            if (tendency.Highlights != string.Empty)
            {
                tendencies.Add(tendency.Comment);
            }
        }
        body["TendencyText"] = tendencies;
        body["Travel advisory"] = [source.TravelAdvisory.Highlights, source.TravelAdvisory.Comment];
        return body;
    }
}

public class TendencyTypeResolver : IValueResolver<EAWSBulletin, AvalancheReport, TendencyType>
{
    public TendencyType Resolve(EAWSBulletin source, AvalancheReport destination,
        TendencyType member, ResolutionContext context)
    {
        var types = source.Tendency.Select(t => t.TendencyType).ToList();
        return types.Any(t => t != types[0]) ? TendencyType.UNKNOWN : types[0];
    }
}

public class StartTimeResolver : IValueResolver<EAWSBulletin, AvalancheReport, DateTime>
{
    public DateTime Resolve(EAWSBulletin source, AvalancheReport destination,
        DateTime member, ResolutionContext context)
    {
        return source.ValidTime.StartTime;
    }
}

public class EndTimeResolver : IValueResolver<EAWSBulletin, AvalancheReport, DateTime>
{
    public DateTime Resolve(EAWSBulletin source, AvalancheReport destination,
        DateTime member, ResolutionContext context)
    {
        return source.ValidTime.EndTime;
    }
}

public class AvalancheProblemUpperBoundResolver : IValueResolver<EAWSAvalancheProblem, AvalancheProblem, string?>
{
    public string? Resolve(EAWSAvalancheProblem source, AvalancheProblem destination, string? member, ResolutionContext context)
    {
        return source.Elevation.UpperBound;
    }
}

public class AvalancheProblemLowerBoundResolver : IValueResolver<EAWSAvalancheProblem, AvalancheProblem, string?>
{
    public string? Resolve(EAWSAvalancheProblem source, AvalancheProblem destination, string? member, ResolutionContext context)
    {
        return source.Elevation.LowerBound;
    }
}

public class AspectResolver : IValueResolver<EAWSAvalancheProblem, AvalancheProblem, Aspect>
{
    public Aspect Resolve(EAWSAvalancheProblem source, AvalancheProblem destination, Aspect member, ResolutionContext context)
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

public class DangerRatingUpperBoundResolver : IValueResolver<EAWSDangerRating, DangerRating, string?>
{
    public string? Resolve(EAWSDangerRating source, DangerRating destination, string? member, ResolutionContext context)
    {
        return source.Elevation?.UpperBound;
    }
}

public class DangerRatingLowerBoundResolver : IValueResolver<EAWSDangerRating, DangerRating, string?>
{
    public string? Resolve(EAWSDangerRating source, DangerRating destination, string? member, ResolutionContext context)
    {
        return source.Elevation?.LowerBound;
    }
}
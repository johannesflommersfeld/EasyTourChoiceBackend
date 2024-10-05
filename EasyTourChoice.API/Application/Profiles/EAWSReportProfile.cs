using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Profiles;

public class EAWSReportProfile : Profile
{
    public EAWSReportProfile()
    {
        CreateMap<EAWSBulletin, AvalancheReport>()
            .ForMember(dest => dest.ReportBody, opt => opt.MapFrom<ReportBodyResolver>())
            .ForMember(dest => dest.Tendency, opt => opt.MapFrom<TendencyTypeResolver>());
        CreateMap<EAWSAvalancheProblem, AvalancheProblem>();
        CreateMap<EAWSDangerRating, DangerRating>();
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
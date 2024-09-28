using AutoMapper;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Profiles;
public class AvalancheReportProfile : Profile
{
    public AvalancheReportProfile()
    {
        CreateMap<EAWSBulletin, AvalancheReportDto>()
            .ForMember(dest => dest.ReportBody, opt => opt.MapFrom<ReportBodyResolver>())
            .ForMember(dest => dest.Tendency, opt => opt.MapFrom<TendencyTypeResolver>());
    }
}

public class ReportBodyResolver : IValueResolver<EAWSBulletin, AvalancheReportDto, Dictionary<string, List<string>>>
{
    public Dictionary<string, List<string>> Resolve(EAWSBulletin source, AvalancheReportDto destination,
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

public class TendencyTypeResolver : IValueResolver<EAWSBulletin, AvalancheReportDto, TendencyType>
{
    public TendencyType Resolve(EAWSBulletin source, AvalancheReportDto destination,
        TendencyType member, ResolutionContext context)
    {
        var types = source.Tendency.Select(t => t.TendencyType).ToList();
        return types.Any(t => t != types[0]) ? TendencyType.UNKNOWN : types[0];
    }
}
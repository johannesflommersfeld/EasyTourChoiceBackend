using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class AvalancheReportDto
{
    required public DateTime PublicationTime { get; set; }

    required public List<AvalancheProblemDto> AvalancheProblems { get; set; }

    required public List<DangerRating> DangerRatings { get; set; }

    required public Dictionary<string, List<string>> ReportBody { get; set; }

    required public TendencyType Tendency { get; set; }

    public string? RegionName { get; set; }
}
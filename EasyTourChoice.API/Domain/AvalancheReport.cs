using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Domain;

public record AvalancheReport
{
    required public DateTime PublicationTime { get; set; }

    required public DateTime StartTime { get; set; }

    required public DateTime EndTime { get; set; }

    required public List<AvalancheProblem> AvalancheProblems { get; set; }

    required public List<DangerRating> DangerRatings { get; set; }

    required public Dictionary<string, List<string>> ReportBody { get; set; }

    required public TendencyType Tendency { get; set; }

    public string? RegionName { get; set; }

    public string? RegionID { get; set; }

    public bool IsValid()
    {
        return (StartTime < DateTime.Now) && (EndTime > DateTime.Now);
    }
}

public record AvalancheProblem
{
    required public AvalancheProblemType ProblemType { get; set; }

    public string? UpperBound { get; set; }

    public string? LowerBound { get; set; }

    required public ValidTimePeriod ValidTimePeriod { get; set; }

    required public SnowpackStability SnowpackStability { get; set; }

    required public Frequency Frequency { get; set; }

    [AvalancheSize]
    required public int AvalancheSize { get; set; }

    required public Aspect Aspect { get; set; }
}

public record DangerRating
{
    required public AvalancheDangerRating MainValue { get; set; }

    public string? UpperBound { get; set; }

    public string? LowerBound { get; set; }

    required public ValidTimePeriod ValidTimePeriod { get; set; }
}
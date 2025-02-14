using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Domain;

public record AvalancheReport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    required public DateTime PublicationTime { get; set; }

    required public DateTime StartTime { get; set; }

    required public DateTime EndTime { get; set; }

    required public List<AvalancheProblem> AvalancheProblems { get; set; }
    
    required public List<DangerRating> DangerRatings { get; set; }

    required public Dictionary<string, List<string>> ReportBody { get; set; }

    required public TendencyType Tendency { get; set; }

    public string? RegionName { get; set; }

    public string? RegionId { get; set; }

    public bool IsValid()
    {
        return (StartTime < DateTime.Now) && (EndTime > DateTime.Now);
    }
}

public record AvalancheProblem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    required public AvalancheProblemType ProblemType { get; set; }

    public string? UpperBound { get; set; }

    public string? LowerBound { get; set; }

    required public ValidTimePeriod ValidTimePeriod { get; set; }

    required public SnowpackStability SnowpackStability { get; set; }

    required public Frequency Frequency { get; set; }

    [AvalancheSize]
    required public int AvalancheSize { get; set; }

    required public Aspect Aspect { get; set; }
    
    required public List<AvalancheReport> AvalancheReports { get; set; }
}

public record DangerRating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    required public AvalancheDangerRating MainValue { get; set; }

    public string? UpperBound { get; set; }

    public string? LowerBound { get; set; }

    public required ValidTimePeriod ValidTimePeriod { get; set; }
    
    required public List<AvalancheReport> AvalancheReports { get; set; }

}
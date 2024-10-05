using EasyTourChoice.API.Domain;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Application.Models;

public class AvalancheProblemDto
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

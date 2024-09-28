using EasyTourChoice.API.Entities;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Models;

public class AvalancheProblemDto
{
    required public AvalancheProblemType ProblemType { get; set; }

    required public Elevation Elevation { get; set; }

    required public ValidTimePeriod ValidTimePeriod { get; set; }

    required public SnowpackStability SnowpackStability { get; set; }

    required public Frequency Frequency { get; set; }

    [AvalancheSize]
    required public int AvalancheSize { get; set; }

    required public Aspect Aspect { get; set; }
}

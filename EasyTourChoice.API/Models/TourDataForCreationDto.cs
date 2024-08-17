using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TourDataForCreationDto
{
    [Required(ErrorMessage = "You should provide a name value.")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public Activity ActivityType { get; set; }

    public Location? StartingLocation { get; set; }

    public Location? ActivityLocation { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; }// unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    public AreaDto? Area { get; set; }
}
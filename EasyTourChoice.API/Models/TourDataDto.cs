using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TourDataDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A tour needs to contain a name.")]
    [MaxLength(50)]
    public required string Name { get; set; } = string.Empty;

    public Activity ActivityType { get; set; }

    public Location? StartingLocation { get; set; }

    public Location? ActivityLocation { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; } // unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    public AreaDto? Area { get; set; }

    // derived fields
    public float? TravelTime { get; set; } // estimated travel time in hours

    public WeatherPreview? WeatherPreview { get; set; }

    public int? Temperature { get; set; }

    public AvelancheRisk? AvelancheRisk { get; set;}
}
using System.ComponentModel.DataAnnotations;

namespace EasyTourChoice.API.Models;

public class TourDataDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; } = string.Empty;

    public Activity ActivityType { get; set; }

    public uint? StartingLocationId { get; set; }

    public uint? ActivityLocationId { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public uint? Distance { get; set; } // total distance of the activity in km

    [MaxLength(120)]
    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; } // unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    public byte? Aspect { get; set; } // encodes the encountered aspects (see definition in Types.cs)

    public uint? AreaId { get; set; }

    // derived fields
    public float? TravelTime { get; set; } // estimated travel time in hours

    public WeatherPreview? WeatherPreview { get; set; }

    public int? Temperature { get; set; }

    public AvelancheRisk? AvelancheRisk { get; set;}
}
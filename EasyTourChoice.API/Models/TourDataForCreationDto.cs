using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TourDataForCreationDto
{
    [Required(ErrorMessage = "You should provide a name for the tour.")]
    [MaxLength(50)]
    public required string Name { get; set; }

    public Activity ActivityType { get; set; }

    public LocationForCreationDto? StartingLocation { get; set; }

    public LocationForCreationDto? ActivityLocation { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public uint? Distance { get; set; } // total distance of the activity in km

    [MaxLength(120)]
    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; }// unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    public byte? Aspect { get; set; } // encodes the encountered aspects (see definition in Types.cs)

    public AreaForCreationDto? Area { get; set; }
}
using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Application.Models.BaseModels;

public class TourBase
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    public Activity ActivityType { get; set; }

    public int? StartingLocationId { get; set; }

    public int? ActivityLocationId { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public uint? Distance { get; set; } // total distance of the activity in km

    [MaxLength(120)]
    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; } // unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    [Aspect]
    public byte? Aspect { get; set; } // encodes the encountered aspects (see definition in Types.cs)

    public int? AreaId { get; set; }

    public string? AvalancheRegionId { get; set; }
}

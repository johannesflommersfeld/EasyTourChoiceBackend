using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.ValidationAttributes;

namespace EasyTourChoice.API.Entities;

public class TourData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    public Activity ActivityType { get; set; }

    [Location]
    public Location? StartingLocation { get; set; }

    [ForeignKey("StartingLocation")]
    public int StartingLocationId { get; set; }

    [Location]
    public Location? ActivityLocation { get; set; }

    [ForeignKey("ActivityLocation")]
    public int ActivityLocationId { get; set; }

    public float? Duration { get; set; } // expected activity time in hours

    public float? ApproachDuration { get; set; } // expected approach time in hours

    public int? MetersOfElevation { get; set; }

    public int? Distance { get; set; } // total distance of the activity in km

    [MaxLength(120)]
    public string? ShortDescription { get; set; }

    public GeneralDifficulty? Difficulty { get; set; } // unit depends on the type of activity

    public RiskLevel? Risk { get; set; } // categories depend on the type of activity

    [Aspect]
    public byte? Aspect { get; set; } // encodes the encountered aspects (see definition in Types.cs)

    public Area? Area { get; set; }

    [ForeignKey("Area")]
    public int AreaId { get; set; }
}
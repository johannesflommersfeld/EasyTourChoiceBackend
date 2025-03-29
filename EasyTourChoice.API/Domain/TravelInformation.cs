using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Domain.ValidationAttributes;

namespace EasyTourChoice.API.Domain;

public class TravelInformation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? TravelInfoId { get; set; }

    [Location]
    [ForeignKey("StartingLocationId")]
    public Location? StartingLocation { get; set; }

    [Location]
    [ForeignKey("TargetLocationId")]
    public Location? TargetLocation { get; set; }
    
    public int? StartingLocationId { get; set; }

    public int? TargetLocationId { get; set; }
    
    public required float TravelTime; // traveling time with the car in hours
    
    public required float TravelDistance; // traveling time with the car in hours
    
    public List<Location>? Route; // should only be filled when requested to show on screen
}
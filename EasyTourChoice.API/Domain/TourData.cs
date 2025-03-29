using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EasyTourChoice.API.Application.Models.BaseModels;
using EasyTourChoice.API.Domain.ValidationAttributes;

namespace EasyTourChoice.API.Domain;

public class TourData : TourBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Location]
    [ForeignKey("StartingLocationId")]
    public Location? StartingLocation { get; set; }

    [Location]
    [ForeignKey("ActivityLocationId")]
    public Location? ActivityLocation { get; set; }
    
    [ForeignKey("AreaId")]
    public Area? Area { get; set; }
}
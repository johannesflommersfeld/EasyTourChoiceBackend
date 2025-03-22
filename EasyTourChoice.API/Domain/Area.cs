using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyTourChoice.API.Domain.ValidationAttributes;

namespace EasyTourChoice.API.Domain;

public class Area
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AreaId { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Location]
    [ForeignKey("LocationId")]
    public Location? Location { get; set; }
    
    [Required]
    public required int? LocationId { get; set; }

    public ICollection<TourData> Tours { get; set; } = [];
}
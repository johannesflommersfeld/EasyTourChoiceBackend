using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTourChoice.API.Entities;

public class Area
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AreaId { get; set; }

    [Required]
    public required string Name { get; set; }

    public ICollection<TourData> Tours { get; set; } = [];
}
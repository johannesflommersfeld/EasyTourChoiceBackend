using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTourChoice.API.Entities;

public class Location
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LocationId { get; set; }

    public double? Latitude { get; init; } // latitude with decimal minutes
    public double? Longitude { get; init; } // longitude with decimal minutes
    public double? Altitude { get; init; } // altitude in meter
}
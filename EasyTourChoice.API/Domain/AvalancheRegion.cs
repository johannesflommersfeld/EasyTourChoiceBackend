using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyTourChoice.API.Application.Models;

namespace EasyTourChoice.API.Domain;

public record AvalancheRegion
{
    [Key]
    public required string Id { get; init; }
    public required GeometryType Type { get; init; }
    public required ICollection<ICollection<ICollection<double>>> Polygons { get; init; }
}
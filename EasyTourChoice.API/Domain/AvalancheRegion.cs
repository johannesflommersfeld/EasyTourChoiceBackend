using EasyTourChoice.API.Application.Models;

namespace EasyTourChoice.API.Domain;

public record AvalancheRegion
{
    required public string Id { get; set; }
    required public GeometryType Type { get; set; }
    required public ICollection<ICollection<ICollection<double>>> Polygons { get; set; }
}
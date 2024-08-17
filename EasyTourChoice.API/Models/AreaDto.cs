namespace EasyTourChoice.API.Models;

public class AreaDto
{
    public int AreaId { get; set; }

    public required string Name { get; set; }

    // calculated field
    public uint NumberOfTours { get; set; }
}

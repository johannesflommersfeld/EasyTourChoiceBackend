using EasyTourChoice.API.Models.BaseModels;

namespace EasyTourChoice.API.Models;

public class TourDataForCreationDto : TourBase
{
    public LocationForCreationDto? StartingLocation { get; set; }

    public LocationForCreationDto? ActivityLocation { get; set; }

    public AreaForCreationDto? Area { get; set; }
}
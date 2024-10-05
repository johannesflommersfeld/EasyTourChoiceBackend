using EasyTourChoice.API.Application.Models.BaseModels;

namespace EasyTourChoice.API.Application.Models;

public class TourDataForCreationDto : TourBase
{
    public LocationForCreationDto? StartingLocation { get; set; }

    public LocationForCreationDto? ActivityLocation { get; set; }

    public AreaForCreationDto? Area { get; set; }
}
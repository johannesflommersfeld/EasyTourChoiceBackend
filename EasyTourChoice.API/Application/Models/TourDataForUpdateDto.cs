using EasyTourChoice.API.Application.Models.BaseModels;

namespace EasyTourChoice.API.Application.Models;

public class TourDataForUpdateDto : TourBase
{
    public LocationDto? StartingLocation { get; set; }

    public LocationDto? ActivityLocation { get; set; }
}
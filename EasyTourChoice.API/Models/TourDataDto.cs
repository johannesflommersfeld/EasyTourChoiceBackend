using EasyTourChoice.API.Models.BaseModels;

namespace EasyTourChoice.API.Models;

public class TourDataDto : TourBase
{
    public int Id { get; set; }

    // derived fields
    public TravelInformationDto? TravelDetails {get; set;}
}
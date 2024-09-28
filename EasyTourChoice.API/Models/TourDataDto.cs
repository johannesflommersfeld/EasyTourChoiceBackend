using EasyTourChoice.API.Models.BaseModels;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TourDataDto : TourBase
{
    public int Id { get; set; }

    // derived fields
    public TravelInformationDto? TravelDetails { get; set; }

    public AvalancheReportDto? Bulletin { get; set; }
}
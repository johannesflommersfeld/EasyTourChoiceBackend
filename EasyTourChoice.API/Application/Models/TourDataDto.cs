using EasyTourChoice.API.Application.Models.BaseModels;

namespace EasyTourChoice.API.Application.Models;

public class TourDataDto : TourBase
{
    public int Id { get; set; }

    // derived fields
    public TravelInformationDto? TravelDetails { get; set; }

    public AvalancheReportDto? Bulletin { get; set; }

    public WeatherForecastDto? WeatherForecast { get; set; }
}
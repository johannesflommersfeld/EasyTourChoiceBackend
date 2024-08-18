using EasyTourChoice.API.Models.BaseModels;

namespace EasyTourChoice.API.Models;

public class TourDataDto : TourBase
{
    public int Id { get; set; }

    // derived fields
    public float? TravelTime { get; set; } // estimated travel time in hours

    public TravelDetailsDto? TravelDetails {get; set;}

    public WeatherPreview? WeatherPreview { get; set; }

    public int? Temperature { get; set; }

    public WeatherForecastDto? WeatherForecast {get; set;}

    public AvelancheRisk? AvelancheRisk { get; set;}

    public AvalancheReportDto? AvalancheReport { get; set;}
}
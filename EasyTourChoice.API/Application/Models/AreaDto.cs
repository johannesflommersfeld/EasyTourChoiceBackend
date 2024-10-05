using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Models;

public class AreaDto
{
    public int AreaId { get; set; }

    public required string Name { get; set; }

    // derived fields
    public uint NumberOfTours { get; set; }

    public WeatherPreview? WeatherPreview { get; set; }

    public int? Temperature { get; set; }

    public WeatherForecastDto? WeatherForecast { get; set; }

    public AvelancheRisk? AvelancheRisk { get; set; }

    public AvalancheReportDto? AvalancheReport { get; set; }
}

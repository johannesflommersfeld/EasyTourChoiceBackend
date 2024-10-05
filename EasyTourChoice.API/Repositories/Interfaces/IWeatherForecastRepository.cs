using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IWeatherForecastRepository
{
    IEnumerable<WeatherForecast> GetAllReports();
    WeatherForecast? GetReportByLocation(Location location);
    void SaveForecast(Location location, WeatherForecast forecast);
}
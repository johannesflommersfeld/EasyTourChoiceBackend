using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IWeatherForecastRepository
{
    Task<List<WeatherForecast>> GetAllReportsAsync();
    Task<WeatherForecast?> GetReportByLocationAsync(Location location);
    Task<bool> SaveForecastAsync(Location location, WeatherForecast forecast);
    Task RemoveOutdatedForecastsAsync(TimeSpan maxAge);
}
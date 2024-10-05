using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Repositories;
public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly Dictionary<Location, WeatherForecast> _forecasts = [];

    public IEnumerable<WeatherForecast> GetAllReports()
    {
        return _forecasts.Values.ToList();
    }

    public WeatherForecast? GetReportByLocation(Location location)
    {
        return _forecasts[location];
    }

    public void SaveForecast(Location location, WeatherForecast forecast)
    {
        _forecasts[location] = forecast;
    }
}

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
        _forecasts.TryGetValue(location, out WeatherForecast? forecast);
        return forecast;
    }

    public void SaveForecast(Location location, WeatherForecast forecast)
    {
        _forecasts[location] = forecast;
    }
}

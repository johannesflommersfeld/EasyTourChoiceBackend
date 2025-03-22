using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyTourChoice.API.Repositories;
public class WeatherForecastRepository(TourDataContext context) : IWeatherForecastRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public List<WeatherForecast> GetAllReports()
    {
        return _context.WeatherForecasts
            .Include(w => w.Location)
            .ToList();
    }

    public WeatherForecast? GetReportByLocation(Location location)
    {
        // Round the location to match how it's stored when fetched from YR API
        var roundedLocation = LocationUtils.RoundLocation(location);
        var roundedLatitude = roundedLocation.Latitude;
        var roundedLongitude = roundedLocation.Longitude;

        return _context.WeatherForecasts
            .Include(w => w.Location)
            .FirstOrDefault(w =>
                w.Location != null &&
                Math.Abs(Math.Round(w.Location.Latitude, LocationUtils.ROUND_PRECISION) - roundedLatitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION) &&
                Math.Abs(Math.Round(w.Location.Longitude, LocationUtils.ROUND_PRECISION) - roundedLongitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION));
    }

    public void SaveForecast(Location location, WeatherForecast forecast)
    {
        if (location.LocationId is null)
        {
            
        }
        
        // Round the location to match how it's stored when fetched from YR API
        var roundedLocation = LocationUtils.RoundLocation(location);
        var roundedLatitude = roundedLocation.Latitude;
        var roundedLongitude = roundedLocation.Longitude;

        var existingForecast = _context.WeatherForecasts
            .Include(w => w.Location)
            .FirstOrDefault(w =>
                w.Location != null &&
                Math.Abs(Math.Round(w.Location.Latitude, LocationUtils.ROUND_PRECISION) - roundedLatitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION) &&
                Math.Abs(Math.Round(w.Location.Longitude, LocationUtils.ROUND_PRECISION) - roundedLongitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION));

        if (existingForecast is not null)
        {
            // Update existing forecast
            existingForecast.Meta = forecast.Meta;
            existingForecast.Timeseries = forecast.Timeseries;
        }
        else
        {
            forecast.LocationId = location.LocationId;
            _context.WeatherForecasts.Add(forecast);
        }

        _context.SaveChanges();
    }

    public void RemoveOutdatedForecasts(TimeSpan maxAge)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(maxAge);
        var outdatedForecasts = _context.WeatherForecasts
            .Where(w => w.Meta.UpdatedAt < cutoffTime)
            .ToList();

        if (outdatedForecasts.Any())
        {
            _context.WeatherForecasts.RemoveRange(outdatedForecasts);
            _context.SaveChanges();
        }
    }
}

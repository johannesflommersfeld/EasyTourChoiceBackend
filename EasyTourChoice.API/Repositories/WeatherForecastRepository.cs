using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyTourChoice.API.Repositories;
public class WeatherForecastRepository(TourDataContext context) : IWeatherForecastRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<WeatherForecast>> GetAllReportsAsync()
    {
        return await _context.WeatherForecasts
            .Include(w => w.Location)
            .ToListAsync();
    }

    public async Task<WeatherForecast?> GetReportByLocationAsync(Location location)
    {
        // Round the location to match how it's stored when fetched from YR API
        var roundedLocation = LocationUtils.RoundLocation(location);
        var roundedLatitude = roundedLocation.Latitude;
        var roundedLongitude = roundedLocation.Longitude;

        return await _context.WeatherForecasts
            .Include(w => w.Location)
            .FirstOrDefaultAsync(w =>
                w.Location != null &&
                Math.Abs(Math.Round(w.Location.Latitude, LocationUtils.ROUND_PRECISION) - roundedLatitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION) &&
                Math.Abs(Math.Round(w.Location.Longitude, LocationUtils.ROUND_PRECISION) - roundedLongitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION));
    }

    public async Task<bool> SaveForecastAsync(Location location, WeatherForecast forecast)
    {
        // Round the location to match how it's stored when fetched from YR API
        var roundedLocation = LocationUtils.RoundLocation(location);
        var roundedLatitude = roundedLocation.Latitude;
        var roundedLongitude = roundedLocation.Longitude;

        var existingForecast = await _context.WeatherForecasts
            .Include(w => w.Location)
            .FirstOrDefaultAsync(w =>
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

        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task RemoveOutdatedForecastsAsync(TimeSpan maxAge)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(maxAge);
        var outdatedForecasts = await _context.WeatherForecasts
            .Where(w => w.Meta.UpdatedAt < cutoffTime)
            .ToListAsync();

        if (outdatedForecasts.Count > 0)
        {
            _context.WeatherForecasts.RemoveRange(outdatedForecasts);
            await _context.SaveChangesAsync();
        }
    }
}

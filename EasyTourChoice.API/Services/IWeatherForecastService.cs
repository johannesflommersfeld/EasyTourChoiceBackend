using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public interface IWeatherForecastService
{
    Task<WeatherForecastDto?> GetWeatherForecastAsync(Location location);
}
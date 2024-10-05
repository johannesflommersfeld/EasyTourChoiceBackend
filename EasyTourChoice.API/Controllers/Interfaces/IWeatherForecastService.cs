using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IWeatherForecastService
{
    Task<WeatherForecastDto?> GetWeatherForecastAsync(Location location);
}
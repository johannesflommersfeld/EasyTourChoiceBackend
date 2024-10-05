using EasyTourChoice.API.Application.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface ILocationHandler
{
    Task<UpdateLocationResult> UpdateLocationAsync(int locationID, LocationForUpdateDto locationToPatch);
    Task<LocationDto?> GetLocationByIdAsync(int locationID);
    Task<bool> LocationExistsAsync(int locationID);
}

public record UpdateLocationResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
}
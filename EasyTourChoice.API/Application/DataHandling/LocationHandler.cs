using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataHandling;

public class LocationHandler(
    ILocationRepository locationRepository,
    IMapper mapper,
    ILogger<LocationHandler> logger
) : ILocationHandler
{
    private readonly ILocationRepository _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LocationHandler> _logger = logger;

    public async Task<LocationDto?> GetLocationByIdAsync(int locationID)
    {
        var location = await _locationRepository.GetLocationAsync(locationID);
        return _mapper.Map<LocationDto>(location);
    }

    public async Task<bool> LocationExistsAsync(int locationID)
    {
        return await _locationRepository.LocationExistsAsync(locationID);
    }

    public async Task<UpdateLocationResult> UpdateLocationAsync(int locationID, LocationForUpdateDto locationToPatch)
    {
        var result = new UpdateLocationResult();

        var location = await _locationRepository.GetLocationAsync(locationID);
        if (location == null)
        {
            result.IsNotFound = true;
            return result;
        }

        _mapper.Map(locationToPatch, location);
        await _locationRepository.SaveChangesAsync();

        result.IsSuccess = true;
        var msg = $"Area {locationID} was updated.";
        _logger.LogInformation("{msg}", msg);

        return result;
    }
}

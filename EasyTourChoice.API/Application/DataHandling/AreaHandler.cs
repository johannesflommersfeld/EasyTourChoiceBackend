using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataHandling;

public class AreaHandler(
    IAreaRepository areaRepository,
    IMapper mapper,
    ILogger<AreaHandler> logger
) : IAreaHandler
{
    private readonly IAreaRepository _areaRepository = areaRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AreaHandler> _logger = logger;

    public async Task<List<AreaDto>> GetAllAreasAsync()
    {
        var areaList = (await _areaRepository.GetAllAreasAsync()).ToList();

        return _mapper.Map<List<AreaDto>>(areaList);
    }

    public async Task<AreaDto?> GetAreaByIdAsync(int areaId)
    {
        // include weather and avalanche information
        var area = await _areaRepository.GetAreaByIdAsync(areaId);
        return _mapper.Map<AreaDto>(area);
    }

    public async Task<bool> AreaExistsAsync(int areaId)
    {
        return await _areaRepository.AreaExistsAsync(areaId);
    }

    public async Task UpdateAreasAsync()
    {
        await _areaRepository.SaveChangesAsync();
    }

    public async Task<UpdateAreaResult> UpdateAreaAsync(int areaID, AreaForUpdateDto areaToPatch)
    {
        var result = new UpdateAreaResult();

        if (string.IsNullOrEmpty(areaToPatch.Name))
        {
            result.IsBadRequest = true;
            result.ModelState.AddModelError("Name", "Name is required.");
            return result;
        }

        var area = await _areaRepository.GetAreaByIdAsync(areaID);
        if (area == null)
        {
            result.IsNotFound = true;
            return result;
        }

        _mapper.Map(areaToPatch, area);
        await _areaRepository.SaveChangesAsync();

        result.IsSuccess = true;
        var msg = $"Area {areaID} was updated.";
        _logger.LogInformation("{msg}", msg);

        return result;
    }
}
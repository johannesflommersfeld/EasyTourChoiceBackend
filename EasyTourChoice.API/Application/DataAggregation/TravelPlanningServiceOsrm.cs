using System.Text.Json;
using System.Text.Json.Serialization;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using AutoMapper;

namespace EasyTourChoice.API.Application.DataAggregation;

public class TravelPlanningServiceOsrm(
    ILogger<TravelPlanningServiceOsrm> logger,
    IHttpService httpService,
    ITravelInformationRepository travelInfoRepository,
    IMapper mapper
) : ITravelPlanningService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;
    private readonly ITravelInformationRepository _travelInfoRepository = travelInfoRepository;
    private readonly IMapper _mapper = mapper;


    public async Task<TravelInformationDto?> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation,
        bool httpOnly=false)
    {
        if (targetLocation.LocationId is null)
        {
            return null;
        }
        
        var travelInfos = httpOnly ? null : await _travelInfoRepository.GetTravelInformationAsync(currentLocation, (int)targetLocation.LocationId);
        if (travelInfos is null 
            || Math.Abs(travelInfos.TravelTime) < float.Epsilon 
            || Math.Abs(travelInfos.TravelDistance) < float.Epsilon 
            || travelInfos.Route is null 
            || travelInfos.Route.Count == 0)
        {
            await using Stream stream =
                await _httpService.PerformGetRequestAsync(GetUrl(currentLocation, targetLocation, false));

            travelInfos = new()
            {
                TargetLocationId = targetLocation.LocationId,
                StartingLocation = currentLocation,
                TravelTime = 0,
                TravelDistance = 0
            };
            try
            {
                var response = await JsonSerializer.DeserializeAsync<OSRMResponse>(stream);
                // convert to kilometers
                travelInfos.TravelDistance = response?.Routes[0].Legs[0].Distance / 1_000 ?? 0;
                // convert to hours
                travelInfos.TravelTime = response?.Routes[0].Legs[0].Duration / 3_600 ?? 0;
                if(!httpOnly && !await _travelInfoRepository.SaveTravelInformationAsync(travelInfos))
                {
                    _logger.LogError("Failed to save travel information in the database.");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("{Message}", e.Message);
                return null;
            }
        }

        return _mapper.Map<TravelInformationDto>(travelInfos);
    }

    public async Task<TravelInformationDto?> GetCachedTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        if (targetLocation.LocationId is null)
        {
            return null;
        }
        
        var travelInfos = await _travelInfoRepository.GetTravelInformationAsync(currentLocation, (int)targetLocation.LocationId);
        return _mapper.Map<TravelInformationDto>(travelInfos);
    }

    
    public async Task<TravelInformationWithRouteDto?> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        if (targetLocation.LocationId is null)
        {
            return null;
        }
        
        var travelInfos = await _travelInfoRepository.GetTravelInformationAsync(currentLocation, (int)targetLocation.LocationId);
        if (travelInfos is null 
            || Math.Abs(travelInfos.TravelTime) < float.Epsilon 
            || Math.Abs(travelInfos.TravelDistance) < float.Epsilon 
            || travelInfos.Route is null 
            || travelInfos.Route.Count == 0)
        {
            await using Stream stream =
                await _httpService.PerformGetRequestAsync(GetUrl(currentLocation, targetLocation, true));
            travelInfos = new()
            {
                TargetLocationId = targetLocation.LocationId,
                StartingLocation = currentLocation,
                TravelTime = 0,
                TravelDistance = 0,
                Route = null
            };
            try
            {
                var response = await JsonSerializer.DeserializeAsync<OSRMResponse>(stream);
                // convert to kilometers
                travelInfos.TravelDistance = response?.Routes[0].Legs[0].Distance / 1_000 ?? 0;
                // convert to hours
                travelInfos.TravelTime = response?.Routes[0].Legs[0].Duration / 3_600 ?? 0;
                travelInfos.Route = response?.Routes[0]?.Geometry?.ConvertToLocations();
                if(!await _travelInfoRepository.SaveTravelInformationAsync(travelInfos))
                {
                    _logger.LogError("Failed to save travel information in the database.");
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("{Message}", e.Message);
                return null;
            }
        }

        return _mapper.Map<TravelInformationWithRouteDto>(travelInfos);
    }

    private string GetUrl(Location currentLocation, Location targetLocation, bool withOverview)
    {
        // TODO: move string to configuration file
        return string.Format("http://router.project-osrm.org/route/v1/driving/{0},{1};{2},{3}?geometries=geojson&overview={4}&alternatives=false&skip_waypoints=true",
                currentLocation.Longitude,
                currentLocation.Latitude,
                targetLocation.Longitude,
                targetLocation.Latitude,
                withOverview ? "simplified" : "false");
    }
}

internal record class OSRMResponse
{
    [property: JsonPropertyName("code")]
    public string ResponseCode { get; set; } = ""; // HTTP response code

    [property: JsonPropertyName("routes")]
    public List<OSRMRoute> Routes { get; set; } = [];
}

internal record class OSRMRoute
{
    [property: JsonPropertyName("legs")]
    public List<OSRMLeg> Legs { get; set; } = [];

    [property: JsonPropertyName("geometry")]
    public OSRMGeometry? Geometry { get; set; } = null;
}

internal record class OSRMLeg
{
    [property: JsonPropertyName("distance")]
    public float Distance { get; set; } // travelling distance in meters

    [property: JsonPropertyName("duration")]
    public float Duration { get; set; } // travelling time in seconds
}

internal record class OSRMGeometry
{
    [property: JsonPropertyName("coordinates")]
    public List<List<float>> Coordinates { get; set; } = [];

    public List<Location> ConvertToLocations()
    {
        List<Location> locationList = [];
        foreach (var coordinate in Coordinates)
        {
            var location = new Location()
            {
                Longitude = coordinate[0],
                Latitude = coordinate[1],
            };
            locationList.Add(location);
        }
        return locationList;
    }
}
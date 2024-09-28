using System.Text.Json;
using System.Text.Json.Serialization;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public class TravelPlanningServiceOSRM(
    ILogger<TravelPlanningServiceOSRM> logger,
    IHttpService httpService
) : ITravelPlanningService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;


    public async Task<TravelInformationDto> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        await using Stream stream =
            await _httpService.PerformGetRequestAsync(GetURL(currentLocation, targetLocation, false));

        TravelInformationDto travelInformationDto = new()
        {
            TargetLocationId = targetLocation.LocationId,
            StartingLocation = currentLocation,
            TravelTime = 0,
            TravelDistance = 0
        };
        try
        {
            var response = await JsonSerializer.DeserializeAsync<OSRMResponse>(stream);
            // convert to meters
            travelInformationDto.TravelDistance = response?.Routes[0].Legs[0].Distance / 1_000 ?? 0;
            // convert to hours
            travelInformationDto.TravelTime = response?.Routes[0].Legs[0].Duration / 3_600 ?? 0;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }

        return travelInformationDto;
    }

    public async Task<TravelInformationWithRouteDto> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        await using Stream stream =
            await _httpService.PerformGetRequestAsync(GetURL(currentLocation, targetLocation, true));
        TravelInformationWithRouteDto travelInformationDto = new()
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
            // convert to meters
            travelInformationDto.TravelDistance = response?.Routes[0].Legs[0].Distance / 1_000 ?? 0;
            // convert to hours
            travelInformationDto.TravelTime = response?.Routes[0].Legs[0].Duration / 3_600 ?? 0;
            travelInformationDto.Route = response?.Routes[0]?.Geometry?.ConvertToLocations();
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }

        return travelInformationDto;
    }

    private string GetURL(Location currentLocation, Location targetLocation, bool with_overview)
    {
        // TODO: move string to configuration file
        return string.Format("http://router.project-osrm.org/route/v1/driving/{0},{1};{2},{3}?geometries=geojson&overview={4}&alternatives=false&skip_waypoints=true",
                currentLocation.Longitude,
                currentLocation.Latitude,
                targetLocation.Longitude,
                targetLocation.Latitude,
                with_overview ? "simplified" : "false");
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

    public List<LocationDto> ConvertToLocations()
    {
        List<LocationDto> locationList = [];
        foreach (var coordinate in Coordinates)
        {
            var location = new LocationDto()
            {
                Latitude = coordinate[0],
                Longitude = coordinate[1],
            };
            locationList.Add(location);
        }
        return locationList;
    }
}
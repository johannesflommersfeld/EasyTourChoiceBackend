using Newtonsoft.Json;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models.BaseModels;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Repositories.Interfaces;
using YamlDotNet.Serialization;

namespace EasyTourChoice.API.Domain;

public class EawsRegionService : IAvalancheRegionService
{
    private readonly ILogger _logger;
    private readonly IHttpService _httpService;
    private readonly IAvalancheRegionsRepository _avalancheRegionsRepository;
    private readonly Dictionary<string, string> _regionNames;

    public EawsRegionService(
        ILogger<EawsRegionService> logger,
        IHttpService httpService,
        IAvalancheRegionsRepository avalancheRegionsRepository
    )
    {
        _logger = logger;
        _httpService = httpService;
        _avalancheRegionsRepository = avalancheRegionsRepository;

        // TODO: include proper resource handling
        string yamlContent = File.ReadAllText("Resources/regions.yaml");
        var deserializer = new DeserializerBuilder().Build();
        _regionNames = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);
    }

    public async Task<string?> GetRegionIDAsync(LocationBase location)
    {
        var regions = await _avalancheRegionsRepository.GetAllRegionsAsync();
        if (regions.Count == 0)
        {
            await LoadRegionsAsync();
            regions = await _avalancheRegionsRepository.GetAllRegionsAsync();
        }

        var regionId = SearchInRegions(location, regions);
        if (regionId is not null) return regionId;

        // check if new regions were added.
        await LoadRegionsAsync();
        regions = await _avalancheRegionsRepository.GetAllRegionsAsync();
        return SearchInRegions(location, regions);
    }

    private string? SearchInRegions(LocationBase location, IEnumerable<AvalancheRegion> regions)
    {
        foreach (AvalancheRegion region in regions)
        {
            foreach (ICollection<ICollection<double>> polygon in region.Polygons)
            {
                var polygonPoints = polygon
                    .Select(p => p.ToList())
                    .Select(p => new LocationBase() { Longitude = p[0], Latitude = p[1] })
                    .ToList();

                if (IsInPolygon(location, polygonPoints))
                {
                    return region.Id;
                }
            }
        }
        return null;
    }

    private async Task LoadRegionsAsync()
    {
        await using var stream = await _httpService.PerformGetRequestAsync(GetUrl());
        await using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            var eawsRegions = serializer.Deserialize<EAWSRegionsDto>(reader)
                ?? throw new NullReferenceException("Failed to load EAWS regions.");
            
            var listOfTasks = new List<Task>();
            foreach (var feature in eawsRegions.Features)
            {
                listOfTasks.Add(_avalancheRegionsRepository.SaveRegionAsync(
                    new AvalancheRegion()
                    {
                        Id = feature.Properties.Id,
                        Type = feature.Geometry.Type,
                        Polygons = feature.Geometry.Coordinates.First(),
                    }));
            }
            await Task.WhenAll(listOfTasks);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }
    }

    public string GetRegionName(string id)
    {
        return _regionNames[id];
    }

    private static bool IsInPolygon(LocationBase location, List<LocationBase> polygon)
    {
        // Implements winding number algorithm.
        // see https://web.archive.org/web/20130126163405/http://geomalgorithms.com/a03-_inclusion.html
        var windingNumber = 0;
        for (var i = 0; i < polygon.Count; i++)
        {
            var p1 = polygon[i];
            var p2 = polygon[(i + 1) % polygon.Count];
            // Filter out edges that cannot be crossed by horizontal ray through the location.
            if (((location.Latitude > p1.Latitude) && (location.Latitude >= p2.Latitude))
                || ((location.Latitude < p1.Latitude) && (location.Latitude <= p2.Latitude)))
            {
                continue;
            }

            // Only consider edges to the "right" of the location.
            if ((location.Longitude < p1.Longitude) && (location.Longitude <= p2.Longitude))
            {
                continue;
            }

            // Determine on which side of the edge, the point is located
            // see https://www.cs.drexel.edu/~deb39/Classes/Papers/comp175-06-pineda.pdf
            var edgeFunctionValue =
                (location.Latitude - p1.Latitude) * (p2.Longitude - p1.Longitude)
                - (location.Longitude - p1.Longitude) * (p2.Latitude - p1.Latitude);
            windingNumber += edgeFunctionValue > 0 ? 1 : -1;
        }
        return Math.Abs(windingNumber) % 2 == 1;
    }

    private static string GetUrl()
    {
        // TODO: move to configuration file
        return "https://regions.avalanches.org/micro-regions.geojson";
    }
}
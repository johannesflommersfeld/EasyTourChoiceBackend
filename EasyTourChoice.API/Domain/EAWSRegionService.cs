using Newtonsoft.Json;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models.BaseModels;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Repositories.Interfaces;
using YamlDotNet.Serialization;

namespace EasyTourChoice.API.Domain;

public class EAWSRegionService : IAvalancheRegionService
{
    private readonly ILogger _logger;
    private readonly IHttpService _httpService;
    private readonly IAvalancheRegionsRepository _avalancheRegionsRepository;
    private readonly Dictionary<string, string> _regionNames;

    public EAWSRegionService(
        ILogger<EAWSRegionService> logger,
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
        var regions = _avalancheRegionsRepository.GetAllRegions();
        if (!regions.Any())
        {
            await LoadRegionsAsync();
            regions = _avalancheRegionsRepository.GetAllRegions();
        }
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
        await using Stream stream = await _httpService.PerformGetRequestAsync(GetURL());
        using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            var eawsRegions = serializer.Deserialize<EAWSRegionsDto>(reader)
                ?? throw new NullReferenceException("Failed to load EAWS regions.");
            foreach (var feature in eawsRegions.Features)
            {
                _avalancheRegionsRepository.SaveRegion(
                    new AvalancheRegion()
                    {
                        Id = feature.Properties.Id,
                        Type = feature.Geometry.Type,
                        Polygons = feature.Geometry.Coordinates.First(),
                    });
            }
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
        int windingNumber = 0;
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
            var EdgeFunctionValue =
                (location.Latitude - p1.Latitude) * (p2.Longitude - p1.Longitude)
                - (location.Longitude - p1.Longitude) * (p2.Latitude - p1.Latitude);
            windingNumber += EdgeFunctionValue > 0 ? 1 : -1;
        }
        return Math.Abs(windingNumber) % 2 == 1;
    }

    private string GetURL()
    {
        // TODO: move to configuration file
        return "https://regions.avalanches.org/micro-regions.geojson";
    }
}
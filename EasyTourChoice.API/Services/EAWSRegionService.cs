using Newtonsoft.Json;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models.BaseModels;

namespace EasyTourChoice.API.Services;

public class EAWSRegionService(
    ILogger<EAWSRegionService> logger,
    IHttpService httpService
) : IHostedService
{
    public EAWSRegions? Regions { get; private set; }

    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;
    private readonly Task _completedTask = Task.CompletedTask;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return LoadRegionsAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Regions = null;
        return _completedTask;
    }

    public string? GetRegionID(LocationBase location)
    {
        if (Regions is null)
        {
            throw new NullReferenceException("Regions must be loaded before EAWSRegionService.GetRegionID() can be called.");
        }
        foreach (Feature feature in Regions.Features)
        {
            foreach (ICollection<ICollection<double>> polygon in feature.Geometry!.Coordinates.First().Cast<ICollection<ICollection<double>>>())
            {
                var polygonPoints = polygon
                    .Select(p => p.ToList())
                    .Select(p => new LocationBase() { Longitude = p[0], Latitude = p[1] })
                    .ToList();

                if (IsInPolygon(location, polygonPoints))
                {
                    return feature.Properties.Id;
                }
            }
        }
        return null;
    }

    public async Task LoadRegionsAsync()
    {
        await using Stream stream = await _httpService.PerformGetRequestAsync(GetURL());
        using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            Regions = serializer.Deserialize<EAWSRegions>(reader);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }
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
        return windingNumber % 2 == 1;
    }

    private string GetURL()
    {
        // TODO: move to configuration file
        return "https://regions.avalanches.org/micro-regions.geojson";
    }
}
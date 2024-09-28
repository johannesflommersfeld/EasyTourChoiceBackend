using Newtonsoft.Json;
using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;
using AutoMapper;

namespace EasyTourChoice.API.Services;

public class EAWSReportService(
    ILogger<EAWSReportService> logger,
    IHttpService httpService,
    IMapper mapper
) : IHostedService
{
    private Dictionary<string, EAWSBulletin>? _reports = null;
    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;
    private readonly IMapper _mapper = mapper;
    private readonly Task _completedTask = Task.CompletedTask;

    public async Task<AvalancheReportDto?> GetLatestAvalancheReportAsync(string regionID)
    {
        if (_reports is null || !_reports[regionID].IsValid())
        {
            await FetchLatestAvalancheReportAsync();
        }

        if (_reports is null)
        {
            return null;
        }

        var report = _mapper.Map<AvalancheReportDto>(_reports?[regionID]);
        report.RegionName = _reports?[regionID]?.Regions.First(r => r.RegionID == regionID).Name;
        return report;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // set this as environment variable
        return FetchLatestAvalancheReportAsync("de");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _reports = null;
        return _completedTask;
    }

    private async Task FetchLatestAvalancheReportAsync(string language = "de")
    {
        await using Stream stream = await _httpService.PerformGetRequestAsync(GetURL(language));
        using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            var bulletins = (serializer.Deserialize<EAWSReport>(reader)?.Bulletins)
                ?? throw new JsonException("Could not deserialize the latest avalanche report.");
            _reports = [];
            foreach (var bulletin in bulletins)
            {
                foreach (var region in bulletin.Regions)
                {
                    _reports[region.RegionID] = bulletin;
                }
            }
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("{Message}", e.Message);
        }
        catch (JsonException e)
        {
            _logger.LogError("{Message}", e.Message);
        }
    }

    private string GetURL(string language)
    {
        // TODO: move to configuration file
        return $"https://static.avalanche.report/bulletins/latest/{language}_CAAMLv6.json";
    }
}
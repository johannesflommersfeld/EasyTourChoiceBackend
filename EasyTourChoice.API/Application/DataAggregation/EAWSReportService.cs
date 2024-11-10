using AutoMapper;
using Newtonsoft.Json;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.DataAggregation;

public class EAWSReportService(
    ILogger<EAWSReportService> logger,
    IHttpService httpService,
    IMapper mapper,
    IAvalancheReportsRepository avalancheReportsRepository
) : IAvalancheReportService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpService _httpService = httpService;
    private readonly IMapper _mapper = mapper;
    private readonly IAvalancheReportsRepository _reportsRepository = avalancheReportsRepository;

#if DEBUG
    public async Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionID, bool mustBeValid=true)
#else
    public async Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionID)
#endif
    {
        var report = _reportsRepository.GetReportByRegionID(regionID);
        if (report is null || !report.IsValid())
        {
            await FetchLatestAvalancheReportAsync();
            report = _reportsRepository.GetReportByRegionID(regionID);
        }

        if (report is null)
            return null;

        if (mustBeValid && !report.IsValid())
            return null;

        return _mapper.Map<AvalancheReportDto>(report);
    }

    private async Task FetchLatestAvalancheReportAsync(string language = "de")
    {
        await using Stream stream = await _httpService.PerformGetRequestAsync(GetURL(language));
        using var reader = new JsonTextReader(new StreamReader(stream));
        try
        {
            var serializer = new JsonSerializer();
            var bulletins = (serializer.Deserialize<EAWSReportDto>(reader)?.Bulletins)
                ?? throw new JsonException("Could not deserialize the latest avalanche report.");

            foreach (var bulletin in bulletins)
            {
                foreach (var region in bulletin.Regions)
                {
                    _reportsRepository.SaveReport(region.RegionID, _mapper.Map<AvalancheReport>(bulletin));
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
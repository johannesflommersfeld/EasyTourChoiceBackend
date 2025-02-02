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
    public async Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionID, bool mustBeValid = true)
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

    private async Task FetchLatestAvalancheReportAsync(string language = "en")
    {
        var listOfTasks = new List<Task>();
        foreach (var url in GetURLs(language))
        {
            listOfTasks.Add(FetchLatestAvalancheReportFromURLAsync(url));
        }
        await Task.WhenAll(listOfTasks);
    }

    private async Task FetchLatestAvalancheReportFromURLAsync(string url)
    {
        using Stream stream = await _httpService.PerformGetRequestAsync(url);
        try
        {
            using var reader = new JsonTextReader(new StreamReader(stream));
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

    private string[] GetURLs(string language)
    {
        string datePattern = "yyyy-MM-dd";
        var date = DateTime.Now.ToString(datePattern);
        return [
            $"https://static.avalanche.report/bulletins/latest/EUREGIO_{language}_CAAMLv6.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-02.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-03.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-04.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-05.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-06.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-08.json",
            $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-DE-BY.json",
            ];
    }
}
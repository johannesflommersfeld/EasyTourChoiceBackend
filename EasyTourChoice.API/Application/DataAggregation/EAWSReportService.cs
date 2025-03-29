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
    public async Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionId, bool mustBeValid = true)
#else
    public async Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionID)
#endif
    {
        var report = await _reportsRepository.GetReportByRegionIdAsync(regionId);
        if (report is null || !report.IsValid())
        {
            await FetchLatestAvalancheReportAsync();
            report = await _reportsRepository.GetReportByRegionIdAsync(regionId);
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
            var bulletins = serializer.Deserialize<EAWSReportDto>(reader)?.Bulletins
                ?? throw new JsonException("Could not deserialize the latest avalanche report.");

            List<Task> saveTasks = [];
            foreach (var bulletin in bulletins)
            {
                foreach (var region in bulletin.Regions)
                {
                    var report = _mapper.Map<AvalancheReport>(bulletin);
                    report.RegionId = region.RegionID;
                    report.RegionName = region.Name;
                    saveTasks.Add(_reportsRepository.SaveReportAsync(report));
                }
            }
            await Task.WhenAll(saveTasks);
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
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-02.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-03.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-04.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-05.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-06.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AT-08.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-DE-BY.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-CH.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-FR.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-GB.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-NO.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-PL-12.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-RO.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-SE.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-SI.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-SK.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-FL.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-FL.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-AD.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-ES.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-ES-CT.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-ES-CT-L.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IS.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-21.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-25.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-34.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-36.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-57.json",
            // $"https://static.avalanche.report/eaws_bulletins/{date}/{date}-IT-MeteoMont.json",
            ];
    }
}
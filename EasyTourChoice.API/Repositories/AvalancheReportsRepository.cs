using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Serialization.TypeInspectors;

namespace EasyTourChoice.API.Repositories;
public class AvalancheReportsRepository(TourDataContext context) : IAvalancheReportsRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<IEnumerable<AvalancheReport>> GetAllReports()
    {
        return await _context.AvalancheReports.ToListAsync();
    }

    public async Task<AvalancheReport?> GetReportByRegionID(string regionId)
    {
        return await _context.AvalancheReports.SingleOrDefaultAsync(r => r.RegionId == regionId);
    }

    public async Task SaveReport(string id, AvalancheReport report)
    {
        foreach (var dangerRating in report.DangerRatings)
        {
            dangerRating.AvalancheReports.Add(report);
        }

        foreach (var problem in report.AvalancheProblems)
        {
            problem.AvalancheReports.Add(report);
        }
            
        await _context.AvalancheReports.AddAsync(report);
        await _context.DangerRatings.AddRangeAsync(report.DangerRatings);
        await _context.AvalancheProblems.AddRangeAsync(report.AvalancheProblems);
    }
}

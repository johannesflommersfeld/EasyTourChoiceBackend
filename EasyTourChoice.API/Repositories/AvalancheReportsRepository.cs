using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyTourChoice.API.Repositories;
public class AvalancheReportsRepository(TourDataContext context) : IAvalancheReportsRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<AvalancheReport>> GetAllReportsAsync()
    {
        return await _context.AvalancheReports.ToListAsync();
    }

    public async Task<AvalancheReport?> GetReportByRegionIdAsync(string regionId)
    {
        return await _context.AvalancheReports
            .AsNoTracking()
            .Include(r => r.AvalancheProblems)
            .Include(r => r.DangerRatings)
            .SingleOrDefaultAsync(r => r.RegionId == regionId);
    }

    public async Task SaveReportAsync(AvalancheReport report)
    {
        foreach (var dangerRating in report.DangerRatings)
        {
            dangerRating.AvalancheReports.Add(report);
        }

        foreach (var problem in report.AvalancheProblems)
        {
            problem.AvalancheReports.Add(report);
        }

        var existingReport = _context.AvalancheReports
            .Include(r => r.AvalancheProblems)
            .Include(r => r.DangerRatings)
            .FirstOrDefault(r => r.RegionId == report.RegionId);
        if (existingReport is not null)
        {
            _context.DangerRatings.RemoveRange(existingReport.DangerRatings);
            _context.AvalancheProblems.RemoveRange(existingReport.AvalancheProblems);
            _context.AvalancheReports.Remove(existingReport);
            await _context.SaveChangesAsync();
        }
        await _context.AvalancheReports.AddAsync(report);
        await _context.DangerRatings.AddRangeAsync(report.DangerRatings);
        await _context.AvalancheProblems.AddRangeAsync(report.AvalancheProblems);


        await _context.SaveChangesAsync();
    }
}

using EasyTourChoice.API.DbContexts;
namespace EasyTourChoice.API.Application.DataAggregation;

public class AvalancheReportCleanupService : IHostedService, IDisposable
{
    private readonly TourDataContext _context;
    private readonly ILogger<AvalancheReportCleanupService> _logger;
    private Timer? _timer;

    public AvalancheReportCleanupService(TourDataContext context, ILogger<AvalancheReportCleanupService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Avalanche report cleanup service started.");

        var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone);

        // Schedule cleanup at 18h05 CET
        var nextRunTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 18, 5, 0);
        if (nextRunTime < currentTime)
        {
            nextRunTime = nextRunTime.AddDays(1);
        }
        var delay = nextRunTime - currentTime;
        _timer = new Timer(CleanupExpiredAvalancheReports!, null, delay, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void CleanupExpiredAvalancheReports(object state)
    {
        try
        {
            await _context.CleanupExpiredAvalancheReportsAsync();
            _logger.LogInformation("Avalanche report cleanup completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during avalanche report cleanup.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Avalanche report cleanup service stopped.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
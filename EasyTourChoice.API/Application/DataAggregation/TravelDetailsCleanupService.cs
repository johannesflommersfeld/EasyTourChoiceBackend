using EasyTourChoice.API.DbContexts;
namespace EasyTourChoice.API.Application.DataAggregation;

public class TravelDetailsCleanupService: IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TravelDetailsCleanupService> _logger;
    private Timer? _timer;

    public TravelDetailsCleanupService(IServiceScopeFactory scopeFactory, ILogger<TravelDetailsCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Avalanche report cleanup service started.");

        var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone);

        // Schedule cleanup at 03h00 CET
        var nextRunTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 3, 0, 0);
        if (nextRunTime < currentTime)
        {
            nextRunTime = nextRunTime.AddDays(1);
        }
        var delay = nextRunTime - currentTime;
        _timer = new Timer(CleanupTravelDetails!, null, delay, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void CleanupTravelDetails(object state)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TourDataContext>();
            await context.DropTravelDetails();
            _logger.LogInformation("Travel details cleanup completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during travel details cleanup.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Travel details cleanup service stopped.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
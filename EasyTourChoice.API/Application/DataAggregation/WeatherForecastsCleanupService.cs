using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataAggregation;

public class WeatherForecastsCleanupService(
    ILogger<WeatherForecastsCleanupService> logger,
    IServiceScopeFactory scopeFactory) : IHostedService, IDisposable
{
    private readonly ILogger<WeatherForecastsCleanupService> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private Timer? _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Weather report cleanup service started.");

        var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetTimeZone);

        // Schedule cleanup every hour
        _timer = new Timer(CleanupExpiredWeatherForecasts!, null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }
    private async void CleanupExpiredWeatherForecasts(object state)
    {
        _logger.LogInformation("Weather Forecasts Cleanup Service running.");


            _logger.LogInformation("Running outdated weather forecasts cleanup");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var weatherForecastRepository = scope.ServiceProvider.GetRequiredService<IWeatherForecastRepository>();
                // Set the max age to 1 hour for weather forecasts
                weatherForecastRepository.RemoveOutdatedForecasts(TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during weather forecasts cleanup");
            }

            await Task.Delay(TimeSpan.FromHours(1));
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Weather report cleanup service stopped.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}